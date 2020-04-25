using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePreviewRenderer : MonoBehaviour
{
    public GameObject background = null;
    public GameObject locker = null;

    public GameObject star1 = null;
    public GameObject star2 = null;
    public GameObject star3 = null;



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

    public void SetEnable(bool enabled, int star)
    {
        if (!enabled)
        {
            this.star1.SetActive(false);
            this.star2.SetActive(false);
            this.star3.SetActive(false);
            this.locker.SetActive(true);

            // Grey out
            //Material myNewMaterial = new Material(Shader.Find("Grayscale"));
            //background.GetComponent<SpriteRenderer>().material = myNewMaterial;
        }
        else
        {
            this.star1.SetActive(star >= 1);
            this.star2.SetActive(star >= 2);
            this.star3.SetActive(star >= 3);
            this.locker.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (this.callback != null)
        {
            this.callback(this.StageId);
        }
    }

}
