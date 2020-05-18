using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActivity : BaseActivity
{
    private float timeSpan = 0;

    private float startTime = 0;
    public DelayActivity(float timeSpan)
    {
        this.timeSpan = timeSpan;
    }


    public override bool HasFinished()
    {
        return Time.realtimeSinceStartup > startTime + timeSpan;
    }

    public override void OnBeginning()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public override void OnFinished()
    {

    }

    public override void Update()
    {

    }
}
