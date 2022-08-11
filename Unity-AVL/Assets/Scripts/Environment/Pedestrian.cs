using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0f;

    [SerializeField]
    protected float resetDistance = 0f;

    [SerializeField]
    protected float minEgoDistance = 0f;

    [SerializeField]
    protected float reverseDistance = 0f;

    [SerializeField]
    protected float reverseAngle = 0f;

    [SerializeField]
    protected float waitTime = 0f;

    protected enum PedestrianState {
        uninitialized,
        waiting,
        walking
    };

    protected Transform egoVehicle = null;
    protected Vector3 target = new Vector3();
    protected Vector3 spawn = new Vector3();
    protected Vector3 bounds = new Vector3();

    protected PedestrianState currentState = PedestrianState.uninitialized;
    protected float waitTimer = 0f;

    public void Init(Transform egoVehicle, Vector3 spawn, Vector3 bounds) {
        this.egoVehicle = egoVehicle;
        this.spawn = spawn;
        this.bounds = bounds;

        this.transform.position = this.PickValidPosition();
        this.RandomMovement();
    }

    protected Vector3 PickValidPosition() {
        Vector3 position = new Vector3(
            this.spawn.x + Random.Range(-this.bounds.x, this.bounds.x),
            this.transform.position.y,
            this.spawn.z + Random.Range(-this.bounds.z, this.bounds.z)
        );

        return position;
    }

    protected void ReverseMovement() {
        this.target = this.transform.position + (this.transform.forward * -this.reverseDistance);
        this.transform.LookAt(this.target);
        this.currentState = PedestrianState.walking;
    }

    protected void RandomMovement() {
        this.target = this.PickValidPosition();
        this.transform.LookAt(this.target);
        this.currentState = PedestrianState.walking;
    }

    void FixedUpdate()
    {
        if(this.currentState == PedestrianState.uninitialized) {
            return;
        }

        if (this.currentState == PedestrianState.waiting) {
            this.waitTimer -= Time.fixedDeltaTime;

            if (this.waitTimer <= 0) {
                this.ReverseMovement();
            } else {
                return;
            }
        }

        Vector3 egoDifference = this.egoVehicle.position - this.transform.position;
        float egoAngle = Vector3.Angle(this.transform.forward, egoDifference);
        if(egoDifference.magnitude <= this.minEgoDistance && egoAngle <= this.reverseAngle) {
            this.waitTimer = this.waitTime;
            this.currentState = PedestrianState.waiting;
            
            return;
        }

        Vector3 resetDifference = this.target - this.transform.position;
        if(resetDifference.magnitude <= this.resetDistance) {
            this.RandomMovement();
        }

        this.transform.position += this.transform.forward * this.speed;
    }
}
