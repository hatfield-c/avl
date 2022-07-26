using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTOS : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected TaskList taskList = null;

    [SerializeField]
    protected AddressBus addressBus = null;

    [SerializeField]
    protected CommandBus commandBus = null;

    [SerializeField]
    protected MemoryContainer memoryContainer = null;

    protected const int MEM_SIZE = 1024;
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

        TaskInterface task = this.tasks[this.taskIndex];
        task.Execute(this.addressBus, this.commandBus, this.memoryContainer.GetMemory());

        this.taskIndex++;
    }

    public static int GetMemSize() {
        return RTOS.MEM_SIZE;
    }
}
