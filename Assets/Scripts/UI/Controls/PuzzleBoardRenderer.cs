using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoardRenderer : MonoBehaviour
{
    public int Width = 1;

    public int Height = 1;

    // public Vector2 CharacterStartAnchor = Vector2.zero;

    public GameObject startAnchor = null;

    public int AnchorInternal = 0;

    public GameObject PuzzleBoardPrefab = null;

    public GameObject ConnectLineHorizon = null;

    public GameObject ConnectLineVertical = null;

    private PoemInstance poemInstance = null;

    private GameObject BoardRootNode = null;

    public PuzzleDefinition PuzzleDefinition
    {
        get; private set;
    }

    public StageDefinition StageDefinition
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(StageDefinition stageDefinition, PoemInstance poemInstance)
    {
        this.StageDefinition = stageDefinition;
        this.poemInstance = poemInstance;




    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    private void RenderPuzzleNodeToBoard(PuzzleCharacter character)
    {

    }

    public GameObject GetNodeAtPosition(Vector2 position)
    {
        return null;
    }

    public GameObject GetNodeByIndex(int index)
    {
        return null;
    }

    public GameObject GetNodeByCharacterId(string charId)
    {
        return null;
    }

    public void OnBoardClickedAt(object evt, object customerData)
    {

    }

    /////////// Callback from Provider /////////
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    public void OnChooseCharacterAt(Vector2 position)
    {

    }

    /// <summary>
    /// The two characters not match, or use click the firstPosition again. Cancel them.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="firstPosition"></param>
    public void OnChooseCharacterAt(Vector2 position, Vector2 firstPosition)
    {

    }

    public void OnMatchNotConnected(Vector2 position, Vector2 firstPosition)
    {

    }

    public void OnConnected(Vector2 position, Vector2 firstPosition)
    {

    }

    public void CheckAndMakeShuffle()
    {

    }

    private void CreateLinePrefab(Vector2 posA, Vector2 posB)
    {

    }

    private void PlayAnimationMergeChars(GameObject charNodeA, GameObject charNodeB, List<GameObject> lineNodes, Action followUpAction)
    {

    }
}
