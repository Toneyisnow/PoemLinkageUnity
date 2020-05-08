using Assets.Scripts.UI.Activities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBoardRenderer : MonoBehaviour
{
    public int Width = 1;

    public int Height = 1;

    public GameObject anchorStart = null;

    public GameObject anchorInterval = null;

    public float scaleFactor = 1.0f;

    public bool isTint = false;

   
    public GameObject hintBoardPrefab = null;

    private PoemInstance poemInstance = null;

    private bool isTotalBlindMode = false;

    private ActivityManager activityManager = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(PoemInstance poemInstance, bool isBlind)
    { 
        this.poemInstance = poemInstance;
        this.isTotalBlindMode = isBlind;

        this.activityManager = gameObject.GetComponentInParent<ActivityManager>();
        this.Width = poemInstance.Width;
        this.Height = poemInstance.Height;

        float anchorX = anchorStart.transform.localPosition.x;
        float anchorY = anchorStart.transform.localPosition.y;

        float interval = this.anchorInterval.transform.localPosition.x - anchorX;

        // Render the characters onto HintBoard
        for (int i = 0; i < this.Width; i++)
        {
            for (int j = 0; j < this.Height; j++)
            {
                string charId = poemInstance.GetCharacterIdAt(i, j);

                GameObject go = new GameObject("Character_" + (j * Width + i).ToString());
                go.transform.parent = this.transform;

                float posX = anchorX + i * interval;
                float posY = anchorY - j * interval;

                if(this.isTint)
                {
                    posX += j * interval;
                }

                go.transform.localPosition = new Vector3(posX, posY, -1);
                go.transform.localScale = new Vector3(1.0f, 1.0f, 1);

                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                var sprite = GlobalStorage.GetSpriteFromDictionary(charId);
                //// Texture2D texture = (Texture2D)Resources.Load("characters/fzlb/c_" + charId);
                if (sprite != null)
                { 
                    renderer.sprite = sprite;
                }

                if(!poemInstance.IsUncoveredAt(i + j * Width))
                {
                    Color theColorToAdjust = renderer.material.color;
                    theColorToAdjust.a = this.isTotalBlindMode ? 0f : 0.2f;
                    renderer.material.color = theColorToAdjust;
                }


            }
        }
    }

    public PoemInstance GetPoem()
    {
        return this.poemInstance;
    }

    public void ReceiveCharacter(string characterId)
    {
        Debug.Log("HintBoardRenderer.ReceiveCharacter: " + characterId);

        int charIndex = poemInstance.GetFirstCoveredIndex(characterId);
        if(charIndex < 0)
        {
            return;
        }

        Transform childTransform = this.gameObject.transform.Find("Character_" + charIndex.ToString());
        if (childTransform == null)
        {
            return;
        }

        ReceiveCharActivity activity = new ReceiveCharActivity(gameObject, childTransform.gameObject);
        this.activityManager.PushActivity(activity);

    }

    public void RevealCoveredChars()
    {
        Debug.Log("HintBoardRenderer: Start RevealCoveredChars.");

        HashSet<int> coveredChars = this.poemInstance.GetCoveredChars();
        foreach (int charIndex in coveredChars)
        {
            Transform childTransform = this.gameObject.transform.Find("Character_" + charIndex.ToString());
            if (childTransform == null)
            {
                continue;
            }

            if (childTransform.gameObject.GetComponent<Revealing>() == null)
            {
                childTransform.gameObject.AddComponent<Revealing>();
            }
        }
    }

    public bool IsBusy()
    {
        var revealingActivities = this.gameObject.GetComponentsInChildren<Revealing>();
        return revealingActivities != null && revealingActivities.Length > 0;
    }
}
