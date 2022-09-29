using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTOS : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected TaskList taskList = null;

    [SerializeField]
    protected DeviceRegistry deviceRegistry = null;

    [SerializeField]
    protected PhysicsBody body = null;

    protected int taskIndex = 0;

    protected TaskInterface[] tasks = null;

    void Start() {
        this.tasks = this.taskList.GetTasks();

        if(this.tasks.Length < 1) {
            Debug.Log("There are no tasks added to the task list. No tasks will be executed as a result.");
        }
    }

    void FixedUpdate()
    {
        if(this.tasks == null || this.deviceRegistry == null || this.body == null) {
            return;
        }

        if(this.taskIndex >= this.tasks.Length) {
            this.taskIndex = 0;
        }

        this.deviceRegistry.ReadSensors();
        
        if (this.tasks.Length == 0) {
            return;
        }

        TaskInterface task = this.tasks[this.taskIndex];
        task.Execute(this.deviceRegistry);

        this.deviceRegistry.CommandActuators();
        this.body.UpdatePhysics();

        this.taskIndex++;
    }
}
