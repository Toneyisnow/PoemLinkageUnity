using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueScene : MonoBehaviour
{
    public GameObject enterGameButton = null;


    // Start is called before the first frame update
    void Start()
    {
        CommonButton button = enterGameButton.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterGame(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        bool btn = GUI.Button(new Rect(0, 0, 200, 40), "GUI Button");
        if (btn)
        {
            SceneManager.LoadScene("SelectCategoryScene");
        }
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("SelectCategoryScene");
    }

}
