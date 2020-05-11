using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueCameraGlance : MonoBehaviour
{
    private enum MovingState
    {
        Wait,
        Moving,
        WaitBack,
        MovingBack
    }

    private float startX = 3.5f;
    private float endX = -3.5f;

    private float moveTimeSpan = 16.0f;
    private float waitTimeSpan = 3.0f;

    private float lastTimeStamp = 0;

    private MovingState state;
    private float posY = 0;

    // Start is called before the first frame update
    void Start()
    {
        lastTimeStamp = Time.realtimeSinceStartup;
        state = MovingState.Wait;
        posY = this.gameObject.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.realtimeSinceStartup - lastTimeStamp;
        float posX = 0;
        switch (state)
        {
            case MovingState.Wait:
                if (deltaTime > waitTimeSpan)
                {
                    state = MovingState.Moving;
                    lastTimeStamp = Time.realtimeSinceStartup;
                }
                break;
            case MovingState.Moving:

                posX = startX + deltaTime / moveTimeSpan * (endX - startX);
                this.gameObject.transform.localPosition = new Vector3(posX, posY, 0);

                if (deltaTime > moveTimeSpan)
                {
                    state = MovingState.WaitBack;
                    lastTimeStamp = Time.realtimeSinceStartup;
                }
                break;
            case MovingState.WaitBack:
                if (deltaTime > waitTimeSpan)
                {
                    state = MovingState.MovingBack;
                    lastTimeStamp = Time.realtimeSinceStartup;
                }
                break;
            case MovingState.MovingBack:
                posX = endX + deltaTime / moveTimeSpan * (startX - endX);
                this.gameObject.transform.localPosition = new Vector3(posX, posY, 0);

                if (deltaTime > moveTimeSpan)
                {
                    state = MovingState.Wait;
                    lastTimeStamp = Time.realtimeSinceStartup;
                }
                break;
        }
    }
}
