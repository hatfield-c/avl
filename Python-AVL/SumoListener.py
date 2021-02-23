import time
import traceback
import socket
import queue
import threading
import json

import Cli
import TcpProtocol
import Config

class SumoListener:

    LISTENING = False

    def __init__(self, timeout, dummy = False):
        if dummy:
            return

        self.timeout = timeout

        self.toSumoSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        self.toSumoSocket.setblocking(False)

        toSumoAddress = (
            Config.TCP_IP_ADDR, 
            int(Config.TCP_PORT_TO_SUMO)
        )

        self.toSumoSocket.bind(toSumoAddress)

        self.messageQueue = queue.SimpleQueue()

        self.readStreamThread = None
        self.listenSocket = None
        self.listenAddress = None

    def startListening(self, sumoManager):
        self.connectToUnityServer()

        self.readStreamThread = threading.Thread(
            target = self.listen
        )
        self.readStreamThread.start()

        initMessage = self.messageQueue.get(True)
        sumoManager.processMessage(initMessage)

    def connectToUnityServer(self):
        Cli.printLine(2, "Connecting to Unity...")

        self.toSumoSocket.listen(1)

        stopWatch = time.time()
        trying = True

        while trying:
            if time.time() - stopWatch > self.timeout:
                Cli.printLine(3, "[ERROR] Connection timed out!")
                exit()
            
            try:
                tmpSocket, tmpAddress = self.toSumoSocket.accept()
                tmpSocket.setblocking(False)
                trying = False

                Cli.printLine(3, "Connection accepted!")

            except BlockingIOError:
                pass

            except Exception as exception:
                traceback.print_exc()
                print(exception)
                Cli.printLine(0, "[ERROR] Error occured while trying to make connection to Unity Listener.")
                exit()
                
        self.listenSocket = tmpSocket
        self.listenAddress = tmpAddress

    def listen(self):
        SumoListener.LISTENING = True

        buffer = ""
        while SumoListener.LISTENING:
            try:
                buffer += self.listenSocket.recv(1024).decode("utf-8")
                
                delimIndex = buffer.find(TcpProtocol.END_OF_MSG)

                while delimIndex > 0:
                    message = buffer[0 : delimIndex]

                    if delimIndex != len(buffer) - 1:
                        buffer = buffer[delimIndex + 1 : ]
                    else:
                        buffer = ""

                    self.messageQueue.put(message)

                    delimIndex = buffer.find(TcpProtocol.END_OF_MSG)

            except BlockingIOError:
                pass

            except Exception as exception:
                self.toSumoSocket.close()

                traceback.print_exc()
                print(exception)
                Cli.printLine(3, "[ERROR] Error occured while trying to receive client name.")
                exit()

    @staticmethod
    def ConnectionSuccess(message):
        data = json.loads(message)
        
        Cli.printLine(3, "Response from the Unity Server: " + data["message"])

    @staticmethod
    def StopListening():
        SumoListener.LISTENING = False