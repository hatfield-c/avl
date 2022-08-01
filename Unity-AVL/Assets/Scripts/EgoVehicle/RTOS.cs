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

    protected int taskIndex = 0;

    protected TaskInterface[] tasks = null;

    void Start() {
        this.tasks = this.taskList.GetTasks();
    }

    void FixedUpdate()
    {
        if(this.taskIndex >= this.tasks.Length) {
            this.taskIndex = 0;
        }

        this.deviceRegistry.ReadSensors();

        TaskInterface task = this.tasks[this.taskIndex];
        task.Execute(this.deviceRegistry);

        this.taskIndex++;
    }
}
