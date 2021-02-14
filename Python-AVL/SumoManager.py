import traci
import os
import sys
import SUMO_vehicle
import Cli
import TcpCommands
import TerrainManager
import VehicleManager

class SumoManager:
    def __init__(self, sumoBinaryPath, networkPath):

        self.sumoBinaryPath = sumoBinaryPath
        self.networkPath = networkPath

        if 'SUMO_HOME' in os.environ:
            tools = os.path.join(os.environ['SUMO_HOME'], 'tools')
            sys.path.append(tools)
        else:
            sys.exit("please declare environment variable 'SUMO_HOME'")

        sumoCmd = [ self.sumoBinaryPath, "-c", self.networkPath, "--start" ]
        traci.start(sumoCmd)

        self.vehicleManager = VehicleManager.VehicleManager()
        self.terrainManager = TerrainManager.TerrainManager()

    def stepSumo(self):

        traci.simulationStep() 
        self.vehicleManager.update()

    def initUnity(self, server):
        Cli.printLine(1, "Building terrain...")

        terrainMessage = server.CompileMessage(
            TcpCommands.DST_UNITY,
            TcpCommands.UNITY_INIT_JUNC,
            self.terrainManager.encodeJunctionData()
        )
        server.sendMessage(terrainMessage)

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