using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneSensor : AbstractDevice
{
    [SerializeField]
    protected List<AbstractSoundSource> sounds = new List<AbstractSoundSource>();

    public override void CommandDevice(float[] options) { }

    public override void ReadDevice(float[] memory, int[,,] memoryPixels) {
        int maxSound = 0;
        
        for(int i = 0; i < this.sounds.Count; i++) {
            int sound = this.sounds[i].GetSound(this.transform.position);
            
            if(sound > maxSound) {
                maxSound = sound;
            }
        }

        memory[0] = maxSound;
    }
}
