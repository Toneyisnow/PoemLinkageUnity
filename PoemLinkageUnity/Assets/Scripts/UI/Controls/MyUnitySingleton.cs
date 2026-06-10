using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitySingleton : MonoBehaviour
{
    private static MyUnitySingleton instance = null;
    public static MyUnitySingleton Instance
    {
        get { return instance; }
    }

    private AudioSource backgroundAudio = null;

    // Spawn the singleton automatically when the game starts, before the first
    // scene loads. This guarantees the welcome background music object exists
    // (and survives scene loads) without having to place it manually in a scene.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("MyUnitySingleton");
            go.AddComponent<MyUnitySingleton>();
        }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        SetupBackgroundAudio();
    }

    private void SetupBackgroundAudio()
    {
        backgroundAudio = this.gameObject.GetComponent<AudioSource>();
        if (backgroundAudio == null)
        {
            backgroundAudio = this.gameObject.AddComponent<AudioSource>();
        }

        backgroundAudio.clip = Resources.Load<AudioClip>(@"mp3/bg_welcome");
        backgroundAudio.loop = true;
        backgroundAudio.playOnAwake = false;
        backgroundAudio.volume = 0;
        backgroundAudio.Play();

        // Fade the welcome music in over 2 seconds.
        FadeInVolume fadeIn = this.gameObject.AddComponent<FadeInVolume>();
        fadeIn.Initialize(2.0f);
    }

    // Resume the welcome music (used by the home / select scenes). MainGameScene
    // pauses it while a stage plays its own track.
    public void PlayBackgroundAudio()
    {
        if (backgroundAudio != null && !backgroundAudio.isPlaying)
        {
            backgroundAudio.Play();
        }
    }

    public void PauseBackgroundAudio()
    {
        if (backgroundAudio != null && backgroundAudio.isPlaying)
        {
            backgroundAudio.Pause();
        }
    }

    // Fade the welcome music out over the given time, then pause it. Used when
    // switching away from the home / select screens (e.g. starting a stage) so
    // the music does not cut off abruptly.
    public void FadeOutBackgroundAudio(float timeSpan = 2.0f)
    {
        if (backgroundAudio == null || !backgroundAudio.isPlaying)
        {
            return;
        }

        // Drop any in-progress fade-in so the two effects don't fight.
        FadeInVolume existingFadeIn = this.gameObject.GetComponent<FadeInVolume>();
        if (existingFadeIn != null)
        {
            Destroy(existingFadeIn);
        }

        FadeOutVolume fadeOut = this.gameObject.AddComponent<FadeOutVolume>();
        fadeOut.Initialize(timeSpan);
    }
}
