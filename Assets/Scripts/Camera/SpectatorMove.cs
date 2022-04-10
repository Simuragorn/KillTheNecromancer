using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float xBorder;
    [SerializeField] private float yBorder;
    [SerializeField] private Transform spectator;

    void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        float distance = speed * Time.deltaTime;

        float xPosition = horizontalMove * distance + spectator.position.x;
        float yPosition = verticalMove * distance + spectator.position.y;

        xPosition = Mathf.Max(xPosition, -xBorder);
        xPosition = Mathf.Min(xPosition, xBorder);


        yPosition = Mathf.Max(yPosition, -yBorder);
        yPosition = Mathf.Min(yPosition, yBorder);


        spectator.position = new Vector3(xPosition, yPosition, spectator.position.z);
    }
}
