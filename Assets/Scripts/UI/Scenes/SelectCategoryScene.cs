using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCategoryScene : MonoBehaviour
{
    public GameObject categorySprite = null;

    public GameObject leftArror = null;

    public GameObject rightArror = null;

    // Start is called before the first frame update
    void Start()
    {
        int categoryId = GlobalStorage.CurrentCategory;

        string categoryTitleImge = string.Format(@"images/category-title-{0}", categoryId);
        categorySprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(categoryTitleImge);

        CommonButton button = categorySprite.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterCategory(); });

        leftArror.SetActive(categoryId > 1);
        rightArror.SetActive(categoryId < 3);
        
        leftArror.GetComponent<CommonButton>().SetCallback(() => { this.ChangeCategory(false); });
        rightArror.GetComponent<CommonButton>().SetCallback(() => { this.ChangeCategory(true); });
        
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
