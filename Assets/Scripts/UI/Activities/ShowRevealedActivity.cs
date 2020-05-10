using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRevealedCharActivity : BaseActivity
{
    private static float originScale = 3.0f;

    private static float maxScale = 5.0f;

    private GameObject charNode;

    private float t;

    private float timeSpan = 1.0f;

    private float beginTime = 0;

    public ShowRevealedCharActivity(GameObject charNode, float timeSpan)
    {
        this.charNode = charNode;
        this.timeSpan = timeSpan;
    }

    public override bool HasFinished()
    {
        return (Time.realtimeSinceStartup - beginTime >= timeSpan);
    }

    public override void OnBeginning()
    {
        t = 0;
        beginTime = Time.realtimeSinceStartup;

        var renderer = charNode.GetComponent<SpriteRenderer>();
        var originColor = renderer.color;
        renderer.color = new Color(originColor.r, originColor.g, originColor.b, 1.0f);

        charNode.transform.localScale = new Vector3(originScale, originScale, 1.0f);
    }

    public override void OnFinished()
    {
        var renderer = charNode.GetComponent<SpriteRenderer>();
        var originColor = renderer.color;
        renderer.color = new Color(originColor.r, originColor.g, originColor.b, 0.0f);
    }

    // Update is called once per frame
    public override void Update()
    {
        var rate = (Time.realtimeSinceStartup - beginTime) / timeSpan;
        var renderer = charNode.GetComponent<SpriteRenderer>();
        var originColor = renderer.color;
        renderer.color = new Color(originColor.r, originColor.g, originColor.b, rate);

        var scale = originScale + rate * (maxScale - originScale);
        charNode.transform.localScale = new Vector3(scale, scale, 1.0f);
    }
}
