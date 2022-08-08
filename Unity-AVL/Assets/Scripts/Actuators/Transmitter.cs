using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : AbstractDevice {
    [SerializeField]
    protected AbstractReceiver receiver = null;

    [SerializeField]
    protected float maxDistance = 15f;

    public override void CommandDevice(float[] options) {
        if(this.receiver == null) {
            return;
        }

        Vector3 difference = this.transform.position - this.receiver.transform.position;
        
        if(difference.magnitude > this.maxDistance) {
            return;
        }

        this.receiver.ReceiveMessage(options[1]);
    }

    public override void ReadDevice(float[] memory, int[,,] memoryPixels) { }
}
