using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MongoDB.Bson.Serialization;
using UnityEngine.UI;
using Assets.Scripts.UI.Activities;

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
    public GameObject btnReshuffle = null;
    public GameObject btnSuccess = null;

    public GameObject background = null;

    public GameObject revealedChar = null;

    public GameObject txtRevealCount = null;

    public GameObject backgroundAudio = null;

    public GameObject successStar1 = null;
    public GameObject successStar2 = null;
    public GameObject successStar3 = null;

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

    private int revealUsed = 0;
    private int reshuffleUsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        var button = btnBack.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnBackClicked(); });

        button = btnRestart.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnRestartClicked(); });

        button = btnReveal.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnRevealClicked(); });

        button = btnReshuffle.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnReshuffleClicked(); });

        button = btnSuccess.GetComponent<CommonButton>();
        button.SetCallback(() => { this.BtnSuccessClicked(); });


        //// activityManager = this.GetComponent<ActivityManager>();

        InitializeBoard();

        // Stop the main audio
        MyUnitySingleton.Instance.gameObject.GetComponent<AudioSource>().Pause();

        // Play the game audio
        var audio = backgroundAudio.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>(string.Format(@"mp3/bg_{0}", this.StageId));
        audio.loop = true;
        audio.Play();
        backgroundAudio.AddComponent<FadeInVolume>();

        revealUsed = 0;
        reshuffleUsed = 0;
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

        game.transform.parent = this.transform;
        game.name = "ActivityManager";
        // game.tag = "ActivityManager";
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

        var clr = revealedChar.GetComponent<SpriteRenderer>().color;
        revealedChar.GetComponent<SpriteRenderer>().color = new Color(clr.r, clr.g, clr.b, 0.0f);
        ShowRevealedCharActivity showRevealed = new ShowRevealedCharActivity(revealedChar, 1.0f);
        e.ActivityManager.PushActivity(showRevealed);

        if (poem.GetCoveredCharIds().Contains(e.CharacterId))
        {
            int charIndex = poem.GetFirstCoveredIndex(e.CharacterId);
            if (charIndex < 0)
            {
                return;
            }

            var hintBoardRenderer = this.HintBoard.GetComponent<HintBoardRenderer>();
            hintBoardRenderer.ReceiveCharacter(e.CharacterId, e.ActivityManager);
        }

        if (poem.IsAllCharactersUncovered())
        {
            // Success
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
        if (revealUsed == 0 && reshuffleUsed == 0)
        {
            return 3;
        }
        if (revealUsed <= 1 && reshuffleUsed <= 1)
        {
            return 2;
        }

        return 1;
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

        int nextStageId = 0;
        if (this.StageId == 109)
        {
            nextStageId = 201;
        }
        else if (this.StageId == 209)
        {
            nextStageId = 301;
        }
        else
        {
            nextStageId = this.StageId + 1;
        }

        if (nextStageId > 0 && GlobalStorage.LoadRecord(nextStageId) == null)
        {
            StageRecord next = new StageRecord();
            next.StageId = nextStageId;
            next.HighestScore = 0;
            GlobalStorage.SaveRecord(next);
        }

        if (this.StageId % 10 < 8)
        {
            StageRecord next = new StageRecord();
            next.StageId = this.StageId + 2;
            next.HighestScore = 0;
            GlobalStorage.SaveRecord(next);
        }

        // Play animation to close the panels and show poem in full
        var activityManager = AquireActivityManager();
        var fadeOut1 = new FadeOutActivity(this.PuzzleBoard, 1.5f);
        var fadeOut2 = new FadeOutActivity(this.HintBoard, 1.5f);
        var fadeOut3 = new FadeOutActivity(this.btnReveal, 1.5f);
        var fadeOut4 = new FadeOutActivity(this.btnBack, 1.5f);
        var fadeOut5 = new FadeOutActivity(this.btnReshuffle, 1.5f);
        ////var fadeOut6 = new FadeOutActivity(this.txtRevealCount, 1.5f);
        
        this.txtRevealCount.SetActive(false);
        this.btnRestart.SetActive(false);

        var bundle = new BundleActivity();
        bundle.AddActivity(fadeOut1);
        bundle.AddActivity(fadeOut2);
        bundle.AddActivity(fadeOut3);
        bundle.AddActivity(fadeOut4);
        bundle.AddActivity(fadeOut5);
        ////bundle.AddActivity(fadeOut6);

        var gameObject = new GameObject("WinningPoem");
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(string.Format(@"images/stage_{0}_win", this.StageId));
        renderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 1.0f);
        gameObject.transform.position = new Vector3(0f, 0f, -2.0f);
        activityManager.PushActivity(bundle);

        var fadeIn = new FadeInActivity(gameObject, 1.5f);
        activityManager.PushActivity(fadeIn);

        var receiveStar1 = new ReceiveCharActivity(this.gameObject, this.successStar1);
        receiveStar1.SetScales(3.0f, 6.0f);
        activityManager.PushActivity(receiveStar1);
        
        if (score > 1)
        {
            var receiveStar2 = new ReceiveCharActivity(this.gameObject, this.successStar2);
            receiveStar2.SetScales(3.0f, 6.0f);
            activityManager.PushActivity(receiveStar2);
        }

        if (score > 2)
        {
            var receiveStar3 = new ReceiveCharActivity(this.gameObject, this.successStar3);
            receiveStar3.SetScales(3.0f, 6.0f);
            activityManager.PushActivity(receiveStar3);
        }

        var showSuccess = new ReceiveCharActivity(this.gameObject, this.btnSuccess);
        showSuccess.SetScales(0.2f, 0.4f);
        activityManager.PushActivity(showSuccess);
    }

    public void BtnBackClicked()
    {
        SceneManager.LoadScene("SelectStageScene");
    }

    public void BtnRestartClicked()
    {
        OnGameWin();
    }

    public void BtnSuccessClicked()
    {
        SceneManager.LoadScene("SelectStageScene");
    }

    public void BtnReshuffleClicked()
    {
        Debug.Log("BtnReshuffleClicked");

        reshuffleUsed++;
        this.PuzzleBoard.GetComponent<PuzzleBoardRenderer>().CheckAndMakeShuffle(true);
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

        revealUsed++;
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

