// ======================================================
// Copyright (C) 2019 BME Automated Drive Lab
// This program and the accompanying materials
// are made available under the terms of the MIT license.
// ======================================================
// Author: Matyas Szalai 
// Date: 2019. 11. 10.
// ======================================================

using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.ThirdPerson;
//using UnityStandardAssets.Vehicles.Car;
using System.IO;
using static System.Console;

public class scr_TCP : MonoBehaviour
{
    public const string UNITY_UPDT_CAR = "UNITY_UPDT_CAR";
    public const string UNITY_DELT_CAR = "UNITY_DELT_CAR";

    public const string TO_UNITY = "TO_UNITY";
    public const string TO_SUMO = "TO_SUMO";

    public const char MSG_DELIM = '$';
    public const char DATA_DELIM = '|';

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private int ipPort = 4042;                      //TCP/IP PORT
    private string IpAd_loc = "localhost";          //TCP/IP ADRESS
    public int NumOfConnection = 1;
    private int TCPtimeout = 3;                     //timeout to stop reconnecting, while trying to reconnect, the editor is running after a stopped simulation. It needed to stop the server first
    private string message2send;
    private string serverMessage;
    public Queue<string> TCP_recv_queue = new Queue<string>(); //queue to store received messages
    private string cleardincoming;
    private string cleardincoming2;
    private string Rx;


    // Start is called before the first frame update
    void Start()
    {
        ConnectToTcpServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (TCP_recv_queue.Count > 0)
        {
            string msg = TCP_recv_queue.Dequeue();
            if (msg.Contains(scr_TCP.UNITY_UPDT_CAR))
            {
                Rx = msg;
            }
        }
        else
        {
            //Debug.Log("Que is empty");
        }
    }

    public string RxMsg() //public message, get from Main script
    {
        return Rx;
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(Con));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    private void Con()
    {
        bool connect = true;
        while (connect)
        {
            NumOfConnection += 1;
            if (NumOfConnection < TCPtimeout)
            {
                int i = 1;
                try
                {
                    socketConnection = new TcpClient(IpAd_loc, ipPort);
                    message2send = "U3D00";
                    SendMessage();
                    ListenForData();
                    connect = false;
                    i++;
                }
                catch
                {

                }
            }
            else
            {
                clientReceiveThread.Abort();
                Debug.Log("Exit the game");
                Application.Quit();
            }
        }
    }

    private void ReCon()
    {
        NumOfConnection += 1;
        if (NumOfConnection < TCPtimeout)
        {
            try
            {
                socketConnection = new TcpClient(IpAd_loc, ipPort);
                message2send = "U3D00";
                SendMessage();
                ListenForData();
            }
            catch
            {

            }
        }
        else
        {
            clientReceiveThread.Abort();
            Debug.Log("Exit the game");
            Application.Quit();
        }

    }

    private void ListenForData()
    {
        try
        {
            NumOfConnection = 1;
            Byte[] bytes = new Byte[8192];
            while (true)
            {
                // Get a stream object for reading 		
                try
                {
                    Debug.Log("connected");
                    using (NetworkStream stream = socketConnection.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 					
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 						
                            serverMessage = Encoding.ASCII.GetString(incommingData);
                            TCP_recv_queue.Clear();
                            TCP_recv_queue.Enqueue(serverMessage);
                        }
                    }
                }
                catch
                {
                    Debug.Log("lost connection");
                    ReCon();
                }
            }
        }
        catch
        {
            ReCon();
        }
    }

    private void SendMessage()
    {
        if (socketConnection == null)
        {
            Debug.Log("NO SERVER AVAILABLE");
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = message2send;
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
