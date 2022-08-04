using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 0.33f;

    [SerializeField]
    protected float drag = 0.97f;

    protected float velocity = 0f;

    void FixedUpdate()
    {
        this.transform.position = this.transform.position + (this.transform.forward * this.velocity);

        this.ApplyDrag(this.drag);
    }
    
    public void AddSpeed(float amount) {
        this.velocity = this.velocity + amount;

        if (this.velocity > this.maxSpeed) {
            this.velocity = this.maxSpeed;
        }
    }

    public void ApplyDrag(float amount) {
        this.velocity = this.velocity * amount;
    }

    public void Rotate(float amount) {
        this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.up * amount);
    }
}
