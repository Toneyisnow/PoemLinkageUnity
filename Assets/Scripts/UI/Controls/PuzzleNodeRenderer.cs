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

    private Action<PuzzleCharacter> OnClickCallback = null;

    public void Initialize(PuzzleCharacter character, Action<PuzzleCharacter> callback)
    {
        this.Character = character;
        this.OnClickCallback = callback;

        if (this.gameObject.GetComponent<BoxCollider2D>() == null)
        {
            this.gameObject.AddComponent<BoxCollider2D>();
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        this.OnClickCallback(this.Character);
    }

}
