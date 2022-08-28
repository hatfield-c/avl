import traci
import sumolib
import os
import sys

import SUMO_vehicle
import Cli
import TcpProtocol
import Config
import TerrainManager
import VehicleManager

class SumoManager:

    Network = None

    def __init__(self, sumoListener):

        sys.path.append(Config.SUMO_BINARY_PATH)

        sumoCmd = [ Config.SUMO_BINARY_PATH, "-c", Config.SUMO_CFG_PATH, "--start" ]
        traci.start(sumoCmd)

        SumoManager.Network = sumolib.net.readNet(Config.SUMO_NET_PATH)

        self.vehicleManager = VehicleManager.VehicleManager()
        self.terrainManager = TerrainManager.TerrainManager()

        self.commands = {
            TcpProtocol.SUMO_INIT_LISTENER: sumoListener.ConnectionSuccess,
            TcpProtocol.SUMO_UPDT_EGO: self.vehicleManager.updateSumoVehicle,
            TcpProtocol.SUMO_INIT_EGO: self.vehicleManager.initSumoVehicle
        }

    def stepSumo(self):
        traci.simulationStep() 
        self.vehicleManager.update()

    def initUnity(self, server):
        Cli.printLine(1, "Building terrain...")

        junctionMessage = server.CompileMessage(
            TcpProtocol.DST_UNITY,
            TcpProtocol.UNITY_INIT_JUNC,
            self.terrainManager.encodeJunctionData()
        )
        server.sendMessage(junctionMessage)
        
        edgeMessage = server.CompileMessage(
            TcpProtocol.DST_UNITY,
            TcpProtocol.UNITY_INIT_EDGE,
            self.terrainManager.encodeEdgeData()
        )
        server.sendMessage(edgeMessage)

        Cli.printLine(2, "Terrain created!")

    def sendStateToUnity(self, server):

        deleteMessage = server.CompileMessage(
            TcpProtocol.DST_UNITY, 
            TcpProtocol.UNITY_DELT_CAR,
            self.vehicleManager.encodeVehicleDeleteData()
        )

        initMessage = server.CompileMessage(
            TcpProtocol.DST_UNITY, 
            TcpProtocol.UNITY_INIT_CAR, 
            self.vehicleManager.encodeVehicleInitData()
        )

        updateMessage = server.CompileMessage(
            TcpProtocol.DST_UNITY, 
            TcpProtocol.UNITY_UPDT_CAR, 
            self.vehicleManager.encodeVehicleUpdateData()
        )       

        server.sendMessage(deleteMessage)
        server.sendMessage(initMessage)
        server.sendMessage(updateMessage)

    def processUnityMessages(self, listener):

        while listener.messageQueue.qsize() > 0:
            message = listener.messageQueue.get(False)
            
            self.processMessage(message)

    def processMessage(self, message):
        messageParts = self.decodeMessage(message)

        if messageParts["destination"] != TcpProtocol.DST_SUMO:
            print("\n[ERROR]: A message was delivered to SUMO that was not addressed to SUMO, and will now be discarded. Message contents:")
            print(message)
            return

        command = messageParts["command"]

        if command not in self.commands:
            print("\n[ERROR]: A message was delivered to SUMO that had an unfamiliar command, and will now be discarded. Message contents:")
            print(message)
            return

        action = self.commands[command]
        rawData = messageParts["data"]
        
        action(rawData)

    def decodeMessage(self, rawString):
        messageParts = {}

        parts = rawString.split(TcpProtocol.MSG_DELIM)
        
        messageParts["destination"] = parts[0]
        messageParts["command"] = parts[1]
        messageParts["data"] = parts[2]

        return messageParts
        