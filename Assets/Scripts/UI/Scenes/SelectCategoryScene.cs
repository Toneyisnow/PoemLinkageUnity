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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeCategory(bool adding)
    {
        GlobalStorage.CurrentCategory = adding ? GlobalStorage.CurrentCategory + 1 : GlobalStorage.CurrentCategory - 1;
        SceneManager.LoadScene("SelectCategoryScene");
    }

    void EnterCategory()
    {
        Debug.Log("SelectCategoryScene: categoryId=" + GlobalStorage.CurrentCategory);
        SceneManager.LoadScene("SelectStageScene");
    }
}
