using UnityEngine;

public interface IStorable
{
    GameObject GetMyGameObject();
    string GetArchetype();
    void Enable();
    void Disable();
}
