using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleNodeRenderer : MonoBehaviour
{
    public PuzzleCharacter Character
    {
        get; private set;
    }

    private Action OnClickCallback = null;

    public void Initialize(PuzzleCharacter character, Action callback)
    {
        this.Character = character;
        this.OnClickCallback = callback;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

}
