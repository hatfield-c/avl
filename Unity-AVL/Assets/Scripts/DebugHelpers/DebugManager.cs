using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    protected CameraSensor cameraSensor = null;

    [SerializeField]
    protected CameraVisualizer cameraVisualizer = null;

    void Start()
    {
        this.cameraVisualizer.Init(this.cameraSensor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
