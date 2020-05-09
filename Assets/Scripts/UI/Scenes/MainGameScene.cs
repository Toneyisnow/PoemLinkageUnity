using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Bson.Serialization;
using UnityEngine.UI;

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

    public GameObject btnReveal = null;
    public GameObject btnBack = null;
    public GameObject btnRestart = null;
    public GameObject btnWin = null;

    public GameObject background = null;

    public GameObject revealedChar = null;

    public GameObject txtRevealCount = null;

    public List<GameObject> backgroundAudios = null;

    //// private ActivityManager activityManager = null;

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

    private PoemInstance poem = null;

    // Start is called before the first frame update
    void Start()
    {
        // Stop the main audio
        MyUnitySingleton.Instance.gameObject.GetComponent<AudioSource>().Pause();

        // Play the game audio
        backgroundAudios[0].GetComponent<AudioSource>().Play();

        GlobalStorage.LoadSpriteDictionary();

        var button = btnBack.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnBackClicked(); });

        button = btnRestart.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnRestartClicked(); });

        button = btnWin.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnWinClicked(); });

        button = btnReveal.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnRevealClicked(); });

        //// activityManager = this.GetComponent<ActivityManager>();

        InitializeBoard();
    }

    public void InitializeBoard()
    {
        StageDefinition stageDefinition = LoadCurrentStage();
        poem = new PoemInstance(stageDefinition.PoemDefinition, stageDefinition.PuzzleDefinition.SelectedLines,
            stageDefinition.PuzzleDefinition.UncoveredCharIndexes);

        string backgroundImage = string.Format(@"images/stage_{0}_full", this.StageId);
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(backgroundImage);
        //// background.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        // Read the HintBoard and PuzzleBoard
        if (poem.Height == 2 && poem.Width == 5)
        {
            this.HintBoard = GameObject.Instantiate(this.hint25Prefab);
        }
        if (poem.Height == 2 && poem.Width == 7)
        {
            this.HintBoard = GameObject.Instantiate(this.hint27Prefab);
        }
        if (poem.Height == 4 && poem.Width == 5)
        {
            this.HintBoard = GameObject.Instantiate(this.hint45Prefab);
        }
        if (poem.Height == 4 && poem.Width == 7)
        {
            this.HintBoard = GameObject.Instantiate(this.hint47Prefab);
        }

        this.HintBoard.transform.parent = this.hintAnchor.transform;
        this.HintBoard.transform.localPosition = new Vector3(0, 0, -1);
        
        var hintBoardRenderer = this.HintBoard.GetComponent<HintBoardRenderer>();
        hintBoardRenderer.Initialize(poem, !stageDefinition.PuzzleDefinition.IsEasyMode);
        this.HintBoard.transform.localScale = new Vector3(hintBoardRenderer.scaleFactor, hintBoardRenderer.scaleFactor, 1);

        switch (stageDefinition.PuzzleDefinition.BoardSize)
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
        
        var puzzleBoardRenderer = this.PuzzleBoard.GetComponent<PuzzleBoardRenderer>();
        puzzleBoardRenderer.Initialize(stageDefinition, poem);
        this.PuzzleBoard.transform.localScale = new Vector3(puzzleBoardRenderer.ScaleFactor, puzzleBoardRenderer.ScaleFactor, 1);
        
        puzzleBoardRenderer.onReceivedCharacter += PuzzleBoardRenderer_onReceivedCharacter;

        if (!stageDefinition.PuzzleDefinition.IsEasyMode)
        {
            var gameData = GlobalStorage.LoadGameData();

            this.btnReveal.SetActive(true);
            this.txtRevealCount.SetActive(true);
            RefreshRevealButton(gameData?.RevealCount ?? 0);
        }
        else
        {
            this.btnReveal.SetActive(false);
            this.txtRevealCount.SetActive(false);
        }

    }

    public ActivityManager AquireActivityManager()
    {
        GameObject game = new GameObject();
        var activityManager = game.AddComponent<ActivityManager>();
        activityManager.Initialize();

        return activityManager;
    }

    private void PuzzleBoardRenderer_onReceivedCharacter(object sender, ReceivedCharEventArgs e)
    {
        Debug.Log("PuzzleBoardRenderer_onReceivedCharacter");

        var renderer = revealedChar.GetComponent<SpriteRenderer>();
        var sprite = GlobalStorage.GetSpriteFromDictionary(e.CharacterId);
        if (sprite != null)
        {
            renderer.sprite = sprite;
        }

        ShowRevealedCharActivity showRevealed = new ShowRevealedCharActivity(revealedChar, 1.0f);
        e.ActivityManager.PushActivity(showRevealed);

        var hintBoardRenderer = this.HintBoard.GetComponent<HintBoardRenderer>();
        hintBoardRenderer.ReceiveCharacter(e.CharacterId, e.ActivityManager);

        int charIndex = poem.GetFirstCoveredIndex(e.CharacterId);
        if(charIndex < 0)
        {
            return;
        }

        poem.SetUncoveredAt(charIndex);

        if(poem.IsAllCharactersUncovered())
        {
            // Success
            e.ActivityManager.ClearAll();
            e.ActivityManager.PushCallback(() => { this.OnGameWin(); });
        }
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

        //// StageDefinition stageDefinition = BsonSerializer.Deserialize<StageDefinition>(textFile.text);
        return stageDefinition;
    }

    private int CalculateScore()
    {
        return 3;
    }

    public void OnGameWin()
    {
        Debug.Log("OnGameWin");

        // Save record
        StageRecord record = GlobalStorage.LoadRecord(this.StageId);
        if (record == null)
        {
            record = new StageRecord();
            record.StageId = this.StageId;
            record.HighestScore = 0;
        }

        record.JustCompleted = true;

        int score = CalculateScore();
        int gainRevealCount = 0;
        if (record.HighestScore < score)
        {
            gainRevealCount = score - record.HighestScore;
            record.HighestScore = score;
        }
        GlobalStorage.SaveRecord(record);

        if (gainRevealCount > 0)
        {
            var gameData = GlobalStorage.LoadGameData();
            gameData.RevealCount += gainRevealCount;
            GlobalStorage.SaveGame(gameData);
        }
        
        if (this.StageId % 10 < 9)
        {
            int nextStageId = this.StageId + 1;

            if (GlobalStorage.LoadRecord(nextStageId) == null)
            {
                StageRecord next = new StageRecord();
                next.StageId = nextStageId;
                next.HighestScore = 0;
                GlobalStorage.SaveRecord(next);
            }
        }


    }

    public void BtnBackClicked()
    {
        SceneManager.LoadScene("SelectStageScene");
    }

    public void BtnRestartClicked()
    {
        StageRecord record = new StageRecord();
        record.StageId = this.StageId;
        record.HighestScore = 3;
        record.JustCompleted = true;
        GlobalStorage.SaveRecord(record);

        if(this.StageId % 10 < 9)
        {
            int nextStageId = this.StageId + 1;

            if (GlobalStorage.LoadRecord(nextStageId) == null)
            {
                StageRecord next = new StageRecord();
                next.StageId = nextStageId;
                next.HighestScore = 0;
                GlobalStorage.SaveRecord(next);
            }
        }
    }

    public void BtnWinClicked()
    {
    }

    public void BtnRevealClicked()
    {
        Debug.Log("BtnRevealClicked");

        if (this.HintBoard.GetComponent<HintBoardRenderer>().IsBusy())
        {
            // Do not do anything if the revealing is still on going
            return;
        }

        var gameData = GlobalStorage.LoadGameData();
        if (gameData.RevealCount <= 0)
        {
            return;
        }

        gameData.RevealCount--;
        GlobalStorage.SaveGame(gameData);

        RefreshRevealButton(gameData.RevealCount);

        var hintBoardRenderer = this.HintBoard.GetComponent<HintBoardRenderer>();
        hintBoardRenderer.RevealCoveredChars();
    }

    private void RefreshRevealButton(int count)
    {
        this.txtRevealCount.GetComponent<Text>().text = count.ToString();

        if (count <= 0)
        {
            this.btnReveal.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        }
        else
        {
            this.btnReveal.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }
}

