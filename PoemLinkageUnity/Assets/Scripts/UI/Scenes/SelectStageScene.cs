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

    public GameObject background = null;

    // The title animates from the screen center up to the top edge on entry.
    private const float TitleTopY = 5.5f;
    private const float TitleMoveTimeSpan = 0.3f;

    private ActivityManager activityManager;

    public int SelectedCategory
    {
        get; private set;
    }

    private void Awake()
    {
        if (MyUnitySingleton.Instance != null)
        {
            MyUnitySingleton.Instance.PlayBackgroundAudio();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalStorage.LoadSpriteDictionary();

        this.SelectedCategory = GlobalStorage.CurrentCategory;

        // The Background object's sprite is assigned at runtime (the same pattern
        // used by PrologueScene / MainGameScene) so it shows even if the scene's
        // sprite reference is missing.
        GameObject backgroundObject = background != null ? background : GameObject.Find("Background");
        if (backgroundObject != null)
        {
            SpriteRenderer backgroundRenderer = backgroundObject.GetComponent<SpriteRenderer>();
            if (backgroundRenderer != null)
            {
                backgroundRenderer.sprite = Resources.Load<Sprite>(@"images/select_background");
            }
        }

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
        moveTo.Initialize(new Vector2(0, TitleTopY), TitleMoveTimeSpan);

        var delay = new DelayActivity(0.5f);
        activityManager.PushActivity(delay);

        // Debug logging for previewAnchors
        Debug.Log($"SelectStageScene: previewAnchors is null = {this.previewAnchors == null}");
        if (this.previewAnchors != null)
        {
            Debug.Log($"SelectStageScene: previewAnchors count = {this.previewAnchors.Count}");
        }

        var bundle = new BundleActivity();
        for (int i = 0; i < 9; i++)
        {
            try
            {
                if (this.previewAnchors == null || this.previewAnchors.Count <= i)
                {
                    Debug.LogWarning($"SelectStageScene: previewAnchors[{i}] is not available. previewAnchors count: {(this.previewAnchors == null ? 0 : this.previewAnchors.Count)}");
                    continue;
                }

                GameObject previewAnchor = this.previewAnchors[i];
                if (previewAnchor == null)
                {
                    Debug.LogWarning($"SelectStageScene: previewAnchor[{i}] is null");
                    continue;
                }

                GameObject preview = GameObject.Instantiate(StagePreviewPrefab);
                if (preview == null)
                {
                    Debug.LogError($"SelectStageScene: Failed to instantiate StagePreviewPrefab for index {i}");
                    continue;
                }

                // preview.transform.parent = previewAnchor.transform;
                preview.transform.localPosition = previewAnchor.transform.position;
                preview.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                var renderer = preview.GetComponent<StagePreviewRenderer>();
                if (renderer == null)
                {
                    Debug.LogError($"SelectStageScene: StagePreviewRenderer not found on preview prefab");
                    continue;
                }

                int stageId = this.SelectedCategory * 100 + i + 1;
                Debug.Log($"SelectStageScene: Creating preview for stageId={stageId}");
                renderer.Initialize(stageId);
                renderer.SetCallback((stage) => { this.EnterStage(stage); });

                var fadeIn = new FadeInActivity(preview, 0.6f);
                fadeIn.InitObject();
                bundle.AddActivity(fadeIn);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"SelectStageScene: Exception creating preview at index {i}: {ex.Message}\n{ex.StackTrace}");
            }
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
        // Let SelectCategoryScene play the title slide-in; switching scenes here
        // removes the stage previews so they don't overlap the moving title.
        GlobalStorage.AnimateCategoryTitle = true;
        SceneManager.LoadScene("SelectCategoryScene");
    }
}
