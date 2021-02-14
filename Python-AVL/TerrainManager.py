import traci

import TcpCommands
import SumoJunction

class TerrainManager:
    def __init__(self):
        self.junctions = self.compileJunctions()

    def compileJunctions(self):
        junctions = {}

        junctionIds = traci.junction.getIDList()
        for jId in junctionIds:
            junctions[jId] = SumoJunction.SumoJunction(
                jId,
                position = traci.junction.getPosition(jId),
                shape = traci.junction.getShape(jId)
            )

        return junctions

    def encodeJunctionData(self):
        if len(self.junctions) < 1:
            return None

        junctionData = ""

        for jId in self.junctions:
            junctionData += self.junctions[jId].jsonInitData() + TcpCommands.DATA_DELIM
            
        junctionData = junctionData[ : -len(TcpCommands.DATA_DELIM)]

        return junctionData     