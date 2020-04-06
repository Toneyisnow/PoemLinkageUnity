using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonButton : MonoBehaviour
{
    private Action callback = null;
    public void SetCallback(Action action)
    {
        this.callback = action;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (this.callback != null)
        {
            this.callback();
        }
    }

    private void OnMouseDown()
    {
        if (this.callback != null)
        {
            this.callback();
        }
    }
}
