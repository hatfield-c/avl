import json

class SumoEdge:
    def __init__(self, ID, toJunction, fromJunction):
        self.ID = ID
        self.toJunction = toJunction
        self.fromJunction = fromJunction

    def jsonInitData(self):
        data = {}

        data["junctionId"] = self.ID
        data["toJunction"] = self.toJunction
        data["fromJunction"] = self.fromJunction

        return json.dumps(data)
