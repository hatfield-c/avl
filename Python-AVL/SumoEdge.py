import json

class SumoEdge:
    def __init__(self, ID, length, numLanes, laneWidth, toJunction, fromJunction):
        self.ID = ID
        self.length = length
        self.numLanes = numLanes
        self.laneWidth = laneWidth
        self.toJunction = toJunction
        self.fromJunction = fromJunction

        self.width = self.numLanes * self.laneWidth

    def jsonInitData(self):
        data = {}

        data["edgeId"] = self.ID
        data["length"] = self.length
        data["numLanes"] = self.numLanes
        data["laneWidth"] = self.laneWidth
        data["width"] = self.width
        data["toJunction"] = self.toJunction
        data["fromJunction"] = self.fromJunction

        return json.dumps(data)
