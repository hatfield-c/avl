// ======================================================
// Copyright (C) 2019 BME Automated Drive Lab
// This program and the accompanying materials
// are made available under the terms of the MIT license.
// ======================================================
// Author: Matyas Szalai 
// Date: 2019. 11. 10.
// ======================================================

using System.Collections;
using UnityEngine;


public class scr_camera : MonoBehaviour
{

    public Transform target;
    public float distance;
    public float height;
    public float rotationDamping;
    public float heightDamping;
    public float zoomRatio;
    public float defaultFOV; // FOV Stand For Field Of View

    private float rotation_vector;

    //public Vector3 offset;
    public float offset_angle;

    void FixedUpdate()
    {
        Vector3 loccal_velocity = target.InverseTransformDirection(target.GetComponent<Rigidbody>().velocity);
        if (loccal_velocity.z < -0.5f)
        {
            rotation_vector = target.eulerAngles.y;
        }
        else
        {
            rotation_vector = target.eulerAngles.y;
        }

        float acceleration = target.GetComponent<Rigidbody>().velocity.magnitude;
        Camera.main.fieldOfView = defaultFOV + acceleration * zoomRatio * Time.deltaTime;

    }
    private void LateUpdate()
    {
        float wantedAngle = rotation_vector + offset_angle;

        float wantedHeight = target.position.y + height;
        float myAngle = transform.eulerAngles.y;
        float myHeight = transform.position.y;

        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
        myHeight = Mathf.LerpAngle(myHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        Vector3 temp = transform.position;
        temp.y = myHeight;
        transform.position = temp;

        transform.LookAt(target);


    }
}