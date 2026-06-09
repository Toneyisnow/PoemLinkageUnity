using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCategoryScene : MonoBehaviour
{
    public GameObject categorySprite = null;

    public GameObject leftArror = null;

    public GameObject rightArror = null;

    public GameObject background = null;

    // Title slide-in: from the top edge down to its resting position, at the same
    // speed as the stage page's title animation.
    private const float TitleTopY = 5.5f;
    private const float TitleMoveTimeSpan = 0.3f;

    // Horizontal title slide when changing category. The old title flies out and
    // the new one flies in across a scene reload, so each half takes 0.1s for a
    // 0.2s total animation.
    private const float TitleSlideTimeSpan = 0.1f;

    // Guards against re-triggering a slide while one is already in progress.
    private bool isChangingCategory = false;

    // Start is called before the first frame update
    void Start()
    {
        // Keep the welcome background music playing on this page.
        if (MyUnitySingleton.Instance != null)
        {
            MyUnitySingleton.Instance.PlayBackgroundAudio();
        }

        int categoryId = GlobalStorage.CurrentCategory;

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

        string categoryTitleImge = string.Format(@"images/category-title-{0}", categoryId);
        categorySprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(categoryTitleImge);

        CommonButton button = categorySprite.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterCategory(); });

        leftArror.SetActive(categoryId > 1);
        rightArror.SetActive(categoryId < 3);
        
        leftArror.GetComponent<CommonButton>().SetCallback(() => { this.ChangeCategory(false); });
        rightArror.GetComponent<CommonButton>().SetCallback(() => { this.ChangeCategory(true); });

        // When returning from the stage page, slide the title down from the top
        // into its resting position.
        if (GlobalStorage.AnimateCategoryTitle)
        {
            GlobalStorage.AnimateCategoryTitle = false;

            Vector3 restPosition = categorySprite.transform.localPosition;
            categorySprite.transform.localPosition = new Vector3(restPosition.x, TitleTopY, restPosition.z);

            var moveTo = categorySprite.AddComponent<MoveTo>();
            moveTo.Initialize(new Vector2(restPosition.x, restPosition.y), TitleMoveTimeSpan);
        }
        // When arriving from a left/right arrow click, slide the new title in from
        // the matching side into its resting position.
        else if (GlobalStorage.CategorySlideDirection != 0)
        {
            int direction = GlobalStorage.CategorySlideDirection;
            GlobalStorage.CategorySlideDirection = 0;

            Vector3 restPosition = categorySprite.transform.localPosition;
            float offset = GetTitleOffScreenOffset();

            // direction ==  1 (clicked right): enter from the right (+X)
            // direction == -1 (clicked left):  enter from the left  (-X)
            float startX = restPosition.x + direction * offset;
            categorySprite.transform.localPosition = new Vector3(startX, restPosition.y, restPosition.z);

            var moveTo = categorySprite.AddComponent<MoveTo>();
            moveTo.Initialize(new Vector2(restPosition.x, restPosition.y), TitleSlideTimeSpan);
        }
    }

    // Distance from the title's resting position to fully off-screen, based on the
    // camera's visible width plus the title's own width.
    private float GetTitleOffScreenOffset()
    {
        Camera camera = Camera.main;
        float halfWidth = camera != null ? camera.orthographicSize * camera.aspect : 5.0f;

        float titleWidth = 0.0f;
        SpriteRenderer renderer = categorySprite.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            titleWidth = renderer.bounds.size.x;
        }

        return halfWidth + titleWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeCategory(bool adding)
    {
        if (isChangingCategory)
        {
            return;
        }
        isChangingCategory = true;

        StartCoroutine(SlideOutAndChangeCategory(adding));
    }

    // Flies the current title off-screen, then reloads the scene for the new
    // category. The new scene slides its title in from the opposite side.
    private IEnumerator SlideOutAndChangeCategory(bool adding)
    {
        Vector3 restPosition = categorySprite.transform.localPosition;
        float offset = GetTitleOffScreenOffset();

        // Clicked right (adding): fly out to the left (-X).
        // Clicked left:           fly out to the right (+X).
        float outX = restPosition.x + (adding ? -offset : offset);

        var moveTo = categorySprite.AddComponent<MoveTo>();
        moveTo.Initialize(new Vector2(outX, restPosition.y), TitleSlideTimeSpan);

        yield return new WaitForSeconds(TitleSlideTimeSpan);

        GlobalStorage.CurrentCategory = adding ? GlobalStorage.CurrentCategory + 1 : GlobalStorage.CurrentCategory - 1;
        GlobalStorage.CategorySlideDirection = adding ? 1 : -1;
        SceneManager.LoadScene("SelectCategoryScene");
    }

    void EnterCategory()
    {
        Debug.Log("SelectCategoryScene: categoryId=" + GlobalStorage.CurrentCategory);
        SceneManager.LoadScene("SelectStageScene");
    }
}
