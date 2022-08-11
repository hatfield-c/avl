using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    protected enum CarState {
        waiting,
        backward,
        forward
    };

    [SerializeField]
    protected MeshRenderer bodyRenderer = null;

    [SerializeField]
    protected MeshRenderer topRenderer = null;

    [SerializeField]
    protected float egoDistance = 0f;

    [SerializeField]
    protected float egoAngle = 0f;

    [SerializeField]
    protected float maxDistance = 0f;

    [SerializeField]
    protected float speed = 0f;

    protected Transform egoVehicle = null;
    protected Vector3 originalPosition = new Vector3();
    protected CarState currentState = CarState.waiting;
    protected float traveled = 0f;

    void Start() {
        this.originalPosition = this.transform.position;
    }

    void FixedUpdate() {
        if(this.currentState == CarState.waiting) {
            return;
        }

        if (this.currentState == CarState.forward) {
            this.transform.position += this.transform.forward * this.speed;

            this.traveled += this.speed;

            if (this.traveled >= this.maxDistance) {
                this.currentState = CarState.waiting;
                this.transform.position = this.originalPosition;
                this.traveled = 0;
            }

            return;
        }

        if (this.currentState == CarState.backward) {
            Vector3 egoDifference = this.egoVehicle.position - this.transform.position;
            float egoAlignment = Vector3.Angle(-this.transform.forward, egoDifference);
            
            if(egoDifference.magnitude <= this.egoDistance && egoAlignment <= this.egoAngle) {
                this.currentState = CarState.forward;

                Vector3 originDifference = this.originalPosition - this.transform.position;
                this.traveled = this.maxDistance - originDifference.magnitude;

                return;
            }

            this.transform.position += this.transform.forward * -this.speed;

            this.traveled += this.speed;

            if (this.traveled >= this.maxDistance) {
                this.currentState = CarState.forward;
                this.traveled = 0;
            }

            return;
        }

    }

    public void Move(Transform egoVehicle) {
        if(this.currentState != CarState.waiting) {
            return;
        }

        this.egoVehicle = egoVehicle;
        this.currentState = CarState.backward;
    }

    public void SetMaterial(Material material) {
        this.bodyRenderer.material = material;
        this.topRenderer.material = material;
    }
}
