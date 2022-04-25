using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistantTarget : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSpotSprite;
    private Camera camera;
    private float targetRadius;
    public bool IsCursor { private set; get; }

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (IsCursor)
        {
            Vector2 cursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPosition;
        }
    }

    public void SetPosition(float distantSpotOffset, Vector2 position)
    {
        transform.position = position;
        targetRadius = distantSpotOffset;
        targetSpotSprite.size = new Vector2(targetRadius, targetRadius);
        IsCursor = false;
        gameObject.SetActive(true);
    }

    public void MoveWithCursor(float distantSpotOffset)
    {
        targetRadius = distantSpotOffset;
        targetSpotSprite.size = new Vector2(targetRadius, targetRadius);
        IsCursor = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        IsCursor = false;
        gameObject.SetActive(false);
    }
}
