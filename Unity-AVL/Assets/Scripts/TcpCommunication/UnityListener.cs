using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Threading;
using System.IO;

[System.Serializable]
public class UnityListener
{
    [SerializeField] protected string ipAddress = "localhost";
    [SerializeField] protected int toUnityPort = 4042;
    [SerializeField] protected int timeoutSeconds = 5;

    protected Queue<string> messageQueue = new Queue<string>();
    protected TcpClient socketConnection = null;
    protected NetworkStream networkStream = null;
    protected StreamReader streamReader = null;
    
    public static Thread readStreamThread = null;
    public static bool isListening = true;

    public void StartListening() {
        UnityListener.readStreamThread = new Thread(
            () => {
                this.AcceptConnection();

                this.ReadSumoServer();
            }
        );
        UnityListener.readStreamThread.IsBackground = true;
        UnityListener.readStreamThread.Start();
    }

    public void ReadSumoServer() {

        UnityEngine.Debug.Log("Reading Sumo Server stream.");

        while (UnityListener.isListening) {
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
                UnityEngine.Debug.LogError("Exception occured while trying to read from the Sumo Server.");

                UnityListener.StopListening();
            }
        }
    }

    protected void AcceptConnection() {
        Stopwatch connectionTimer = new Stopwatch();
        connectionTimer.Start();

        bool attemptConnection = true;
        while (attemptConnection) {

            if (connectionTimer.ElapsedMilliseconds / 1000 < this.timeoutSeconds) {
                this.socketConnection = new TcpClient();
                this.socketConnection.Connect(this.ipAddress, this.toUnityPort);
                this.networkStream = this.socketConnection.GetStream();
                this.streamReader = new StreamReader(this.networkStream);

                attemptConnection = false;
            } else {
                connectionTimer.Stop();
                UnityEngine.Debug.LogError("Connection timed out before an attempted connection was made by the server.");

                return;
            }
        }

        connectionTimer.Stop();
    }

    public string GetMessage() {
        if (this.messageQueue.Count < 1) {
            return null;
        }

        return this.messageQueue.Dequeue();
    }

    public bool HasMessages() {
        return this.messageQueue.Count > 0;
    }

    public static void StopListening() {
        UnityListener.isListening = false;
    }

}
