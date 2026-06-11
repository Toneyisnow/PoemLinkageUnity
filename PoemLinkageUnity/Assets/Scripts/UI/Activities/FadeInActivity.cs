using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInActivity : BaseActivity
{
    private GameObject gameObject;

    private float timeSpan = 0;

    private float timeStart = 0;

    public FadeInActivity(GameObject gameObject, float timeSpan)
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
        float a = (Time.realtimeSinceStartup - timeStart) / timeSpan;
        SetChildrenAlpha(a);
    }

    public void InitObject()
    {
        SetChildrenAlpha(0);
    }

    // Sets the alpha on every child SpriteRenderer. Not every child carries a
    // SpriteRenderer (e.g. TextMeshPro labels or empty containers); GetComponent
    // returns null for those, so we null-check rather than dereference it. On
    // IL2CPP (iOS) the dereference throws NullReferenceException, which the old
    // catch(MissingComponentException) did not catch.
    private void SetChildrenAlpha(float a)
    {
        if (gameObject == null)
        {
            return;
        }

        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        foreach (var t in ts)
        {
            if (t == null || t.gameObject == null)
            {
                continue;
            }

            var spriteRenderer = t.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var clr = spriteRenderer.color;
                spriteRenderer.color = new Color(clr.r, clr.g, clr.b, a);
            }
        }
    }
}
