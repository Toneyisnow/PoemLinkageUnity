using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Slowly pans the background it is attached to:
//   wait 3s -> move left until the image's right edge is flush with the screen's
//   right edge -> wait 3s -> move back right to the start -> repeat forever.
public class PrologueCameraGlance : MonoBehaviour
{
    private enum MovingState
    {
        Wait,
        Moving,
        WaitBack,
        MovingBack
    }

    private float moveTimeSpan = 16.0f;
    private float waitTimeSpan = 3.0f;

    // startX shows the left edge of the image; endX shows the right edge.
    private float startX = 0;
    private float endX = 0;
    private float posY = 0;
    private float posZ = 0;

    private bool canMove = false;
    private float lastTimeStamp = 0;
    private MovingState state;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 localPosition = this.gameObject.transform.localPosition;
        posY = localPosition.y;
        posZ = localPosition.z;

        ComputeRange();

        if (canMove)
        {
            // Begin aligned to the left edge of the image.
            this.gameObject.transform.localPosition = new Vector3(startX, posY, posZ);
        }

        lastTimeStamp = Time.realtimeSinceStartup;
        state = MovingState.Wait;
    }

    private void ComputeRange()
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        Camera camera = Camera.main;
        if (renderer == null || renderer.sprite == null || camera == null || !camera.orthographic)
        {
            canMove = false;
            return;
        }

        float backgroundHalfWidth = renderer.bounds.extents.x;
        float screenHalfWidth = camera.orthographicSize * camera.aspect;
        float cameraX = camera.transform.position.x;

        // Only pan if the background is actually wider than the screen, otherwise
        // moving would expose empty space at the edges.
        if (backgroundHalfWidth <= screenHalfWidth)
        {
            canMove = false;
            return;
        }

        startX = cameraX - screenHalfWidth + backgroundHalfWidth; // left edge flush
        endX = cameraX + screenHalfWidth - backgroundHalfWidth;   // right edge flush
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }

        float deltaTime = Time.realtimeSinceStartup - lastTimeStamp;
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
                SetPositionX(Mathf.Lerp(startX, endX, Mathf.Clamp01(deltaTime / moveTimeSpan)));
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
                SetPositionX(Mathf.Lerp(endX, startX, Mathf.Clamp01(deltaTime / moveTimeSpan)));
                if (deltaTime > moveTimeSpan)
                {
                    state = MovingState.Wait;
                    lastTimeStamp = Time.realtimeSinceStartup;
                }
                break;
        }
    }

    private void SetPositionX(float posX)
    {
        this.gameObject.transform.localPosition = new Vector3(posX, posY, posZ);
    }
}
