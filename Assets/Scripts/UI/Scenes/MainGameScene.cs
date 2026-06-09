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

    // Reveal hints left for the current stage. Each stage grants its own
    // PuzzleDefinition.RevealCount; the count is per-play and not shared.
    private int revealRemaining = 0;

    // Fade duration for the stage music on entry. Leaving uses a shorter fade so
    // the scene transition stays responsive (the load waits for the fade).
    private const float StageMusicFadeTime = 2.0f;
    private const float StageMusicLeaveFadeTime = 1.0f;

    // Guards against starting more than one leave-the-scene fade at a time.
    private bool isLeavingScene = false;

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

        // Lay the three bottom buttons out at the same height, evenly spread so
        // they sit at the 1/4, 2/4 and 3/4 points of the screen width.
        LayoutBottomButtons();

        // The migration lost the scene's sprite reference for this button (it shows
        // up as "Missing"), so assign it at runtime the same way other scenes do.
        var successRenderer = btnSuccess.GetComponent<SpriteRenderer>();
        if (successRenderer != null)
        {
            successRenderer.sprite = Resources.Load<Sprite>(@"images/success");
        }


        //// activityManager = this.GetComponent<ActivityManager>();

        InitializeBoard();

        // Fade the welcome music out over 2 seconds (it crossfades with the
        // stage music, which fades in over the same span).
        if (MyUnitySingleton.Instance != null)
        {
            MyUnitySingleton.Instance.FadeOutBackgroundAudio(2.0f);
        }

        // Play the game audio
        var audio = backgroundAudio.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>(string.Format(@"mp3/bg_{0}", this.StageId));
        audio.loop = true;
        audio.Play();
        backgroundAudio.AddComponent<FadeInVolume>().Initialize(StageMusicFadeTime);

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

        // Each stage grants its own independent number of reveal hints.
        revealRemaining = stageDefinition.PuzzleDefinition.RevealCount;

        // Hide the reveal button in easy mode, or when the stage grants no hints.
        if (!stageDefinition.PuzzleDefinition.IsEasyMode && revealRemaining > 0)
        {
            this.btnReveal.SetActive(true);
            this.txtRevealCount.SetActive(true);
            RefreshRevealButton(revealRemaining);
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
        if (record.HighestScore < score)
        {
            record.HighestScore = score;
        }
        GlobalStorage.SaveRecord(record);

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

        if (this.StageId % 10 < 8 && GlobalStorage.LoadRecord(this.StageId + 2) == null)
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

        // The WinningPoem sprite is rendered at z = -2 with the default sorting order,
        // which would cover the success/back button (it stays clickable but invisible).
        // Force the button to render on top so the player can see it.
        var successRenderer = this.btnSuccess.GetComponent<SpriteRenderer>();
        if (successRenderer != null)
        {
            successRenderer.sortingOrder = 100;
        }

        var showSuccess = new ReceiveCharActivity(this.gameObject, this.btnSuccess);
        showSuccess.SetScales(0.2f, 0.4f);
        activityManager.PushActivity(showSuccess);
    }

    // Positions btnBack, btnReshuffle and btnRestart at one shared height and
    // spreads them across the screen so the gaps to the edges and between buttons
    // are all equal (the buttons land on the quarter lines of the screen width).
    private void LayoutBottomButtons()
    {
        GameObject[] buttons = new GameObject[] { btnBack, btnReshuffle, btnRestart };

        Camera camera = Camera.main;
        float halfWidth = camera != null ? camera.orthographicSize * camera.aspect : 5.0f;
        float quarter = halfWidth / 2.0f;

        // The three buttons share btnBack's current height.
        float commonY = btnBack.transform.localPosition.y;

        float[] xs = new float[] { -quarter, 0.0f, quarter };
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null)
            {
                continue;
            }

            float z = buttons[i].transform.localPosition.z;
            buttons[i].transform.localPosition = new Vector3(xs[i], commonY, z);
        }
    }

    public void BtnBackClicked()
    {
        FadeOutAndLoadScene("SelectStageScene");
    }

    public void BtnRestartClicked()
    {
        // Restart the current stage: reload the scene so the board returns to its
        // initial state (CurrentStage is unchanged in GlobalStorage).
        FadeOutAndLoadScene("MainGameScene");
    }

    public void BtnSuccessClicked()
    {
        FadeOutAndLoadScene("SelectStageScene");
    }

    // Fades the stage music out, then loads the target scene so the audio does
    // not cut off abruptly when leaving the stage.
    private void FadeOutAndLoadScene(string sceneName)
    {
        if (isLeavingScene)
        {
            return;
        }
        isLeavingScene = true;

        StartCoroutine(FadeOutAndLoadSceneCoroutine(sceneName));
    }

    private IEnumerator FadeOutAndLoadSceneCoroutine(string sceneName)
    {
        AudioSource audioSource = backgroundAudio != null ? backgroundAudio.GetComponent<AudioSource>() : null;
        if (audioSource != null && audioSource.isPlaying)
        {
            backgroundAudio.AddComponent<FadeOutVolume>().Initialize(StageMusicLeaveFadeTime);
            yield return new WaitForSeconds(StageMusicLeaveFadeTime);
        }

        SceneManager.LoadScene(sceneName);
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

        if (revealRemaining <= 0)
        {
            return;
        }

        revealUsed++;
        revealRemaining--;

        RefreshRevealButton(revealRemaining);

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

