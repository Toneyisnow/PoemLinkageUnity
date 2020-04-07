using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintBoardRenderer : MonoBehaviour
{
    public int Width = 1;

    public int Height = 1;

    public Vector2 CharacterStartAnchor = Vector2.zero;

    public int AnchorInternal = 0;

    public bool isTint = false;

    public GameObject hintBoardPrefab = null;

    private PoemInstance poemInstance = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(StageDefinition stageDefinition)
    { 
        this.poemInstance = new PoemInstance(stageDefinition.PoemDefinition, 
            stageDefinition.PuzzleDefinition.UncoveredCharIndexes);

    }

    public PoemInstance GetPoem()
    {
        return this.poemInstance;
    }

}
