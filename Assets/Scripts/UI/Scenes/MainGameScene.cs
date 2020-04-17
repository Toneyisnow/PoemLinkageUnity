using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
    public GameObject hintAnchor = null;
    public GameObject hint25Prefab = null;
    public GameObject hint27Prefab = null;
    public GameObject hint45Prefab = null;
    public GameObject hint47Prefab = null;

    public GameObject puzzleAnchor = null;
    public GameObject puzzleTinyPrefab = null;
    public GameObject puzzleSmallPrefab = null;
    public GameObject puzzleMediumPrefab = null;
    public GameObject puzzleLargePrefab = null;
    public GameObject puzzleHugePrefab = null;


    public int StageId
    {
        get; private set;
    }

    public GameObject HintBoard
    {
        get; private set;
    }

    public GameObject PuzzleBoard
    {
        get; private set;
    }


    // Start is called before the first frame update
    void Start()
    {
        StageDefinition stageDefinition = LoadCurrentStage();
        PoemInstance poem = new PoemInstance(stageDefinition.PoemDefinition, stageDefinition.PuzzleDefinition.SelectedLines,
            stageDefinition.PuzzleDefinition.UncoveredCharIndexes);

        // Read the HintBoard and PuzzleBoard
        this.HintBoard = GameObject.Instantiate(this.hint25Prefab);
        this.HintBoard.transform.parent = this.hintAnchor.transform;
        this.HintBoard.transform.localPosition = new Vector3(0, 0, -1);
        this.HintBoard.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        var hintBoardRenderer = this.HintBoard.GetComponent<HintBoardRenderer>();
        hintBoardRenderer.Initialize(poem);

        switch(stageDefinition.PuzzleDefinition.BoardSize)
        {
            case PuzzleBoardSize.TINY:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleTinyPrefab);
                break;
            case PuzzleBoardSize.SMALL:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleSmallPrefab);
                break;
            case PuzzleBoardSize.MEDIUM:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleMediumPrefab);
                break;
            case PuzzleBoardSize.LARGE:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleLargePrefab);
                break;
            case PuzzleBoardSize.HUGE:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleHugePrefab);
                break;
            default:
                this.PuzzleBoard = GameObject.Instantiate(this.puzzleMediumPrefab);
                break;
        }

        this.PuzzleBoard.transform.parent = this.puzzleAnchor.transform;
        this.PuzzleBoard.transform.localPosition = new Vector3(0, 0, -1);
        this.PuzzleBoard.transform.localScale = new Vector3(0.43f, 0.43f, 1);

        var puzzleBoardRenderer = this.PuzzleBoard.GetComponent<PuzzleBoardRenderer>();
        puzzleBoardRenderer.Initialize(stageDefinition, poem);
    }


    public void Initialize()
    {
        
    }

    private StageDefinition LoadCurrentStage()
    {
        this.StageId = GlobalStorage.CurrentStage;

        string defFileName = string.Format(@"stages/stage_{0}", this.StageId);
        
        Object obj = Resources.Load(defFileName);

        TextAsset textFile = Resources.Load(defFileName) as TextAsset;
        if (textFile == null)
        {
            return null;
        }

        Debug.Log("Def file content: " + textFile.text);

        StageDefinition stageDefinition = JsonConvert.DeserializeObject<StageDefinition>(textFile.text);
        return stageDefinition;
    }
}
