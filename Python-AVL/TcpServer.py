import time
import traceback
import socket

import Cli
import TcpCommands

class TcpServer:
    def __init__(self, IP, port, timeout):
        self.IP = IP
        self.port = port
        self.timeout = timeout
        self.connectionCount = 1

        self.serverSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.serverSocket.setblocking(False)

        socketAddress = (self.IP, self.port)
        self.serverSocket.bind(socketAddress)

        self.unityClient = None
        self.unityAddress = None
        self.unityRunning = False
        self.clientName = None

    def startServer(self):
        Cli.printLine(2, "Connecting to Unity...")

        self.serverSocket.listen(self.connectionCount)
        
        i = 0
        while i < self.connectionCount:
            stopWatch = time.time()
            trying = True
            while trying:
                if time.time() - stopWatch > self.timeout:
                    Cli.printLine(3, "[ERROR] Connection timed out!")
                    exit()

                try:
                    clientSocket, tmpAddress = self.serverSocket.accept()
                    clientSocket.setblocking(False)
                    trying = False

                    Cli.printLine(3, "Connection accepted!")
                except BlockingIOError:
                    time.sleep(0.01)
                except Exception as exception:

                    traceback.print_exc()
                    print(exception)
                    Cli.printLine(0, "[ERROR] Error occured while trying to accept connection from Unity.")
                    exit()
            
            stopWatch = time.time()
            trying = True
            while trying:
                if time.time() - stopWatch > self.timeout:
                    Cli.printLine(3, "[ERROR] Connection timed out!")
                    exit()

                try:
                    message = clientSocket.recv(255).decode("utf-8")
                    clientName = self.decodeClientName(message)

                    if clientName is None:
                        Cli.printLine(3, "[ERROR] Received invalid message while listening for client name! Discarding message and trying again.")
                        Cli.printLine(4, "Message received: " + message)
                        continue

                    self.clientName = clientName
                    trying = False

                    Cli.printLine(3, "Client " + str(self.clientName) + " connected!")
                except BlockingIOError:
                    time.sleep(0.01)
                except Exception as exception:
                    clientSocket.close()

                    traceback.print_exc()
                    print(exception)
                    Cli.printLine(3, "[ERROR] Error occured while trying to receive client name.")
                    exit()

            Cli.printLine(3, "Unity 3D connected!")
                
            self.UnityClient = clientSocket
            self.UnityAddress = tmpAddress
            self.UnityRunning = True
            i = i + 1
            time.sleep(0.01)

    def decodeClientName(self, message):
        messagePieces = message.split(TcpCommands.MSG_DELIM)

        if len(messagePieces) != 2:
            return None

        if messagePieces[0] != TcpCommands.CONNECTION_INIT_UNITY:
            return None

        return messagePieces[1]

    def sendMessage(self, message):
        if message is None:
            return

        self.UnityClient.send(message.encode())

    def closeSocket(self):
        self.serverSocket.close()