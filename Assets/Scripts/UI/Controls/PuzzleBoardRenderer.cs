using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoardRenderer : MonoBehaviour, PuzzleBoardHandler
{
    public int Width = 1;

    public int Height = 1;

    // public Vector2 CharacterStartAnchor = Vector2.zero;

    public GameObject startAnchor = null;

    public GameObject intervalAnchor = null;

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

    private PuzzleBoard puzzleBoard = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(StageDefinition stageDefinition, PoemInstance poemInstance)
    {
        this.StageDefinition = stageDefinition;
        this.poemInstance = poemInstance;

        this.puzzleBoard = new PuzzleBoard(this, stageDefinition, poemInstance);

        // Render the board
        foreach(PuzzleCharacter character in puzzleBoard.PuzzleCharacters)
        {
            RenderPuzzleNode(character);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    private void RenderPuzzleNode(PuzzleCharacter character)
    {
        float interval = intervalAnchor.transform.localPosition.x - startAnchor.transform.localPosition.x;
        Vector3 startPosition = new Vector3(startAnchor.transform.localPosition.x - interval,
                                            startAnchor.transform.localPosition.y + interval,
                                            -1);

        GameObject nodeObject = new GameObject("Character_" + character.Index.ToString());
        nodeObject.transform.parent = this.transform;

        float posX = startPosition.x + interval * character.Position.x;
        float posY = startPosition.y - interval * character.Position.y;
        nodeObject.transform.localPosition = new Vector3(posX, posY, -1);
        nodeObject.transform.localScale = new Vector3(1.0f, 1.0f, 1);

        var sprite = Resources.Load<Sprite>("characters/fzlb/c_" + character.CharacterId);
        if (sprite != null)
        {
            var renderer = nodeObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }

        var nodeRenderer = nodeObject.AddComponent<PuzzleNodeRenderer>();
        nodeRenderer.Initialize(character, (chara) => { this.OnBoardClickedAt(character); });
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

    public void OnBoardClickedAt(PuzzleCharacter character)
    {
        Debug.Log("OnBoardClickedAt: " + character.CharacterId);
        this.puzzleBoard.TakeAction(character);
    }

    /////////// Callback from Provider /////////
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    public void OnChooseCharacter(PuzzleCharacter character)
    {
        Debug.Log("OnChooseCharacter: " + character.Index);

        var characterNode = this.FindCharacterNode(character);
        if(characterNode == null)
        {
            return;
        }

        characterNode.transform.localScale = new Vector3(1.4f, 1.4f, 0);
    }

    /// <summary>
    /// The two characters not match, or use click the firstPosition again. Cancel them.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="firstPosition"></param>
    public void OnUnchooseCharacter(PuzzleCharacter character, PuzzleCharacter firstCharacter)
    {
        Debug.Log("OnUnchooseCharacter: " + character.Position);
        var characterNode = this.FindCharacterNode(character);
        if (characterNode == null)
        {
            return;
        }

        characterNode.transform.localScale = new Vector3(1f, 1f, 0);

        characterNode = this.FindCharacterNode(firstCharacter);
        if (characterNode == null)
        {
            return;
        }

        characterNode.transform.localScale = new Vector3(1f, 1f, 0);
    }

    public void OnMatchNotConnected(PuzzleCharacter character, PuzzleCharacter firstCharacter)
    {
        Debug.Log("OnMatchNotConnected: " + character.Position);
        OnUnchooseCharacter(character, firstCharacter);
    }

    public void OnConnected(PuzzleCharacter character, PuzzleCharacter firstCharacter, List<Vector2Int> connectionPoints, string targetCharId)
    {
        Debug.Log("OnConnected: " + character.Position);
        var characterNode = this.FindCharacterNode(character);
        if (characterNode != null)
        {
            Destroy(characterNode);
        }

        characterNode = this.FindCharacterNode(firstCharacter);
        if (characterNode != null)
        {
            Destroy(characterNode);
        }

        CheckAndMakeShuffle();
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

    private GameObject FindCharacterNode(PuzzleCharacter character)
    {
        if (character == null)
        {
            return null;
        }

        var childTransform = this.gameObject.transform.Find("Character_" + character.Index.ToString());
        if (childTransform != null)
        {
            return childTransform.gameObject;
        }

        return null;
    }
}
