using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivedCharEventArgs : EventArgs
{
    public string CharacterId
    {
        get; private set;
    }

    public ReceivedCharEventArgs(string characterId)
    {
        this.CharacterId = characterId;
    }
}


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

    public float ScaleFactor = 1.0f;

    private PoemInstance poemInstance = null;

    private GameObject BoardRootNode = null;


    public event EventHandler<ReceivedCharEventArgs> onReceivedCharacter = null;


    public PuzzleDefinition PuzzleDefinition
    {
        get; private set;
    }

    public StageDefinition StageDefinition
    {
        get; private set;
    }

    private PuzzleBoard puzzleBoard = null;

    private ActivityManager activityManager = null;

    private float anchorInterval = 0;

    private Vector3 startPosition;

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
        foreach (PuzzleCharacter character in puzzleBoard.PuzzleCharacters)
        {
            RenderPuzzleNode(character);
        }

        this.activityManager = gameObject.GetComponentInParent<ActivityManager>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    private void RenderPuzzleNode(PuzzleCharacter character)
    {
        anchorInterval = intervalAnchor.transform.localPosition.x - startAnchor.transform.localPosition.x;
        startPosition = new Vector3(startAnchor.transform.localPosition.x - anchorInterval,
                                            startAnchor.transform.localPosition.y + anchorInterval,
                                            -1);

        GameObject nodeObject = new GameObject("Character_" + character.Index.ToString());
        nodeObject.transform.parent = this.transform;

        nodeObject.transform.localPosition = this.ConvertToPixelPosition(character.Position);
        nodeObject.transform.localScale = new Vector3(1.0f, 1.0f, 1);

        var sprite = GlobalStorage.GetSpriteFromDictionary(character.CharacterId);
        //// var sprite = Resources.Load<Sprite>("characters/fzlb/c_" + character.CharacterId);
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
        if (characterNode == null)
        {
            Debug.LogWarning("characterNode is null.");
        }

        var firstCharacterNode = this.FindCharacterNode(firstCharacter);
        if (firstCharacterNode == null)
        {
            Debug.LogWarning("firstCharacterNode is null.");
        }

        PlayAnimationMergeChars(characterNode, firstCharacterNode, connectionPoints);

        FormulaDefinition formula = this.StageDefinition.FindFormula(character.CharacterId, firstCharacter.CharacterId);
        if(formula == null)
        {
            return;
        }
        
        if (this.poemInstance.GetCoveredCharIds().Contains(formula.Target))
        {
            if (this.onReceivedCharacter != null)
            {
                this.activityManager.PushCallback(() =>
                    { this.onReceivedCharacter(this, new ReceivedCharEventArgs(formula.Target)); });
            }
        }

        this.activityManager.PushCallback(() => { CheckAndMakeShuffle(); });
    }

    public void CheckAndMakeShuffle()
    {
        Debug.Log("Start CheckAndMakeShuffle.");
    }

    private GameObject CreateLinePrefab(Vector2Int posA, Vector2Int posB)
    {
        GameObject result = null;

        var startX = Math.Min(posA.x, posB.x);
        var startY = Math.Max(posA.y, posB.y);

        if (posA.x == posB.x)
        {
            result = GameObject.Instantiate(this.ConnectLineVertical);
            result.transform.parent = this.transform;

            result.transform.localPosition = this.ConvertToPixelPosition(new Vector2Int(startX, startY));
            result.transform.localScale = new Vector3(3.0f, Math.Abs(posA.y - posB.y) * 2, 1.0f);
        }
        else if (posA.y == posB.y)
        {
            result = GameObject.Instantiate(this.ConnectLineHorizon);
            result.transform.parent = this.transform;

            result.transform.localPosition = this.ConvertToPixelPosition(new Vector2Int(startX, startY));
            result.transform.localScale = new Vector3(Math.Abs(posA.x - posB.x) * 2, 3.0f, 1.0f);
        }
        else
        {
            return null;
        }

        
        return result;
    }

    private Vector3 ConvertToPixelPosition(Vector2Int position)
    {
        var pixelX = this.startPosition.x + position.x * anchorInterval;
        var pixelY = this.startPosition.y - position.y * anchorInterval;

        return new Vector3(pixelX, pixelY, -1);
    }

    private void PlayAnimationMergeChars(GameObject charNodeA, GameObject charNodeB, List<Vector2Int> connectionPoints)
    {
        List<GameObject> lineObjects = new List<GameObject>();
        for(int i = 1; i< connectionPoints.Count; i++)
        {
            var point = connectionPoints[i];
            var lastPoint = connectionPoints[i - 1];

            var lineObject = this.CreateLinePrefab(lastPoint, point);
            lineObjects.Add(lineObject);
        }

        List<GameObject> chars = new List<GameObject>() { charNodeA, charNodeB };
        HighlightCharActivity highLight = new HighlightCharActivity(this.gameObject, chars);
        this.activityManager.PushActivity(highLight);

        List<GameObject> allObjects = new List<GameObject>();
        allObjects.AddRange(lineObjects);
        allObjects.AddRange(chars);

        DestroyObjectActivity destroy = new DestroyObjectActivity(allObjects);
        this.activityManager.PushActivity(destroy);
    }

    public GameObject FindCharacterNode(PuzzleCharacter character)
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
