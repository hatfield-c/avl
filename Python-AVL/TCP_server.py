import time
import socket

import Cli

class TCP_Server(object):
    def __init__(self, IP, port):
        self.IP = IP
        self.port = port
        self.Num_Listener = 1

        self.ServerSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.ServerSocket.setblocking(False)
        s_adr = (self.IP, self.port)
        self.ServerSocket.bind(s_adr)

        self.UnityClient = []
        self.UnityAddress = []
        self.UnityRunning = False
        self.UnityThread = []

    def StartServer(self, UnityQueue):

        self.ServerSocket.listen(self.Num_Listener)
        
        i = 0
        while i < self.Num_Listener:
            trying = True
            while trying:
                try:
                    clientSocket, tmpAddress = self.ServerSocket.accept()
                    clientSocket.setblocking(False)
                    trying = False
                except BlockingIOError:
                    time.sleep(0.01)
            
            ClientName = '00000'

            while ClientName is '00000':
                ClientName = clientSocket.recv(5)

                Cli.printLine(2, "Client " + str(ClientName) + " connected!")

            if ClientName == 'U3D00'.encode('utf8'):
                Cli.printLine(2, "Unity 3D connected!")
                
                self.UnityClient = clientSocket
                self.UnityAddress = tmpAddress
                self.UnityRunning = True
                i = i + 1
                time.sleep(1)

            else:
                clientSocket.close()
                print("ERROR! Check the clients and retry!")

    def CloseSocket(self):
        self.ServerSocket.close()