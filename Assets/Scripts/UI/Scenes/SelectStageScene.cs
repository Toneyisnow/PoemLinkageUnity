﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageScene : MonoBehaviour
{
    public GameObject StagePreviewPrefab = null;

    public List<GameObject> previewAnchors = null;

    public GameObject categoryTitle = null;

    public GameObject btnBack = null;

    public int SelectedCategory
    {
        get; private set;
    }

    private void Awake()
    {
        if (!MyUnitySingleton.Instance.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            MyUnitySingleton.Instance.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.SelectedCategory = GlobalStorage.CurrentCategory;

        if (btnBack != null && btnBack.GetComponent<CommonButton>() != null)
        {
            btnBack.GetComponent<CommonButton>().SetCallback(() => { this.BtnBackClicked(); });
        }

        if (categoryTitle != null)
        {
            categoryTitle.GetComponent<SpriteRenderer>().sprite = 
                Resources.Load<Sprite>(string.Format(@"images/category-title-{0}", this.SelectedCategory));
        }

        // Ensure the first Stage
        var stage101 = GlobalStorage.LoadRecord(101);
        if (stage101 == null)
        {
            stage101 = StageRecord.Create(101);
            GlobalStorage.SaveRecord(stage101);
        }

        for (int i = 0; i < 9; i++)
        {
            GameObject previewAnchor = this.previewAnchors[i];
            GameObject preview = GameObject.Instantiate(StagePreviewPrefab);

            // preview.transform.parent = previewAnchor.transform;
            preview.transform.localPosition = previewAnchor.transform.position;
            preview.transform.localScale = new Vector3(1.5f, 1.5f, 1);

            var renderer = preview.GetComponent<StagePreviewRenderer>();

            int stageId = this.SelectedCategory * 100 + i + 1;
            renderer.Initialize(stageId);
            renderer.SetCallback((stage) => { this.EnterStage(stage); });
        }
    }

    public void EnterStage(int stageId)
    {
        var record = GlobalStorage.LoadRecord(stageId);
        if (record == null)
        {
            return;
        }

        Debug.Log("SelectStageScene: stageId=" + stageId);
        GlobalStorage.CurrentStage = stageId;
        SceneManager.LoadScene("MainGameScene");
    }

    private void BtnBackClicked()
    {
        SceneManager.LoadScene("SelectCategoryScene");
    }
}
