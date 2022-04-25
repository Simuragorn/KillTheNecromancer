using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitClick : MonoBehaviour
{
    [SerializeField] private LayerMask unitMask;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnitOrMove();
        }
        if (Input.GetMouseButtonDown(1))
        {
            UnitsSelection.Instance.DeselectAll();
        }
    }

    private void SelectUnitOrMove()
    {
        Vector2 targetPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        List<Collider2D> colliders = Physics2D.OverlapPointAll(targetPosition, unitMask).ToList();
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
        else if (!UnitsSelection.Instance.DragSelection && 
            !EventSystem.current.IsPointerOverGameObject())
        {
                UnitsSelection.Instance.MoveToPosition(targetPosition);
        }
    }
}
