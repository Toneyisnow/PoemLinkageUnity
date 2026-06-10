using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveAction : MonoBehaviour
{

    private Vector3 originPosition = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private float t = 0;

    private bool disposeObject = false;
    public bool HasFinished
    {
        get; private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Vector3 targetPos, bool disposeObject)
    {
        this.originPosition = this.gameObject.transform.localPosition;
        this.targetPosition = targetPos;

        this.disposeObject = disposeObject;

        this.HasFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 1.4f;
        this.gameObject.transform.localPosition = Vector3.Lerp(originPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, t));


        if (this.gameObject.transform.localPosition == targetPosition)
        {
            Debug.Log("TestMoveAtion: hasFinished.");
            this.HasFinished = true;

            if (disposeObject && this.gameObject != null)
            {
            }
        }
    }
}
