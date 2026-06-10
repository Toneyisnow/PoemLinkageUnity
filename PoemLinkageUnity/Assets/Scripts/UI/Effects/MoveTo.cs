using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    private Vector2 targetPosition;

    private Vector3 startPosition;

    private float timeSpan;

    private float startTime = 0;

    public void Initialize(Vector2 target, float timeSpan)
    {
        targetPosition = target;
        this.timeSpan = timeSpan;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.gameObject.transform.localPosition;
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.realtimeSinceStartup - startTime;
        if (deltaTime > timeSpan)
        {
            Destroy(this);
        }

        float rate = deltaTime / timeSpan;
        if (rate > 1)
        {
            rate = 1;
        }

        float posX = startPosition.x + (targetPosition.x - startPosition.x) * rate;
        float posY = startPosition.y + (targetPosition.y - startPosition.y) * rate;

        this.gameObject.transform.localPosition = new Vector3(posX, posY, startPosition.z);
    }
}
