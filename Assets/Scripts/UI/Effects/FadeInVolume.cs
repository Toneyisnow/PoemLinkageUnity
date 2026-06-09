using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInVolume : MonoBehaviour
{
    private float timeSpan = 6.0f;

    private AudioSource audioSource = null;

    private float startTime = 0;

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
        audioSource.volume = 0;
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
            // End
            audioSource.volume = 1.0f;
            Destroy(this);
            return;
        }

        audioSource.volume = delta / timeSpan;
    }
}
