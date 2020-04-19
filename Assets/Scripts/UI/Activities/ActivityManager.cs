using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    public Queue<BaseActivity> activityQueue = null;

    private BaseActivity currentActivity = null;

    void Start()
    {
        activityQueue = new Queue<BaseActivity>();
    }

    void Update()
    {
        if (currentActivity == null && activityQueue != null && activityQueue.Count > 0)
        {
            currentActivity = this.activityQueue.Dequeue();
            currentActivity.OnBeginning();
        }

        if (currentActivity == null)
        {
            return;
        }

        if (currentActivity.HasFinished())
        {
            currentActivity.OnFinished();
            currentActivity = null;
            return;
        }

        currentActivity.Update();
    }


    public void PushActivity(BaseActivity activity)
    {
        this.activityQueue.Enqueue(activity);
    }

    public void PushCallback(Action action)
    {
        CallbackActivity act = new CallbackActivity(action);
        this.activityQueue.Enqueue(act);
    }

}

