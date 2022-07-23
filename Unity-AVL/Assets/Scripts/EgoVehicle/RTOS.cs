using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTOS : MonoBehaviour
{
    [SerializeField]
    protected List<string> tasks = new List<string>();

    protected int taskIndex = 0;

    void FixedUpdate()
    {
        if(this.taskIndex >= this.tasks.Count) {
            this.taskIndex = 0;
        }

        string task = this.tasks[this.taskIndex];
        Debug.Log("Executing task: " + task);

        this.taskIndex++;
    }
}
