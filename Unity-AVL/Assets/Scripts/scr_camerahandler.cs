// ======================================================
// Copyright (C) 2019 BME Automated Drive Lab
// This program and the accompanying materials
// are made available under the terms of the MIT license.
// ======================================================
// Author: Matyas Szalai 
// Date: 2019. 11. 10.
// ======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scr_camerahandler : MonoBehaviour
{
    static int NumOfCameras = 4;
    [Header("Cameras")]
    [SerializeField] private GameObject[] camera = new GameObject[NumOfCameras];

    private int CameraState = 0;

    void Start()
    {
        camera[0].SetActive(true);
        for (int i = 1; i < NumOfCameras; i++)
        {
            camera[i].SetActive(false);
        }
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeCamera();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Exit the game");
            Application.Quit();
        }
    }

    private void ChangeCamera()
    {
            CameraState += 1;
            if (CameraState < NumOfCameras)
            {
                camera[CameraState].SetActive(true);
                for (int i = 0; i < CameraState; i++)
                {
                    camera[i].SetActive(false);
                }
                for (int j = CameraState + 1; j < NumOfCameras; j++)
                {
                    camera[j].SetActive(false);
                }
            }
            else
            {
                CameraState = 0;
                camera[0].SetActive(true);
                for (int i = 1; i < NumOfCameras; i++)
                {
                    camera[i].SetActive(false);
                }
            }
    }
}
