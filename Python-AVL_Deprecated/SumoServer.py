import time
import traceback
import socket

import Cli
import TcpProtocol
import Config

class SumoServer:
    def __init__(self, timeout):

        self.timeout = timeout

        self.toUnitySocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        self.toUnitySocket.setblocking(False)

        toUnityAddress = (
            Config.TCP_IP_ADDR, 
            int(Config.TCP_PORT_TO_UNITY)
        )

        self.toUnitySocket.bind(toUnityAddress)

        self.socketStream = None
        self.socketAddress = None

    def startServing(self):
        Cli.printLine(2, "Connecting Sumo Server to Unity Listener...")

        self.toUnitySocket.listen(1)
        
        stopWatch = time.time()
        trying = True

        while trying:
            if time.time() - stopWatch > self.timeout:
                Cli.printLine(3, "[ERROR] Connection timed out!")
                exit()

            try:
                tmpSocket, tmpAddress = self.toUnitySocket.accept()
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
                
        self.socketStream = tmpSocket
        self.socketAddress = tmpAddress

    def sendMessage(self, message):
        if message is None:
            return

        self.socketStream.send(message.encode())

    def closeSocket(self):
        self.toUnitySocket.close()

    @staticmethod
    def CompileMessage(
        destinationCode,
        commandCode,
        data
    ):
        if data is None:
            return None

        return destinationCode + TcpProtocol.MSG_DELIM + commandCode + TcpProtocol.MSG_DELIM + data + TcpProtocol.END_OF_MSG