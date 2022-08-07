using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSoundSource : MonoBehaviour
{
    public abstract int GetSound(Vector3 position);

}
