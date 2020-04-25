using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageScene : MonoBehaviour
{
    public GameObject StagePreviewPrefab = null;

    public List<GameObject> previewAnchors = null;


    public int SelectedCategory
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SelectedCategory = GlobalStorage.CurrentCategory;

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
            preview.transform.position = previewAnchor.transform.position;
            var renderer = preview.GetComponent<StagePreviewRenderer>();

            int stageId = this.SelectedCategory * 100 + i + 1;
            renderer.Initialize(stageId);
            renderer.SetCallback((stage) => { this.EnterStage(stage); });

            var stageRecord = GlobalStorage.LoadRecord(stageId);
            if (stageRecord != null)
            {
                renderer.SetEnable(true, stageRecord.HighestScore);
            }
            else
            {
                renderer.SetEnable(false, 0);
            }
        }
    }

    public void EnterStage(int stageId)
    {
        Debug.Log("SelectStageScene: stageId=" + stageId);
        GlobalStorage.CurrentStage = stageId;
        SceneManager.LoadScene("MainGameScene");
    }
}
