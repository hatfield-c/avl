using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[System.Serializable]
public class UnityServer
{
    [SerializeField] protected string ipAddress = "localhost";
    [SerializeField] protected int toSumoPort = 4043;
    [SerializeField] protected int timeoutSeconds = 5;
    protected static string IP_ADDRESS = "localhost";
    protected static int TO_SUMO_PORT = 4043;
    protected static int TIMEOUT = 5;

    protected static Queue<string> QUEUE = new Queue<string>();
    protected static TcpClient CONNECTION = null;
    protected static NetworkStream STREAM = null;

    public static bool isReadingStream = true;

    public UnityServer() {
        UnityServer.IP_ADDRESS = this.ipAddress;
        UnityServer.TO_SUMO_PORT = this.toSumoPort;
        UnityServer.TIMEOUT = this.timeoutSeconds;
    }

    public static void ConnectToSumoListener() {
        Stopwatch connectionTimer = new Stopwatch();
        connectionTimer.Start();

        bool attemptConnection = true;
        while (attemptConnection) {

            if (connectionTimer.ElapsedMilliseconds / 1000 < UnityServer.TIMEOUT) {
                UnityServer.CONNECTION = new TcpClient();
                UnityServer.CONNECTION.Connect(
                    UnityServer.IP_ADDRESS, 
                    UnityServer.TO_SUMO_PORT
                );
                UnityServer.STREAM = UnityServer.CONNECTION.GetStream();

                attemptConnection = false;
            } else {
                connectionTimer.Stop();
                UnityEngine.Debug.LogError("Connection timed out before an attempted connection was made by the server.");

                return;
            }
        }

        connectionTimer.Stop();
        
        string initMessage = UnityServer.CompileMessage(
            TcpProtocol.TO_SUMO, 
            TcpProtocol.SUMO_INIT_LISTENER, 
            "{\"message\": \"CONNECTION SUCCESS!\"}"
        );
        UnityServer.SendMessage(initMessage);
    }

    public static void SendMessage(string message) {
        if (UnityServer.CONNECTION == null) {
            UnityEngine.Debug.LogError("No server connection was found. SendMessage() aborted.");
        }
        
        try {
            if (UnityServer.STREAM.CanWrite) {
                byte[] encodedMessage = Encoding.ASCII.GetBytes(message);

                UnityServer.STREAM.Write(encodedMessage, 0, encodedMessage.Length);
            }
        } catch (System.Exception exception) {
            UnityEngine.Debug.Log(exception.StackTrace);
            UnityEngine.Debug.Log(exception.Message);
            UnityEngine.Debug.LogError("Exception thrown while trying to send message to SUMO. Discarding message.");
        }

    }

    public static string CompileMessage(string destination, string command, string data) {
        return  destination + TcpProtocol.MSG_DELIM + command + TcpProtocol.MSG_DELIM + data + "\n";
    }

}
