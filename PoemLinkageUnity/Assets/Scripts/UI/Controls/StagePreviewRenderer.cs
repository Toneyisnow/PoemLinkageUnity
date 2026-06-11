using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePreviewRenderer : MonoBehaviour
{
    public GameObject background = null;
    public GameObject locker = null;
    public GameObject shownPoem = null;

    public GameObject star1 = null;
    public GameObject star2 = null;
    public GameObject star3 = null;

    private StageRecord stageRecord = null;

    private int stageId = 0;

    public int StageId
    {
        get
        {
            return stageRecord == null ? stageId : stageRecord.StageId;
        }
    }

    private Action<int> callback = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int stageId)
    {
        Debug.Log($"StagePreviewRenderer.Initialize: stageId={stageId}");

        // Read the records
        stageRecord = GlobalStorage.LoadRecord(stageId);
        if (stageRecord != null)
        {
            this.SetEnable(true, stageRecord.HighestScore);
            Debug.Log($"StagePreviewRenderer: Loaded record for stageId={stageId}, HighestScore={stageRecord.HighestScore}");
        }
        else
        {
            this.stageId = stageId;
            this.SetEnable(false, 0);
            Debug.Log($"StagePreviewRenderer: No record found for stageId={stageId}, stage is locked");
        }

        // Load preview image
        var sprite = Resources.Load<Sprite>(string.Format(@"images/stage_{0}_pre", stageId));
        if (sprite != null)
        {
            var renderer = background.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = sprite;
                Debug.Log($"StagePreviewRenderer: Loaded preview image for stageId={stageId}");
            }
            else
            {
                Debug.LogError($"StagePreviewRenderer: SpriteRenderer component not found on background GameObject");
            }
        }
        else
        {
            Debug.LogWarning($"StagePreviewRenderer: Failed to load preview image 'images/stage_{stageId}_pre'");
        }

        // Load poem image
        sprite = Resources.Load<Sprite>(string.Format(@"images/stage_{0}_poem", stageId));
        if (sprite != null)
        {
            var renderer = shownPoem.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = sprite;
                Debug.Log($"StagePreviewRenderer: Loaded poem image for stageId={stageId}");
            }
        }
        else
        {
            Debug.LogWarning($"StagePreviewRenderer: Failed to load poem image 'images/stage_{stageId}_poem'");
        }
    }

    public void SetCallback(Action<int> action)
    {
        this.callback = action;
    }

    public void SetEnable(bool enabled, int star)
    {
        if (!enabled)
        {
            this.star1.SetActive(false);
            this.star2.SetActive(false);
            this.star3.SetActive(false);
            this.locker.SetActive(true);
            this.shownPoem.SetActive(false);

            // Grey out
            Material myNewMaterial = new Material(Shader.Find("Sprites/GrayScale"));
            background.GetComponent<SpriteRenderer>().material = myNewMaterial;
        }
        else
        {
            this.star1.SetActive(star >= 1);
            this.star2.SetActive(star >= 2);
            this.star3.SetActive(star >= 3);
            this.locker.SetActive(false);
            this.shownPoem.SetActive(star >= 1);

            if (stageRecord.JustCompleted)
            {
                if (this.shownPoem.GetComponent<FadeIn>() == null)
                {
                    this.shownPoem.AddComponent<FadeIn>();
                }
                stageRecord.JustCompleted = false;
                GlobalStorage.SaveRecord(stageRecord);
            }
            
        }
    }

    private void OnMouseDown()
    {
        if (this.callback != null)
        {
            this.callback(this.StageId);
        }
    }

}
