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

        for(int i = 0; i < 9; i++)
        {
            GameObject previewAnchor = this.previewAnchors[i];
            GameObject preview = GameObject.Instantiate(StagePreviewPrefab);

            preview.transform.parent = previewAnchor.transform;
            var renderer = preview.GetComponent<StagePreviewRenderer>();

            int stageId = this.SelectedCategory * 100 + i;
            renderer.Initialize(stageId);
            renderer.SetCallback((stage) => { this.EnterStage(stage); });
        }

    }


    public void EnterStage(int stageId)
    {
        GlobalStorage.CurrentStage = stageId;
        SceneManager.LoadScene("MainGameScene");

    }

}
