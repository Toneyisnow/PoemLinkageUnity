using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePreviewRenderer : MonoBehaviour
{
    public int StageId
    {
        get; private set;
    }


    private Action<int> callback = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int stageId)
    {
        this.StageId = stageId;

        // Read the records
    }

    public void SetCallback(Action<int> action)
    {
        this.callback = action;
    }

    private void OnMouseDown()
    {
        if (this.callback != null)
        {
            this.callback(this.StageId);
        }
    }
}
