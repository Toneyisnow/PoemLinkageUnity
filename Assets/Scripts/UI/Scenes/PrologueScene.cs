﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueScene : MonoBehaviour
{
    public GameObject enterGameButton = null;

    public GameObject testChar = null;

    private float t = 0;

    private TestMoveAction action = null;

    public Rect area;


    void TestLoadingSpriteSheet()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("characters/fzlb/fzlb_01");
        Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>();

        foreach (Sprite sprite in sprites)
        {
            spritesDict[sprite.name] = sprite;
        }


        testChar.GetComponent<SpriteRenderer>().sprite = spritesDict["c_4e3e"];

    }

    // Start is called before the first frame update
    void Start()
    {
        CommonButton button = enterGameButton.GetComponent<CommonButton>();
        button.SetCallback(() => { this.EnterGame(); });

        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        if(GlobalStorage.LoadGameData() == null)
        {
            // Create a new GameData
            GameData data = new GameData();
            data.RevealCount = 0;
            GlobalStorage.SaveGame(data);
        }

        TestLoadingSpriteSheet();
    }

    // Update is called once per frame
    void Update()
    {
        if (action != null && action.HasFinished)
        {
            GameObject go = action.gameObject;

            Debug.Log("Scene: found the action has finished.");
            Destroy(action);
            action = null;

            Destroy(go);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 5, 2), "Score");

        //bool btn = GUI.Button(new Rect(0, 0, 200, 40), "GUI Button");
        //if (btn)
        {
        //    SceneManager.LoadScene("SelectCategoryScene");
        }
    }

    public void EnterGame()
    {
        GlobalStorage.CurrentCategory = 1;
        SceneManager.LoadScene("SelectCategoryScene");
    }

}
