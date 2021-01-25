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

public class scr_Main : MonoBehaviour
{
    [Header("Scripts gameobject")]
    public GameObject MyScripts;

    [Header("Map offsets")]
    public int posoffset_x = 250;
    public int posoffset_y = 250;

    [Header("Vehicle ID Lists")]
    private string[] IDlist = new string[5];
    private string[] oldIDlist = new string[5];
    GameObject[] car = new GameObject[5];
    private Dictionary<string, VehicleUpdateData> CarDict = new Dictionary<string, VehicleUpdateData>();


    [Header("timer for motion vizualisation")]
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        string[] MoveAbleVehList = new string[] {"Tocus1", "Tocus2", "Tocus3", "Tocus4", "Tocus5" };  //array of moving vehicles or objects 

        for (int i = 0; i < MoveAbleVehList.Length; i++)
        {
            car[i] = GameObject.Find(MoveAbleVehList[i]);       //Find all af the object from the previous array
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.deltaTime;

        string Rx = MyScripts.GetComponent<scr_TCP>().RxMsg(); //get the incoming string from TCP script
        SplitData(Rx, timer);
    }

    public void SplitData(string message, float timer)           //split incoming string per vehicle
    {
        if(message == null) {
            return;
        }

        Array.Clear(IDlist, 0, IDlist.Length);
        string[] messageComponents = message.Split(scr_TCP.MSG_DELIM);
        string[] dataPerVehicle = messageComponents[2].Split(scr_TCP.DATA_DELIM);       

        for (int i = 0; i < dataPerVehicle.Length; i++)     
        {
            VehicleUpdateData data = JsonUtility.FromJson<VehicleUpdateData>(dataPerVehicle[i]);

            //CarInfo car = new CarInfo(DataPerVehicle[i]);       //creating a CarInfo class with name car
            IDlist[i] = data.vehicleId;  //adding the id to the ID list to check
            if (Array.IndexOf(oldIDlist, data.vehicleId) == -1 && data.vehicleId != null) //check the ID, if the new ID is element of the old list, it returns with the index, if not element, it returns with -1
            {
                CarDict.Add(data.vehicleId, data);      //fill up dictionary
            }
            else if (data.vehicleId != null)
            {
                CarDict[data.vehicleId] = data;       //update dictionary
            }
            else
            {

            }
        }
        Transform(CarDict, IDlist);    //call the transfrom function
        oldIDlist = IDlist; //update the list

    }

    public void Transform(Dictionary<string, VehicleUpdateData> CarDict, string[] IDs) {
        int num = CarDict.Count;
        int j = 6;  //default

        for (int i = 0; i < num; i++)  //running through all vehicle
        {
            VehicleUpdateData tmp_CarInfo = CarDict[IDs[i]];  //ccreating tmp CarInfo to handle the current object
            switch (tmp_CarInfo.vehicleId) // If the ID is matching with one case, they get j index to this element from "MoveAbleVehList"
                {
                case "carA":
                    j = 0;
                    break;
                case "carB":
                    j = 1;
                    break;
                case "carC":
                    j = 2;
                    break;
                case "carD":
                    j = 3;
                    break;
                case "carE":
                    j = 4;
                    break;
                default:
                    print("something is wrong");
                    break;
            }
            Vector3 tempPos = car[j].transform.position;               // get the current position
            tempPos.x = (float)(tmp_CarInfo.position[0] + posoffset_x);       //adding the offset
            tempPos.z = (float)(tmp_CarInfo.position[1] + posoffset_y);
            Quaternion tempRot = car[j].transform.rotation;            // get the current position
            Quaternion rot;
            Vector3 ydir = new Vector3(0, 1, 0);    //y direction to rotation
            rot = Quaternion.AngleAxis((tmp_CarInfo.heading), ydir);
            car[j].transform.SetPositionAndRotation(tempPos, rot);  //set the position and the rotation
            
            VehicleBase vehicle = car[j].GetComponent<VehicleBase>();

            if (vehicle == null) {
                car[j].GetComponent<scr_VehicleHandler>().CalculateSteering(tmp_CarInfo.heading, tmp_CarInfo.speed, timer);
                car[j].GetComponent<scr_VehicleHandler>().BrakeLightSwitch(tmp_CarInfo.brake);
            } else {
                VehicleState state = vehicle.GetVehicleState();
                state.SetHeading(tmp_CarInfo.heading);
                state.SetSpeed(tmp_CarInfo.speed);
                state.SetTimer(timer);
                state.SetIsBraking(tmp_CarInfo.brake);

                vehicle.UpdateState();
            }
        }
    }
}

public class CarInfo
{
    public string vehid;
    public float posx;
    public float posy;
    public float speed;
    public float heading;
    public float brakelight; //float
    public int sizeclass;
    public Color color;

    public bool brakestate; //bool

    public CarInfo(string txt)
    {
        if (txt.Contains(";"))
        {
            
            string[] a = txt.Split(';'); //split the data of a vehicle, the data order: vehid, posx, posy, speed, heading, brakelight state, sizeclass
            if (a.Length >= 7)
            {
                vehid = a[0];
                posx = (float)Convert.ToDouble(a[1], new CultureInfo("en-US"));
                posy = (float)Convert.ToDouble(a[2], new CultureInfo("en-US"));
                speed = (float)Convert.ToDouble(a[3], new CultureInfo("en-US"));
                heading = (float)Convert.ToDouble(a[4], new CultureInfo("en-US"));
                brakelight = (float)Convert.ToDouble(a[5], new CultureInfo("en-US"));
                sizeclass = (int)Convert.ToDouble(a[6], new CultureInfo("en-US"));      //not used in this project
                color = this.extractColor(a[7]);

                if (brakelight == 1)
                    brakestate = true;
                else
                    brakestate = false;
            }
            else
            {
                Debug.Log("incorrect messeage length");
            }
        }
    }

    protected Color extractColor(string s) {
        string rawDigits = s.Substring(1, s.Length - 2);
        string[] digits = rawDigits.Split(',');

        Color color = new Color(
            (float)Convert.ToDouble(digits[0], new CultureInfo("en-US")) / 255f,
            (float)Convert.ToDouble(digits[1], new CultureInfo("en-US")) / 255f,
            (float)Convert.ToDouble(digits[2], new CultureInfo("en-US")) / 255f,
            (float)Convert.ToDouble(digits[3], new CultureInfo("en-US")) / 255f
        );

        return color;
    }
}