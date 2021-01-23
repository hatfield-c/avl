using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SovietAnimator : AVehicleAnimator {
    [SerializeField] protected SovietPrefab prefab = null;
    [SerializeField] private float wheelRotationRate = 0.1f;

    protected Vector3 wheelAngleBuffer = new Vector3();

    public override void Animate(VehicleState state) {
        this.BrakeLightSwitch(state.GetIsBraking());
        this.WheelHandler(state);
    }
    
    protected void WheelHandler(VehicleState state) {
        SovietPrefab.WheelData wheels = this.prefab.GetWheels();
        this.TurnWheel(wheels.frontRight.transform, state);
        this.TurnWheel(wheels.frontLeft.transform, state);
        this.TurnWheel(wheels.backRight.transform, state);
        this.TurnWheel(wheels.backLeft.transform, state);
    }

    protected void TurnWheel(Transform wheel, VehicleState state) {
        float xRotation = state.GetSpeed() * this.wheelRotationRate;

        wheel.Rotate(xRotation, 0, 0);
    }

    public void BrakeLightSwitch(bool state) {
        for (int i = 0; i < 2; i++) {
            Light light = this.prefab.GetBrakeLights()[i];
            if (state) {
                if (light.intensity < 20)
                    light.intensity += 5;
            } else {
                if (light.intensity > 0)
                    light.intensity -= 1;
            }
        }
    }
}
