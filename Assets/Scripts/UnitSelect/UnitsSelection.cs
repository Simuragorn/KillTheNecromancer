using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSelection : MonoBehaviour
{
    public Dictionary<GameObject, Unit> AllGameObjectUnits;
    private List<Unit> selectedUnits = new List<Unit>();

    public static UnitsSelection Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        AllGameObjectUnits = new Dictionary<GameObject, Unit>();
        selectedUnits = new List<Unit>();
    }

    public void ClickSelect(GameObject gameObject)
    {
        DeselectAll();
        if (AllGameObjectUnits.TryGetValue(gameObject, out Unit unit))
        {
            selectedUnits.Add(unit);
            unit.SelectOrDeselect(true);
        }
    }
    public void ShiftClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out Unit unit))
        {
            bool isAdd = !selectedUnits.Contains(unit);
            if (isAdd)
            {
                selectedUnits.Add(unit);
            }
            else
            {
                selectedUnits.Remove(unit);
            }
            unit.SelectOrDeselect(isAdd);
        }
    }
    public void DragClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out Unit unit))
        {
            if (!selectedUnits.Contains(unit))
            {
                selectedUnits.Add(unit);
                unit.SelectOrDeselect(true);
            }
        }
    }
    public void DeselectAll()
    {
        selectedUnits.ForEach(u => u.SelectOrDeselect(false));
        selectedUnits.Clear();
    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        selectedUnits.ForEach(u => u.MoveToPosition(targetPosition));
    }
}
