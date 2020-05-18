using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageScene : MonoBehaviour
{
    public GameObject StagePreviewPrefab = null;

    public List<GameObject> previewAnchors = null;

    public GameObject categoryTitle = null;

    public GameObject btnBack = null;

    private ActivityManager activityManager;

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
        GlobalStorage.LoadSpriteDictionary();

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

        
        // Play animation to show title and pre
        activityManager = this.gameObject.GetComponent<ActivityManager>();
        if (activityManager == null)
        {
            return;
        }
        activityManager.Initialize(false);

        var moveTo = categoryTitle.AddComponent<MoveTo>();
        moveTo.Initialize(new Vector2(0, 5.5f), 0.6f);

        var delay = new DelayActivity(0.5f);
        activityManager.PushActivity(delay);

        var bundle = new BundleActivity();
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

            var fadeIn = new FadeInActivity(preview, 0.6f);
            fadeIn.InitObject();
            bundle.AddActivity(fadeIn);
        }

        activityManager.PushActivity(bundle);
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
