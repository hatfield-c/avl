import traci
import sumolib
import os
import sys

import SUMO_vehicle
import Cli
import TcpCommands
import Config
import TerrainManager
import VehicleManager

class SumoManager:

    Network = None

    def __init__(self):

        if 'SUMO_HOME' in os.environ:
            tools = os.path.join(os.environ['SUMO_HOME'], 'tools')
            sys.path.append(tools)
        else:
            sys.exit("please declare environment variable 'SUMO_HOME'")

        sumoCmd = [ Config.SUMO_BINARY_PATH, "-c", Config.SUMO_CFG_PATH, "--start" ]
        traci.start(sumoCmd)

        SumoManager.Network = sumolib.net.readNet(Config.SUMO_NET_PATH)

        self.vehicleManager = VehicleManager.VehicleManager()
        self.terrainManager = TerrainManager.TerrainManager()

    def stepSumo(self):

        traci.simulationStep() 
        self.vehicleManager.update()

    def initUnity(self, server):
        Cli.printLine(1, "Building terrain...")

        junctionMessage = server.CompileMessage(
            TcpCommands.DST_UNITY,
            TcpCommands.UNITY_INIT_JUNC,
            self.terrainManager.encodeJunctionData()
        )
        server.sendMessage(junctionMessage)
        
        edgeMessage = server.CompileMessage(
            TcpCommands.DST_UNITY,
            TcpCommands.UNITY_INIT_EDGE,
            self.terrainManager.encodeEdgeData()
        )
        server.sendMessage(edgeMessage)

        Cli.printLine(2, "Terrain created!")

    def sendStateToUnity(self, server):
        deleteMessage = server.CompileMessage(
            TcpCommands.DST_UNITY, 
            TcpCommands.UNITY_DELT_CAR,
            self.vehicleManager.encodeVehicleDeleteData()
        )

        initMessage = server.CompileMessage(
            TcpCommands.DST_UNITY, 
            TcpCommands.UNITY_INIT_CAR, 
            self.vehicleManager.encodeVehicleInitData()
        )

        updateMessage = server.CompileMessage(
            TcpCommands.DST_UNITY, 
            TcpCommands.UNITY_UPDT_CAR, 
            self.vehicleManager.encodeVehicleUpdateData()
        )       

        server.sendMessage(deleteMessage)
        server.sendMessage(initMessage)
        server.sendMessage(updateMessage)