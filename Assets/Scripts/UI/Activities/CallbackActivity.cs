using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackActivity : BaseActivity
{
    private Action callbackAction = null;

    public CallbackActivity(Action callback)
    {
        this.callbackAction = callback;
    }



    public override bool HasFinished()
    {
        return true;
    }

    public override void OnBeginning()
    {
        if (this.callbackAction != null)
        {
            this.callbackAction();
        }
    }

    public override void OnFinished()
    {
        
    }

    public override void Update()
    {
        
    }
}
