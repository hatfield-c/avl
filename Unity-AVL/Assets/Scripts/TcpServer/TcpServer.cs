using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Threading;
using System.IO;

[System.Serializable]
public class TcpServer
{
    public const string CONNECTION_INIT_UNITY = "CONNECTION_INIT_UNITY";

    public const string UNITY_DELT_CAR = "UNITY_DELT_CAR";
    public const string UNITY_INIT_CAR = "UNITY_INIT_CAR";
    public const string UNITY_UPDT_CAR = "UNITY_UPDT_CAR";

    public const string TO_UNITY = "TO_UNITY";
    public const string TO_SUMO = "TO_SUMO";

    public const char MSG_DELIM = '$';
    public const char DATA_DELIM = '|';

    [SerializeField] protected string clientName = "UNITY_3D_0";
    [SerializeField] protected int serverPort = 4042;
    [SerializeField] protected string ipAddress = "localhost";
    [SerializeField] protected int streamBufferSize = 8192;
    [SerializeField] protected float readIntervalSeconds = 0.02f;
    [SerializeField] protected int timeoutSeconds = 5;

    protected Queue<string> messageQueue = new Queue<string>();
    protected TcpClient socketConnection = null;
    protected NetworkStream networkStream = null;
    protected StreamReader streamReader = null;
    
    public static Thread readStreamThread = null;
    public static bool isReadingStream = true;

    public string GetMessage() {
        if (this.messageQueue.Count < 1) {
            return null;
        }

        return this.messageQueue.Dequeue();
    }

    public void StartClient() {
        TcpServer.readStreamThread = new Thread(
            () => {
                this.Connect();

                string initMessage = TcpServer.CONNECTION_INIT_UNITY + TcpServer.MSG_DELIM + this.clientName;
                this.SendMessage(initMessage);

                this.ReadServerStream();
            }
        );
        TcpServer.readStreamThread.IsBackground = true;
        TcpServer.readStreamThread.Start();
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
            UnityEngine.Debug.LogError("Exception thrown while trying to send a message. Discarding message.");
        }
    }

    public bool HasMessages() {
        return this.messageQueue.Count > 0;
    }

    public static void KillClient() {
        TcpServer.isReadingStream = false;
    }

    protected void Connect() {
        Stopwatch connectionTimer = new Stopwatch();
        connectionTimer.Start();

        bool attemptConnection = true;
        while (attemptConnection) {

            if (connectionTimer.ElapsedMilliseconds / 1000 < this.timeoutSeconds) {
                this.socketConnection = new TcpClient();
                this.socketConnection.Connect(this.ipAddress, this.serverPort);
                this.networkStream = this.socketConnection.GetStream();
                this.streamReader = new StreamReader(this.networkStream);

                attemptConnection = false;
            } else {
                connectionTimer.Stop();
                UnityEngine.Debug.LogError("Connection timed out before the server responded.");

                return;
            }
        }

        connectionTimer.Stop();
    }

    public void ReadServerStream() {

        UnityEngine.Debug.Log("Reading server stream.");
        while (TcpServer.isReadingStream) {
            try {
                while (
                    !this.streamReader.EndOfStream
                ) {
                    string message = this.streamReader.ReadLine();
                    this.messageQueue.Enqueue(message);
                }
            } catch (System.Exception exception) {
                this.streamReader.Close();
                this.networkStream.Close();
                this.socketConnection.Close();

                UnityEngine.Debug.Log(exception);
                UnityEngine.Debug.LogError("Exception occured while trying to read from the server stream.");

                TcpServer.KillClient();
            }
        }
    }
    
}
