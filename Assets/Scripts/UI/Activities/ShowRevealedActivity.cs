using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRevealedCharActivity : BaseActivity
{
    private static Vector3 originScale = new Vector3(1.0f, 1.0f, 1.0f);

    private static Vector3 maxScale = new Vector3(1.6f, 1.6f, 1.0f);

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
        var originColor = renderer.material.color;
        renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, 1.0f);
    }

    public override void OnFinished()
    {
        var renderer = charNode.GetComponent<SpriteRenderer>();
        var originColor = renderer.material.color;
        renderer.material.color = new Color(originColor.r, originColor.g, originColor.b, 0.0f);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
