using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCategoryScene : MonoBehaviour
{
    public GameObject categorySprite = null;

    // Start is called before the first frame update
    void Start()
    {
        CommonButton button = categorySprite.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterCategory(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterCategory()
    {
        SceneManager.LoadScene("SelectStageScene");

    }
}
