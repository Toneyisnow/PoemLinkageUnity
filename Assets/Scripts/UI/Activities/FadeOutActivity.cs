using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutActivity : BaseActivity
{
    private GameObject gameObject;

    private float timeSpan = 0;

    private float timeStart = 0;

    public FadeOutActivity(GameObject gameObject, float timeSpan)
    {
        this.gameObject = gameObject;
        this.timeSpan = timeSpan;
    }

    public override bool HasFinished()
    {
        return (Time.realtimeSinceStartup - timeStart) > timeSpan;
    }

    public override void OnBeginning()
    {
        timeStart = Time.realtimeSinceStartup;
    }

    public override void OnFinished()
    {
        // Nothing
    }

    public override void Update()
    {
        float a = 1 - (Time.realtimeSinceStartup - timeStart) / timeSpan;

        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        foreach (var t in ts)
        {
            if (t != null && t.gameObject != null)
            {
                try
                {
                    var clr = t.GetComponent<SpriteRenderer>().color;
                    t.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, a);
                }
                catch (MissingComponentException)
                {

                }
            }
        }
    }
}
