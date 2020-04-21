using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBoardRenderer : MonoBehaviour
{
    public int Width = 1;

    public int Height = 1;

    public Vector2 CharacterStartAnchor = Vector2.zero;

    public GameObject anchorStart = null;

    public GameObject anchorInterval = null;

    public bool isTint = false;

    public GameObject hintBoardPrefab = null;

    private PoemInstance poemInstance = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(PoemInstance poemInstance)
    { 
        this.poemInstance = poemInstance;

        float anchorX = anchorStart.transform.localPosition.x;
        float anchorY = anchorStart.transform.localPosition.y;

        float interval = this.anchorInterval.transform.localPosition.x - anchorX;

        // Render the characters onto HintBoard
        for (int i = 0; i < poemInstance.Width; i++)
        {
            for (int j = 0; j < poemInstance.Height; j++)
            {
                string charId = poemInstance.GetCharacterIdAt(i, j);

                GameObject go = new GameObject("CharacterSprite");
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
                Texture2D texture = (Texture2D)Resources.Load("characters/fzlb/c_" + charId);
                if (texture != null)
                { 
                    renderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }

    public PoemInstance GetPoem()
    {
        return this.poemInstance;
    }

}
