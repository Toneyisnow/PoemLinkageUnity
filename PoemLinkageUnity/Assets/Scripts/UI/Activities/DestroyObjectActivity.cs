using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectActivity : BaseActivity
{
    private List<GameObject> targetObjects;


    public DestroyObjectActivity(List<GameObject> targetObjects)
    {
        this.targetObjects = targetObjects;
    }

    public override bool HasFinished()
    {
        return true;
    }

    public override void OnBeginning()
    {
        if(this.targetObjects == null)
        {
            return;
        }

        foreach(var go in this.targetObjects)
        {
            UnityEngine.Object.Destroy(go);
        }
    }

    public override void OnFinished()
    {
        // Nothing to do
    }

    public override void Update()
    {
        
    }

}
