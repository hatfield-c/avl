import math
import traci
import json

class SumoObject(object):

    def __init__(self, SumoID):
        self.ID = str(SumoID)  # VehicleID
        try:

            self.height = traci.vehicle.getHeight(self.ID)

            self.ObjType = traci.vehicle.getTypeID(self.ID)
            self.Route = traci.vehicle.getRouteID(self.ID)
            self.Edge = traci.vehicle.getRoadID(self.ID)
            self.Length = traci.vehicle.getLength(self.ID)
            self.Width = traci.vehicle.getWidth(self.ID)
            
            self.__CalculateSizeClass() #Vehicle size category
            if traci.vehicle.getSignals(self.ID) & 8 == 8: #Bitmask - 8 for brake light
                self.StBrakePedal = True
            else:
                self.StBrakePedal = False

            tmp_pos = traci.vehicle.getPosition(self.ID)  # position: x,y
            self.PosX_FrontBumper = tmp_pos[0] # X position (front bumper, meters)
            self.PosY_FrontBumper = tmp_pos[1] # Y position (front bumper, meters)
            self.Velocity = traci.vehicle.getSpeed(self.ID)
            self.Heading = traci.vehicle.getAngle(self.ID)
            self.Color = traci.vehicle.getColor(self.ID)

            self.__CalculateCenter() #self.PosX_Center, self.PosY_Center (center, meters)

            self.vehicleClass = traci.vehicle.getVehicleClass(self.ID)

        except:
            print("Error creating container for SUMO vehicle: ", self.ID)

    def UpdateVehicle(self):

        if traci.vehicle.getSignals(self.ID) & 8 == 8: #Bitmask - 8 for brake light
            self.StBrakePedal = True
        else:
            self.StBrakePedal = False

        tmp_pos = traci.vehicle.getPosition(self.ID)  # position: x,y
        self.PosX_FrontBumper = tmp_pos[0]  # X position (front bumper, meters)
        self.PosY_FrontBumper = tmp_pos[1]  # Y position (front bumper, meters)
        self.Velocity = traci.vehicle.getSpeed(self.ID)
        self.Heading = traci.vehicle.getAngle(self.ID)
        self.Edge = traci.vehicle.getRoadID(self.ID)

        self.__CalculateCenter()  # self.PosX_Center, self.PosY_Center (center, meters)

    def ReinsertVehicle(self):

        LaneIndex = 1  # dummy
        KeepRouteMode = 1  # KeepRoute: 2 = Free move.

        try:
            traci.vehicle.add(self.ID, self.Route)  # Try to put it back
        except:
            pass #If already there, do nothing
        try:
            traci.vehicle.moveToXY(self.ID, self.Edge, LaneIndex, self.PosX_FrontBumper, self.PosY_FrontBumper, self.Heading, KeepRouteMode)
            traci.vehicle.setSpeed(self.ID, self.Velocity)
        except:
            print("Error reinserting SUMO vehicle: ", self.ID)

    #Transforms SUMO X-Y coordinate system to GPS
    def __TransformGPS(self, PosX, PosY):
        Lon, Lat = traci.simulation.convertGeo(PosX, PosY)
        return Lon, Lat

    # Transforms front bumper position to center
    def __CalculateCenter(self):
        self.PosX_Center = self.PosX_FrontBumper - (math.sin(math.radians(self.Heading)) * (self.Length / 2))
        self.PosY_Center = self.PosY_FrontBumper - (math.cos(math.radians(self.Heading)) * (self.Length / 2))

    # Defines vehicle category based on its size
    def __CalculateSizeClass(self):

        if self.Length < 1:
            self.SizeClass = 1 #pedestrian (workaround)
        elif  self.Length < 4:
            self.SizeClass = 11 #Small car
        elif self.Length < 5:
            self.SizeClass = 12 #Medium car
        else:
            self.SizeClass = 13 #Large car

    def updateFromUnity(self, data):
        self.PosX_Center = data["position"][0]
        self.PosY_Center = data["position"][1]
        self.Heading = data["heading"]

        self.calculateBumper()

    def calculateBumper(self):
        self.PosX_FrontBumper = self.PosX_Center + (math.sin(math.radians(self.Heading)) * (self.Length / 2))
        self.PosY_FrontBumper = self.PosY_Center + (math.cos(math.radians(self.Heading)) * (self.Length / 2))

    def jsonDeleteData(self):
        data = {}

        data["vehicleId"] = self.ID

        return json.dumps(data)

    def jsonInitData(self):
        data = {}

        data["vehicleId"] = self.ID
        data["sizeClass"] = self.SizeClass
        data["vehicleClass"] = self.vehicleClass
        data["colorHex"] = '#%02x%02x%02x' % self.Color[0:3]
        data["length"] = self.Length
        data["width"] = self.Width
        data["height"] = self.height

        return json.dumps(data)

    def jsonUpdateData(self):
        data = {}

        data["vehicleId"] = self.ID
        data["speed"] = self.Velocity
        data["heading"] = self.Heading
        data["position"] = [self.PosX_Center, self.PosY_Center]
        data["brake"] = self.StBrakePedal
        data["currentEdge"] = self.Edge

        return json.dumps(data)
