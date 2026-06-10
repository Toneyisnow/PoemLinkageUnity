using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutVolume : MonoBehaviour
{
    private float timeSpan = 2.0f;

    private AudioSource audioSource = null;

    private float startTime = 0;

    private float startVolume = 1.0f;

    public void Initialize(float timeSpan)
    {
        this.timeSpan = timeSpan;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Destroy(this);
            return;
        }

        startTime = Time.realtimeSinceStartup;
        startVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
        {
            Destroy(this);
            return;
        }

        float delta = Time.realtimeSinceStartup - startTime;
        if (delta >= timeSpan)
        {
            // End: silence and pause the source, but restore the volume so the
            // next play (e.g. resuming on the select screens) starts at full.
            audioSource.Pause();
            audioSource.volume = startVolume;
            Destroy(this);
            return;
        }

        // Mirror of FadeInVolume's square-root curve so the music stays clearly
        // audible and then drops off, instead of a flat linear ramp.
        float ratio = delta / timeSpan;
        audioSource.volume = startVolume * Mathf.Sqrt(1.0f - ratio);
    }
}
