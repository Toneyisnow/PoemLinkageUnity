using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quickly pulses a set of character nodes larger and smaller around their current
/// scale to indicate the two selected characters match but cannot be connected.
/// </summary>
public class ScalePulseActivity : BaseActivity
{
    private List<GameObject> nodes;

    private List<Vector3> baseScales;

    private float amplitude;

    private int pulseCount;

    private float duration;

    private float t;

    public ScalePulseActivity(List<GameObject> nodes, float amplitude = 0.1f, int pulseCount = 4, float duration = 0.6f)
    {
        this.nodes = nodes;
        this.amplitude = amplitude;
        this.pulseCount = pulseCount;
        this.duration = duration;
    }

    public override void OnBeginning()
    {
        t = 0;

        // Capture the scale each node starts at so the pulse oscillates around it.
        baseScales = new List<Vector3>();
        foreach (var node in nodes)
        {
            baseScales.Add(node != null ? node.transform.localScale : Vector3.one);
        }
    }

    public override bool HasFinished()
    {
        return t >= duration;
    }

    public override void Update()
    {
        t += Time.deltaTime;
        float ratio = Mathf.Clamp01(t / duration);

        // Oscillate larger (+) then smaller (-); ends back at the base scale after pulseCount cycles.
        float factor = 1.0f + amplitude * Mathf.Sin(ratio * Mathf.PI * 2.0f * pulseCount);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].transform.localScale = baseScales[i] * factor;
            }
        }
    }

    public override void OnFinished()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].transform.localScale = baseScales[i];
            }
        }
    }
}
