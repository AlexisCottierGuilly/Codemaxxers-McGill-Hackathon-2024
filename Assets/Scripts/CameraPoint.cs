using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public GameObject target;
    public Vector2 speed = new Vector2(1f, 0.5f);
    public float speedModifier = 2f;

    public CloudGenerator cloudGenerator;

    private float distance = 15f;
    private bool mouseDown = false;

    private Vector2 lastMousePosition;
    private float lastNonZeroSpeed = 0;

    private float maxRadiusModifier = 2f;

    void Start()
    {
        distance = cloudGenerator.maxRadius * maxRadiusModifier;
    }

    void Update()
    {
        HandleMouseMovement();
        HandleMovement();
    }
    
    void HandleMouseMovement()
    {
        // If left mouse button is pressed, rotate the orbital
        // If released, continue rotating with speed
        if (Input.GetButton("Fire1"))
        {
            mouseDown = true;

            if (lastMousePosition != new Vector2(0, 0))
            {
                float deltaX = Input.mousePosition.x - lastMousePosition.x;
                float deltaY = Input.mousePosition.y - lastMousePosition.y;

                transform.Translate(new Vector3(-deltaX, 0, 0) * Time.deltaTime * 25f);
                transform.LookAt(target.transform);

                if (deltaX != 0)
                    lastNonZeroSpeed = deltaX;

                if (lastNonZeroSpeed != 0)
                    speed = new Vector2(-lastNonZeroSpeed / 5f, speed.y);
                else
                    speed = new Vector2(0, speed.y);
            }

            lastMousePosition = Input.mousePosition;
        }
        else
        {
            mouseDown = false;
            lastMousePosition = new Vector2(0, 0);
            lastNonZeroSpeed = 0;
        }
    }

    void HandleMovement()
    {
        // Rotate the camera so it faces the orbital (give the effect that the orbital is rotating)
        float scroll = Input.mouseScrollDelta.y;
        distance -= scroll * (0.8f + distance / 10f);

        distance = Mathf.Max(Mathf.Min(cloudGenerator.maxRadius * maxRadiusModifier, distance), 2f);
        
        if (!mouseDown)
        {
            transform.Translate(new Vector3(speed.x, 0, 0) * Time.deltaTime * speedModifier * distance);
            transform.LookAt(target.transform);
        }

        Vector3 directionToTarget = transform.position - target.transform.position;
        Vector3 normalizedDirection = directionToTarget.normalized;
        transform.position = target.transform.position + normalizedDirection * distance;
    }
}
