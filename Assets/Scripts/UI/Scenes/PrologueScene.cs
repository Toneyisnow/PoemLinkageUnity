using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueScene : MonoBehaviour
{
    public GameObject enterGameButton = null;

    public GameObject testSprite = null;

    private float t = 0;

    private TestMoveAction action = null;


    // Start is called before the first frame update
    void Start()
    {
        CommonButton button = enterGameButton.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterGame(); });

        action = testSprite.AddComponent<TestMoveAction>();
        action.Initialize(new Vector3(2.0f, 0, -1), true);

        var record = GlobalStorage.LoadRecord(105);

        // testSprite.transform.position = Vector3.Lerp(new Vector3(-1, 0, -1), new Vector3(1, 0, -1), Mathf.SmoothStep(0.0f, 1.0f, Time.deltaTime * 1.4f));
    }

    // Update is called once per frame
    void Update()
    {
        // t += Time.deltaTime * 1.4f;
        // testSprite.transform.position = Vector3.Lerp(new Vector3(-1, 0, -1), new Vector3(1, 0, -1), Mathf.SmoothStep(0.0f, 1.0f, t));
    
        if (action != null && action.HasFinished)
        {
            GameObject go = action.gameObject;

            Debug.Log("Scene: found the action has finished.");
            Destroy(action);
            action = null;

            Destroy(go);
        }
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
        GlobalStorage.CurrentCategory = 1;
        SceneManager.LoadScene("SelectCategoryScene");
    }

}
