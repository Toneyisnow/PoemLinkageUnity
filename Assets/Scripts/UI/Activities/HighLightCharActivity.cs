using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCharActivity : BaseActivity
{
    private static Vector3 originScale = new Vector3(1.0f, 1.0f, 1.0f);

    private static Vector3 maxScale = new Vector3(1.6f, 1.6f, 1.0f);

    private GameObject rootNode;

    private List<PuzzleCharacter> characters;

    private List<GameObject> characterNodes;

    private float t;

   
    public HighlightCharActivity(GameObject rootNode, List<PuzzleCharacter> characters)
    {
        this.rootNode = rootNode;
        this.characters = characters;
        this.characterNodes = null;
    }
    public HighlightCharActivity(GameObject rootNode, List<GameObject> characters)
    {
        this.rootNode = rootNode;
        this.characterNodes = characters;
    }

    public override bool HasFinished()
    {
        if (characterNodes == null || characterNodes.Count == 0)
        {
            return true;
        }

        foreach (var charNode in characterNodes)
        {
            if (charNode.transform.localScale == maxScale)
            {
                return true;
            };
        }

        return false;
    }

    public override void OnBeginning()
    {
        t = 0;

        var puzzleBoardRenderer = rootNode.GetComponent<PuzzleBoardRenderer>();

        if (characterNodes == null)
        {

            characterNodes = new List<GameObject>();
            foreach (PuzzleCharacter character in characters)
            {
                var charNode = puzzleBoardRenderer.FindCharacterNode(character);
                if (charNode != null)
                {
                    characterNodes.Add(charNode);
                }
            }
        }
    }

    public override void OnFinished()
    {
        // Do nothing here
    }

    // Update is called once per frame
    public override void Update()
    {
        t += Time.deltaTime * 1.2f;
        
        foreach (var charNode in characterNodes)
        {
            charNode.transform.localScale = Vector3.Lerp(originScale, maxScale, Mathf.SmoothStep(0.0f, 1.0f, t));
        }

    }
}
