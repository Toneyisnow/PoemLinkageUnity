﻿using System.Collections;
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
        this.StageId = GlobalStorage.CurrentStage;

        // Read the HintBoard and PuzzleBoard
        this.HintBoard = GameObject.Instantiate(this.hint25Prefab);
        this.HintBoard.transform.parent = this.hintAnchor.transform;
        this.HintBoard.transform.localPosition = new Vector3(0, 0, 0);
        this.HintBoard.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        this.PuzzleBoard = GameObject.Instantiate(this.puzzleMediumPrefab);
        this.PuzzleBoard.transform.parent = this.puzzleAnchor.transform;
        this.PuzzleBoard.transform.localPosition = new Vector3(0, 0, 0);
        this.PuzzleBoard.transform.localScale = new Vector3(0.5f, 0.5f, 1);



    }


    public void Initialize()
    {
        
    }

}
