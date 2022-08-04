using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDevice : MonoBehaviour
{
    abstract public void CommandDevice(float[] options);

    abstract public void ReadDevice(float[] memory, int[,,] memoryPixels);
}
