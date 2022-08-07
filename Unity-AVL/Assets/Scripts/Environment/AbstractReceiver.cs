using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractReceiver : MonoBehaviour
{
    public abstract void ReceiveMessage(float message);
}
