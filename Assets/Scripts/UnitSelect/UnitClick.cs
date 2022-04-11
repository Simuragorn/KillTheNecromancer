using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    [SerializeField] private LayerMask clickable;
    [SerializeField] private LayerMask ground;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            List<Collider2D> colliders = Physics2D.OverlapPointAll(targetPosition, clickable).ToList();
            var collider = colliders.FirstOrDefault();
            if (collider != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UnitsSelection.Instance.ShiftClickSelect(collider.gameObject);
                }
                else
                {
                    UnitsSelection.Instance.ClickSelect(collider.gameObject);
                }
            }
            else
            {
                UnitsSelection.Instance.MoveToPosition(targetPosition);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            UnitsSelection.Instance.DeselectAll();
        }
    }
}
