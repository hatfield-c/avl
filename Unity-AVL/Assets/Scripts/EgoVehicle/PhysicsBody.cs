using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    [SerializeField]
    protected float maxSpeed = 20f;

    [SerializeField]
    protected float drag = 0.97f;

    protected float velocity = 0f;

    public void UpdatePhysics() {
        this.transform.position = this.transform.position + (this.transform.forward * this.velocity * Time.fixedDeltaTime);

        this.ApplyDrag(this.drag);
    }
    
    public void AddSpeed(float amount) {
        this.velocity = this.velocity + amount;

        if (amount >= 0) {
            if (this.velocity > this.maxSpeed) {
                this.velocity = this.maxSpeed;
            }
        } else {
            if (this.velocity < -this.maxSpeed) {
                this.velocity = -this.maxSpeed;
            }
        }
    }

    public void ApplyDrag(float amount) {
        this.velocity = this.velocity * amount;
    }

    public void Rotate(float amount) {
        this.transform.eulerAngles = this.transform.eulerAngles + (Vector3.up * amount);
    }

    public float GetSpeed() {
        return this.velocity;
    }
}
