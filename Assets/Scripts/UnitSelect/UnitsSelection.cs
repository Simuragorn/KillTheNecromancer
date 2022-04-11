using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSelection : MonoBehaviour
{
    public Dictionary<GameObject, Unit> AllGameObjectUnits;
    public List<Unit> selectedUnits { private set; get; } = new List<Unit>();

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
        List<Vector3> positions = GetPositionsListAround(targetPosition, new float[] { 1f, 2f, 3f }, new int[] { 5, 10, 20 });
        for (int i = 0; i < selectedUnits.Count; ++i)
        {
            Unit selectedUnit = selectedUnits[i];
            selectedUnit.MoveTo(positions[i], MoveTypeEnum.ToPosition);
        }
    }

    private List<Vector3> GetPositionsListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        var positionsList = new List<Vector3>();
        positionsList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; ++i)
        {
            positionsList.AddRange(GetPositionsListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionsList;
    }

    private List<Vector3> GetPositionsListAround(Vector3 startPosition, float distance, int positionsCount)
    {
        var positionsList = new List<Vector3>();
        for (int i = 0; i < positionsCount; ++i)
        {
            float angle = i * (360 / positionsCount);
            Vector3 direction = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + direction * distance;
            positionsList.Add(position);
        }
        return positionsList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
