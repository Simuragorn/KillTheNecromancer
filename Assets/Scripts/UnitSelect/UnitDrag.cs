using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    [SerializeField] private RectTransform boxVisual;

    private Rect selectionBox;
    private Camera camera;

    private Vector2 startPosition;
    private Vector2 endPosition;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
            SelectUnits();
        }
    }

    private void DrawVisual()
    {
        boxVisual.position = (startPosition + endPosition) / 2;

        Vector2 boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));

        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        selectionBox.xMin = Mathf.Min(startPosition.x, endPosition.x);
        selectionBox.yMin = Mathf.Min(startPosition.y, endPosition.y);

        selectionBox.xMax = Mathf.Max(startPosition.x, endPosition.x);
        selectionBox.yMax = Mathf.Max(startPosition.y, endPosition.y);
    }

    private void SelectUnits()
    {
        bool isFirstUnit = true;
        foreach (var unit in UnitsSelection.Instance.AllGameObjectUnits)
        {
            Vector2 unitPosition = camera.WorldToScreenPoint(unit.Key.transform.position);
            if (selectionBox.Contains(unitPosition))
            {
                if (isFirstUnit)
                {
                    UnitsSelection.Instance.DeselectAll();
                    isFirstUnit = false;
                }

                UnitsSelection.Instance.DragClickSelect(unit.Key);
                StartCoroutine(OnDragSelection());
            }
        }
    }

    private IEnumerator OnDragSelection()
    {
        UnitsSelection.Instance.DragSelection = true;
        yield return null;
        UnitsSelection.Instance.DragSelection = false;
    }
}
