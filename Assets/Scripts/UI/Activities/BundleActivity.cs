using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleActivity : BaseActivity
{
    private List<BaseActivity> activityList = null;


    public BundleActivity()
    {
        this.activityList = new List<BaseActivity>();
    }

    public void AddActivity(BaseActivity activity)
    {
        this.activityList.Add(activity);
    }

    public override bool HasFinished()
    {
        foreach(var activity in activityList)
        {
            if (activity != null && !activity.HasFinished())
            {
                return false;
            }
        }

        return true;
    }

    public override void OnBeginning()
    {
        foreach (var activity in activityList)
        {
            if (activity != null)
            {
                activity.OnBeginning();
            }
        }
    }

    public override void OnFinished()
    {
        
    }

    public override void Update()
    {
        foreach (var activity in activityList)
        {
            if (activity.HasFinished())
            {
                activity.OnFinished();
            }

            activity.Update();
        }
    }
}
