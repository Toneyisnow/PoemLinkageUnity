using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quickly rotates a set of character nodes left and right around the Z axis to
/// indicate the two selected characters do not match.
/// </summary>
public class RotateShakeActivity : BaseActivity
{
    private List<GameObject> nodes;

    private float maxAngle;

    private int shakeCount;

    private float duration;

    private float t;

    public RotateShakeActivity(List<GameObject> nodes, float maxAngle = 20.0f, int shakeCount = 3, float duration = 0.6f)
    {
        this.nodes = nodes;
        this.maxAngle = maxAngle;
        this.shakeCount = shakeCount;
        this.duration = duration;
    }

    public override void OnBeginning()
    {
        t = 0;
    }

    public override bool HasFinished()
    {
        return t >= duration;
    }

    public override void Update()
    {
        t += Time.deltaTime;
        float ratio = Mathf.Clamp01(t / duration);

        // Oscillate left (+) then right (-); ends back at 0 after shakeCount cycles.
        float angle = maxAngle * Mathf.Sin(ratio * Mathf.PI * 2.0f * shakeCount);

        foreach (var node in nodes)
        {
            if (node != null)
            {
                node.transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public override void OnFinished()
    {
        foreach (var node in nodes)
        {
            if (node != null)
            {
                node.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
