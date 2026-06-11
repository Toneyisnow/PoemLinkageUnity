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
        this.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        float a = 1 - (Time.realtimeSinceStartup - timeStart) / timeSpan;

        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        foreach (var t in ts)
        {
            if (t == null || t.gameObject == null)
            {
                continue;
            }

            // Not every child carries a SpriteRenderer (e.g. TextMeshPro labels or
            // empty container objects). GetComponent returns null for those, so we
            // must null-check rather than dereference it: on IL2CPP (iOS) the
            // dereference throws NullReferenceException, which the old
            // catch(MissingComponentException) did not catch, aborting the fade.
            var spriteRenderer = t.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var clr = spriteRenderer.color;
                spriteRenderer.color = new Color(clr.r, clr.g, clr.b, a);
            }
        }
    }
}
