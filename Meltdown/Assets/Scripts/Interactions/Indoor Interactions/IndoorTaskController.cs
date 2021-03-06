﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorTaskController : TaskController
{

    public GameObject[] lightTasks = new GameObject[4];
    public GameObject[] tapTasks = new GameObject[4];
    public GameObject[] saladTasks = new GameObject[4];

    public ToggleItem leftSwitch;
    public ToggleItem rightSwitch;

    protected override void loadTasks()
    {
        //add different task types to our task dictionary
        tasks.Add(TaskTypes.Tap, new TapTask());
        tasks.Add(TaskTypes.Light1, new LightSwitchTask(leftSwitch, TaskTypes.Light1));
        tasks.Add(TaskTypes.Light2, new LightSwitchTask(rightSwitch, TaskTypes.Light2));
        tasks.Add(TaskTypes.Salad, new SaladTask());
        tasks.Add(TaskTypes.Salad2, new SaladTask());
    }

    protected override void setupRepeatingTasks()
    {
        // Continually generate tasks
        InvokeRepeating("checkForNewTask", 1.0f, 0.5f);
    }

    //generates a new task from enum TaskTypes, based on rng
    protected override TaskTypes generateTask()
    {
        int newTask = Random.Range(8, 10);
        return (TaskTypes)System.Enum.Parse(typeof(TaskTypes), newTask.ToString());
    }

    protected override void generateTaskTime()
    {
        timeCount = 0.0f;
        newTaskTime = Random.Range(10.0f, 20.0f);
    }

    public void removeSaladTask()
    {
        int i;
        for (i = 0; i < taskList.Count; i++) {
            TaskTypes type = taskList[i];
            if (type == TaskTypes.Salad) {
                break;
            } else if (type == TaskTypes.Salad2) {
                break;
            }
        }
        taskList.RemoveAt(i);
        if (taskList.Count < 1)
        {
            addTask();
        }
        updateUI();
    }

    public bool containsTask(int num)
    {
        switch (num)
        {
            case 0:
                if (taskList.Contains(TaskTypes.Light1)) { return true; }
                return false;
            case 1:
                if (taskList.Contains(TaskTypes.Light2)) { return true; }
                return false;
            case 2:
                if (taskList.Contains(TaskTypes.Tap)) { return true; }
                return false;
            default:
                return false;
        }
    }

    public bool isFull()
    {
        if(taskList.Count > 3)
        {
            return true;
        }
        return false;
    }

    public void activateTask(TaskTypes task)
    {
        taskList.Add(task);
        updateUI();
        tasks[task].setupTask();
    }

    protected override void checkForNewTask()
    {
        //update time count, and if it reaches the time set to generate a new task at, do so.
        if (taskList.Count < maxTasks)
        {
            timeCount += 0.5f;
            if (timeCount >= newTaskTime && (!taskList.Contains(TaskTypes.Salad) || !taskList.Contains(TaskTypes.Salad2)))
            {
                addTask();
            }
        }

    }

    // Sets all tasks on the Task List UI to hidden (i.e. used to update UI or right at beginning before any tasks have been generated)
    protected override void hideAllUITasks() {
        foreach (GameObject task in lightTasks) {
            task.SetActive(false);
        }
        foreach (GameObject task in tapTasks) {
            task.SetActive(false);
        }
        foreach (GameObject task in saladTasks) {
            task.SetActive(false);
        }
    }


    // Updates the Task List UI on the level every time a new task is generated or completed
    protected override void updateUI () {
        hideAllUITasks();
        int i = 0;
        foreach (TaskTypes type in taskList) {
            switch (type) {
                case TaskTypes.Tap:
                    tapTasks[i].SetActive(true);
                    break;
                case TaskTypes.Light1:
                    lightTasks[i].SetActive(true);
                    break;
                case TaskTypes.Light2:
                    lightTasks[i].SetActive(true);
                    break;
                case TaskTypes.Salad:
                    saladTasks[i].SetActive(true);
                    break;
                case TaskTypes.Salad2:
                    saladTasks[i].SetActive(true);
                    break;
            }
            i++;
        }
    }
}
