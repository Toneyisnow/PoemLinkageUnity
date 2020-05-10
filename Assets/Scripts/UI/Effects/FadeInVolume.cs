using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInVolume : MonoBehaviour
{
    private float timeSpan = 6.0f;

    private AudioSource audioSource = null;

    private float startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Destroy(this);
        }

        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource == null)
        {
            Destroy(this);
        }

        float delta = Time.realtimeSinceStartup - startTime;
        if (delta > timeSpan)
        {
            // End
            audioSource.volume = 1.0f;
            Destroy(this);
        }

        audioSource.volume = delta / timeSpan;
    }
}
