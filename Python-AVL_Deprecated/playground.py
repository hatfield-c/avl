import sumolib

networkPath = "C:/Users/Cody/Documents/Academic/Projects/AV/avl/SUMO_Networks/Rectangle/Network_01.net.xml"

network = sumolib.net.readNet(networkPath)

print(network.getEdges())

for e in network.getEdges():
    print(e.getID())