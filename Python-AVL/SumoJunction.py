import json

class SumoJunction:
    def __init__(self, ID, position, shape):
        self.ID = ID
        self.position = position
        self.shape = shape

    def jsonInitData(self):
        data = {}

        data["junctionId"] = self.ID
        data["position"] = self.position
        data["shape"] = self.shape

        return json.dumps(data)