import traci

import TcpProtocol
import SumoManager
import SumoJunction
import SumoEdge

class TerrainManager:
    def __init__(self):
        self.junctions = self.compileJunctions()
        self.edges = self.compileEdges()

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

    def compileEdges(self):
        edges = {}
        
        edgeObjects = SumoManager.SumoManager.Network.getEdges()

        for eObject in edgeObjects:
            eId = eObject.getID()

            edges[eId] = SumoEdge.SumoEdge(
                ID = eId,
                length = eObject.getLength(),
                numLanes = len(eObject.getLanes()),
                laneWidth = eObject.getLanes()[0].getWidth(),
                toJunction = eObject.getToNode().getID(),
                fromJunction = eObject.getFromNode().getID()
            )
        
        return edges

    def encodeJunctionData(self):
        if len(self.junctions) < 1:
            return None

        junctionData = ""

        for jId in self.junctions:
            junctionData += self.junctions[jId].jsonInitData() + TcpProtocol.DATA_DELIM
            
        junctionData = junctionData[ : -len(TcpProtocol.DATA_DELIM)]

        return junctionData     

    def encodeEdgeData(self):

        if len(self.edges) < 1:
            return None

        edgeData = ""

        for eId in self.edges:
            edgeData += self.edges[eId].jsonInitData() + TcpProtocol.DATA_DELIM
            
        edgeData = edgeData[ : -len(TcpProtocol.DATA_DELIM)]
        
        return edgeData     