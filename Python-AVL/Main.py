import sys
import time
import threading
import traci
import traceback

import TrafficSimulator
import TCP_server
import Cli

from queue import Queue

class SumoUnity:
    def __init__(self, IP, Port, SumoNetwork):

        self.NetworkName = SumoNetwork
        self.UnityQueue = Queue(maxsize=1)

        Cli.printHeading3("SUMO")
        print("Starting SUMO...")

        self.TrafficSim = TrafficSimulator.TrafficSimulator(self.NetworkName)

        Cli.printLine(1, "SUMO is running!")

        print("Parsing traffic lights...")

        self.TrafficLights = self.TrafficSim.ParseTrafficLights()
        self.SumoObjects = []

        Cli.printLine(1, "Parsed!")
        print("SUMO has been initialized successfully.")

        self.ServerIP = IP
        self.ServerPort = Port

        Cli.printHeading3("TCP Server")
        print("Initializing TCP server...")

        self.Server = TCP_server.TCP_Server(self.ServerIP, self.ServerPort)

        Cli.printLine(1, "TCP server initialized!")

        print("Starting TCP server...")
        Cli.printLine(1, "Attempting to open TCP server at [" + str(self.Server.IP) + ":" + str(self.Server.port) + "]...")

        self.Server.StartServer(self.UnityQueue)

        Cli.printLine(2, "TCP server opened!")
        Cli.printLine(1, "TCP server started!")
        print("TCP server is ready.")

    def main(self):

        Cli.printHeading3("Simulation")
        print("Begin simulation.")

        deltaT = 0.02

        while True:

            TiStamp1 = time.time()

            self.SumoObjects, self.TrafficLights = self.TrafficSim.StepSumo(self.SumoObjects, self.TrafficLights)

            self.enqueData(self.SumoObjects, self.TrafficLights)

            while not self.UnityQueue.empty():

                msg = self.UnityQueue.get()
                self.Server.UnityClient.send(msg.encode())
                    
            TiStamp2 = time.time() - TiStamp1
            if TiStamp2 > deltaT:
                pass
            else:
                time.sleep(deltaT-TiStamp2)

    def enqueData(self, Vehicles, TrafficLights):

        DataToUnity = "O1G"

        for veh in Vehicles:
            DataToUnity += veh.ID + ";"
            DataToUnity += "{0:.3f}".format(veh.PosX_Center) + ";"
            DataToUnity += "{0:.3f}".format(veh.PosY_Center) + ";"
            DataToUnity += "{0:.2f}".format(veh.Velocity) + ";"
            DataToUnity += "{0:.2f}".format(veh.Heading) + ";"
            DataToUnity += str(int(veh.StBrakePedal)) + ";"
            DataToUnity += str(veh.SizeClass) + ";"
            DataToUnity += str(veh.Color) + "@"

        for tls in TrafficLights:
            pass

        DataToUnity = DataToUnity + "&\n"

        with self.UnityQueue.mutex:
            self.UnityQueue.queue.clear()
        
        self.UnityQueue.put(DataToUnity)

IP = 'localhost'
port = 4042
SumoNetwork = "Rectangle/Network_01.sumocfg"

Cli.printHeading1("UTD Autonomous Vehicle Lab (AVL)")

print("Welcome! The application will now initialize.")

try:
    Simulation = SumoUnity(IP, port, SumoNetwork)
    Simulation.main()

    Cli.printHeading2("Exit Application")
    print("Exited normally.")
except KeyboardInterrupt:
    Cli.printHeading2("Exit Application")
    print("Exited via keyboard interrupt.")
except traci.exceptions.FatalTraCIError as traciException:
    Cli.printHeading2("Exit Application")
    print("SUMO is no longer connected. Cause: ")
    Cli.printLine(1, traciException)
except:
    Cli.printHeading2("Exit Application")
    print("Uncaught exception!\n")
    traceback.print_exception(*sys.exc_info())
