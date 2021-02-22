import traci
import SUMO_vehicle
import TcpProtocol

class VehicleManager:
    def __init__(self):
        self.deleteVehicles = {}
        self.initVehicles = {}
        self.activeVehicles = {}

    def update(self):
        activeVehicles = traci.vehicle.getIDList()

        self.buildDeleteList(activeVehicles)
        self.buildInitList(activeVehicles)

        for vehicleKey in self.activeVehicles:
            self.activeVehicles[vehicleKey].UpdateVehicle()
    
    def buildDeleteList(self, activeIds):
        for vehicleKey in self.activeVehicles:
            if(vehicleKey not in activeIds):
                self.deleteVehicles[vehicleKey] = self.activeVehicles.pop(vehicleKey)

    def buildInitList(self, activeIds):
        for vehicleId in activeIds:
            if(vehicleId not in list(self.activeVehicles.keys())):
                newVehicle = SUMO_vehicle.SumoObject(vehicleId)
                self.initVehicles[vehicleId] = newVehicle
                self.activeVehicles[vehicleId] = newVehicle

    def updateSumoVehicle(self, rawData):
        print(rawData)

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