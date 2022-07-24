using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    protected TaskInterface[] tasks = new TaskInterface[] {
        new Task1(),
        new Task2()
    };

    public TaskInterface[] GetTasks() {
        return this.tasks;
    }
}
