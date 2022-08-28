import sys
import time
import threading
import traci
import traceback

import SumoManager

import SumoServer
import SumoListener
import Cli
import Config

class AvlApplication:

    def __init__(self):

        Cli.printHeading3("Initialization")

        print("Initializing SUMO Listener...")
        self.sumoListener = SumoListener.SumoListener(timeout = 60)
        Cli.printLine(1, "SUMO Listener initialized!\n")
        

        print("Initializing SUMO Server...")
        self.sumoServer = SumoServer.SumoServer(timeout = 60)
        Cli.printLine(1, "SUMO Server initialized!\n")
        

        print("Starting SUMO...")
        self.sumoManager = SumoManager.SumoManager(self.sumoListener)        
        Cli.printLine(1, "SUMO is running!\n")

        print("The application has been initialized successfully.")

        Cli.printHeading3("TCP Communication")
        
        print("Starting SUMO Server...")
        Cli.printLine(1, "Waiting for Unity Listener at [" + Config.TCP_IP_ADDR + ":" + Config.TCP_PORT_TO_UNITY + "]...")

        self.sumoServer.startServing()

        Cli.printLine(2, "SUMO Server connected to Unity Listener!")
        Cli.printLine(1, "SUMO Server started!")
        print("SUMO Server is ready.\n")

        print("Starting SUMO Listener...")
        Cli.printLine(1, "Waiting for Unity Server at [" + Config.TCP_IP_ADDR + ":" + Config.TCP_PORT_TO_SUMO + "]...")

        self.sumoListener.startListening(self.sumoManager)

        Cli.printLine(2, "SUMO Listener connected to Unity Server!")
        Cli.printLine(1, "SUMO Listener started!")
        print("SUMO Listener is ready.")

    def main(self):

        Cli.printHeading3("Simulation")
        print("Initializing Unity...")
        self.sumoManager.initUnity(self.sumoServer)

        Cli.printLine(1, "Ready to simulate in Unity!")

        print("Begin simulation.")

        while True:

            self.sumoManager.stepSumo()
            self.sumoManager.processUnityMessages(self.sumoListener)
            self.sumoManager.sendStateToUnity(self.sumoServer)
                    
            time.sleep(Config.DELTA_TIME)


Cli.printHeading1("UTD Autonomous Vehicle Lab (AVL) v" + Config.VERSION)

print("Welcome! The application will now initialize.")

try:
    avl = AvlApplication()
    avl.main()

    Cli.printHeading2("Exit Application")
    print("Exited normally.")

except KeyboardInterrupt:
    Cli.printHeading2("Exit Application")
    print("Exited via keyboard interrupt.")

except SystemExit:
    Cli.printHeading2("Exit Application")
    print("Auto-exited due to an application error.\n")
    traceback.print_exception(*sys.exc_info())

except traci.exceptions.FatalTraCIError as traciException:
    Cli.printHeading2("Exit Application")
    print("SUMO is no longer connected. Cause: ")
    Cli.printLine(1, traciException)

except:
    Cli.printHeading2("Exit Application")
    print("Uncaught exception!\n")
    traceback.print_exception(*sys.exc_info())

try:
    SumoListener.SumoListener.StopListening()
except:
    print("Exception occured while stopping the Listener Thread.\n")
    traceback.print_exception(*sys.exc_info())