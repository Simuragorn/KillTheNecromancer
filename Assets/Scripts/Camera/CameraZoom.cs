using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float maxZoomMultiplier;
    [SerializeField] private float speed;
    [SerializeField] Camera camera;


    private float originalCameraSize;
    private float maxCameraSize;

    private void Start()
    {
        originalCameraSize = camera.orthographicSize;
        maxCameraSize = originalCameraSize * maxZoomMultiplier;
    }
    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        float distance = -Input.mouseScrollDelta.y * speed * Time.deltaTime;
        float size = Mathf.Min(distance + camera.orthographicSize, maxCameraSize);
        size = Mathf.Max(size, originalCameraSize / maxZoomMultiplier);
        camera.orthographicSize = size;
    }
}
