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
        // The ActivityManager only calls OnFinished() once, when the whole bundle
        // reports finished. Because the children here typically share the same
        // duration, they all finish on the same frame and the manager drops the
        // bundle without ever calling Update() again -- so this is the only place
        // the children's OnFinished() (e.g. FadeOutActivity's SetActive(false))
        // reliably runs. Forward it to every child.
        foreach (var activity in activityList)
        {
            if (activity != null)
            {
                activity.OnFinished();
            }
        }
    }

    public override void Update()
    {
        foreach (var activity in activityList)
        {
            // Only advance children that are still running. Finished children are
            // finalized once via OnFinished() when the bundle completes.
            if (activity != null && !activity.HasFinished())
            {
                activity.Update();
            }
        }
    }
}
