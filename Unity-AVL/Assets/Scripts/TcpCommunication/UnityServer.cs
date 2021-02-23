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

    protected Queue<string> messageQueue = new Queue<string>();
    protected TcpClient socketConnection = null;
    protected NetworkStream networkStream = null;

    public static bool isReadingStream = true;

    public void ConnectToSumoListener() {
        Stopwatch connectionTimer = new Stopwatch();
        connectionTimer.Start();

        bool attemptConnection = true;
        while (attemptConnection) {

            if (connectionTimer.ElapsedMilliseconds / 1000 < this.timeoutSeconds) {
                this.socketConnection = new TcpClient();
                this.socketConnection.Connect(this.ipAddress, this.toSumoPort);
                this.networkStream = this.socketConnection.GetStream();

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
        this.SendMessage(initMessage);
    } 

    public void SendMessage(string message) {
        if (this.socketConnection == null) {
            UnityEngine.Debug.LogError("No server connection was found. SendMessage() aborted.");
        }
        
        try {
            if (this.networkStream.CanWrite) {
                byte[] encodedMessage = Encoding.ASCII.GetBytes(message);

                this.networkStream.Write(encodedMessage, 0, encodedMessage.Length);
            }
        } catch (System.Exception exception) {
            UnityEngine.Debug.Log(exception.StackTrace);
            UnityEngine.Debug.Log(exception.Message);
            UnityEngine.Debug.LogError("Exception thrown while trying to send the confirmation message. Discarding message.");
        }

    }

    public static string CompileMessage(string destination, string command, string data) {
        return  destination + TcpProtocol.MSG_DELIM + command + TcpProtocol.MSG_DELIM + data + "\n";
    }

}
