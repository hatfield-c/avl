import sys
import time
import threading
import traci
import traceback

import SumoManager
import TcpServer
import TcpCommands
import Cli

from queue import Queue

class AvlApplication:
    def __init__(self, ip, port, sumoBinaryPath, networkPath):

        Cli.printHeading3("SUMO")
        print("Starting SUMO...")

        self.sumoManager = SumoManager.SumoManager(sumoBinaryPath, networkPath)

        Cli.printLine(1, "SUMO is running!")
        print("SUMO has been initialized successfully.")

        self.serverIP = ip
        self.serverPort = port

        Cli.printHeading3("TCP Server")
        print("Initializing TCP server...")

        self.server = TcpServer.TcpServer(self.serverIP, self.serverPort, 60)

        Cli.printLine(1, "TCP server initialized!")

        print("Starting TCP server...")
        Cli.printLine(1, "Attempting to open TCP server at [" + str(self.server.IP) + ":" + str(self.server.port) + "]...")

        self.server.startServer()

        Cli.printLine(2, "TCP server opened!")
        Cli.printLine(1, "TCP server started!")
        print("TCP server is ready.")

    def main(self):

        Cli.printHeading3("Simulation")
        print("Initializing Unity...")
        self.sumoManager.initUnity(self.server)

        Cli.printLine(1, "Ready to simulate in Unity!")

        print("Begin simulation.")

        deltaT = 0.02

        while True:

            TiStamp1 = time.time()

            self.sumoManager.stepSumo()
            self.sumoManager.sendStateToUnity(self.server)
                    
            TiStamp2 = time.time() - TiStamp1
            if TiStamp2 > deltaT:
                pass
            else:
                time.sleep(deltaT-TiStamp2)

    def buildMessage(
        self,
        destinationCode,
        commandCode,
        data
    ):
        if data is None:
            return None

        return destinationCode + TcpCommands.MSG_DELIM + commandCode + TcpCommands.MSG_DELIM + data + "\n"

IP = 'localhost'
port = 4042
sumoBinaryPath = "C:/Sumo/bin/sumo-gui"
networkPath = "../SUMO_Networks/Rectangle/Network_01.sumocfg"

Cli.printHeading1("UTD Autonomous Vehicle Lab (AVL)")

print("Welcome! The application will now initialize.")

try:
    avl = AvlApplication(IP, port, sumoBinaryPath, networkPath)
    avl.main()

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
