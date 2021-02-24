import traci
import json
import SUMO_vehicle
import TcpProtocol

class VehicleManager:
    def __init__(self):
        self.deleteVehicles = {}
        self.initVehicles = {}
        self.activeVehicles = {}
        self.egoVehicles = {}

    def update(self):
        activeIds = traci.vehicle.getIDList()

        self.buildDeleteList(activeIds)
        self.buildInitList(activeIds)

        for vehicleKey in self.activeVehicles:
            self.activeVehicles[vehicleKey].UpdateVehicle()
    
    def buildDeleteList(self, activeIds):
        for vehicleKey in self.activeVehicles:
            if vehicleKey not in activeIds and vehicleKey not in self.egoVehicles:
                self.deleteVehicles[vehicleKey] = self.activeVehicles.pop(vehicleKey)

    def buildInitList(self, activeIds):
        for vehicleId in activeIds:
            if vehicleId not in self.activeVehicles and vehicleId not in self.egoVehicles:
                newVehicle = SUMO_vehicle.SumoObject(vehicleId)
                self.initVehicles[vehicleId] = newVehicle
                self.activeVehicles[vehicleId] = newVehicle

    def initSumoVehicle(self, rawData):
        rawInitDatum = rawData.split(TcpProtocol.DATA_DELIM)

        for rawInitData in rawInitDatum:
            initData = json.loads(rawInitData)

            vehicleId = initData["vehicleId"]
            vehicleType = initData["vehicleType"]

            traci.vehicle.add(
                vehID = vehicleId,
                routeID = ""
            )
            traci.vehicle.setSpeedMode(vehicleId, 0)
            traci.vehicle.setSpeed(vehID = vehicleId, speed = 0)

            self.egoVehicles[vehicleId] = SUMO_vehicle.SumoObject(vehicleId)

            if vehicleId in self.activeVehicles:
                self.activeVehicles.pop(vehicleId)


    def updateSumoVehicle(self, rawData):
        rawVehicleDatum = rawData.split(TcpProtocol.DATA_DELIM)

        for rawVehicleData in rawVehicleDatum:
            vehicleData = json.loads(rawVehicleData)

            vehicleId = vehicleData["vehicleId"]
            x = vehicleData["position"][0]
            y = vehicleData["position"][1]
            heading = vehicleData["heading"]

            traci.vehicle.setSpeed(vehID = vehicleId, speed = 0)
            traci.vehicle.moveToXY(
                vehID = vehicleId,
                edgeID = "",
                lane = -1,
                x = x,
                y = y,
                angle = heading,
                keepRoute = 2
            )

    def popDeleteList(self):
        deleteList = self.deleteVehicles
        self.deleteVehicles = {}

        return deleteList

    def popInitList(self):
        initList = self.initVehicles
        self.initVehicles = {}

        return initList

    def encodeVehicleDeleteData(self):
        if len(self.deleteVehicles) < 1:
            return None

        deleteVehicles = self.popDeleteList()

        vehicleData = ""

        for vehicleId in deleteVehicles:
            vehicleData += deleteVehicles[vehicleId].jsonDeleteData() + TcpProtocol.DATA_DELIM
            
        vehicleData = vehicleData[ : -len(TcpProtocol.DATA_DELIM)]

        return vehicleData     

    def encodeVehicleInitData(self):
        if len(self.initVehicles) < 1:
            return None

        initVehicles = self.popInitList()

        vehicleData = ""

        for vehicleId in initVehicles:
            vehicleData += initVehicles[vehicleId].jsonInitData() + TcpProtocol.DATA_DELIM
            
        vehicleData = vehicleData[ : -len(TcpProtocol.DATA_DELIM)]

        return vehicleData     

    def encodeVehicleUpdateData(self):
        if len(self.activeVehicles) < 1:
            return None

        vehicleData = ""

        for vehicleId in self.activeVehicles:
            vehicleData += self.activeVehicles[vehicleId].jsonUpdateData() + TcpProtocol.DATA_DELIM
            
        vehicleData = vehicleData[ : -len(TcpProtocol.DATA_DELIM)]

        return vehicleData      