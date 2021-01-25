import traci
import os
import sys
import SUMO_vehicle
import Cli
import TcpCommands

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

        self.edges = self.buildRoadNetwork()
        self.vehicles = {}
        
    def buildRoadNetwork(self):
        edgeIds = traci.lane.getIDList()

        edges = {}
        for id in edgeIds:
            edges[id] = traci.lane.getShape(id)

    def stepSumo(self):

        traci.simulationStep() 

        vehicleIds = traci.vehicle.getIDList()

        for vehicleKey in self.vehicles:
            if(vehicleKey not in vehicleIds):
                self.vehicles.pop(vehicleKey)

        for vehicleId in vehicleIds:
            if(vehicleId not in list(self.vehicles.keys())):
                newVehicle = SUMO_vehicle.SumoObject(vehicleId)
                self.vehicles[vehicleId] = newVehicle

        for vehicleKey in self.vehicles:
            self.vehicles[vehicleKey].UpdateVehicle()

    def encodeVehicleUpdateData(self):
        vehicleData = ""

        for vehicleId in self.vehicles:
            vehicleData += self.vehicles[vehicleId].jsonUpdateData() + TcpCommands.DATA_DELIM
            
        vehicleData = vehicleData[ : -len(TcpCommands.DATA_DELIM)]

        return vehicleData        