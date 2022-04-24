using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsSelection : MonoBehaviour
{
    public Dictionary<GameObject, SelectableUnit> AllGameObjectUnits;
    public List<SelectableUnit> selectedUnits { private set; get; } = new List<SelectableUnit>();

    public static UnitsSelection Instance { get; private set; }
    public bool DragSelection { get; set; }
    public Action<UnitOrderEnum?> OnOrderChanged;
    public Action<UnitFormationEnum> OnFormationChanged;
    public UnitFormationEnum CurrentFormation;
    private void Awake()
    {
        Instance = this;
        AllGameObjectUnits = new Dictionary<GameObject, SelectableUnit>();
        selectedUnits = new List<SelectableUnit>();
    }

    private void Start()
    {
        ChangeFormation(CurrentFormation);
    }

    public void ClickSelect(GameObject gameObject)
    {
        DeselectAll();
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
        {
            selectedUnits.Add(unit);
            unit.SelectOrDeselect(true);
            OnOrderChanged(unit.Unit.CurrentOrder);
        }
    }
    public void ShiftClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
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
        SetSameOrderToAll();
    }
    public void DragClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
        {
            if (!selectedUnits.Contains(unit))
            {
                selectedUnits.Add(unit);
                unit.SelectOrDeselect(true);
            }
        }
        SetSameOrderToAll();
    }
    public void DeselectAll()
    {
        selectedUnits.ForEach(u => u.SelectOrDeselect(false));
        selectedUnits.Clear();
        OnOrderChanged(null);
    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        List<Vector3> positions = Assets.Scripts.UnitSelect.UnitFormationHelper.GetFormationPositions(CurrentFormation, targetPosition, selectedUnits.Count);
        for (int i = 0; i < selectedUnits.Count; ++i)
        {
            SelectableUnit selectedUnit = selectedUnits[i];
            selectedUnit.Unit.MoveTo(positions[i], MoveTypeEnum.ToPosition);
        }
    }

    public void ChangeOrder(UnitOrderEnum unitOrder)
    {
        if (selectedUnits == null)
            return;

        if (unitOrder == UnitOrderEnum.DistantAttack)
        {
            if (!selectedUnits.Any(u => u.Unit.Unit.UnitType == UnitTypeEnum.Distant))
                return;
        }

        selectedUnits.ForEach(u => u.Unit.ChangeOrder(unitOrder));
        if (OnOrderChanged != null)
            OnOrderChanged(unitOrder);
    }

    public void ChangeFormation(UnitFormationEnum unitFormation)
    {
        CurrentFormation = unitFormation;
        if (OnFormationChanged != null)
            OnFormationChanged(unitFormation);
    }

    public void SetSameOrderToAll()
    {
        if (selectedUnits == null)
            return;

        UnitOrderEnum popularOrder = GetMajorityOrder();
        ChangeOrder(popularOrder);
    }

    private UnitOrderEnum GetMajorityOrder()
    {
        var order = UnitOrderEnum.Attack;
        if (selectedUnits == null)
            return order;

        var ordersQuantity = new Dictionary<UnitOrderEnum, int>();
        selectedUnits.ForEach(u =>
        {
            if (ordersQuantity.ContainsKey(u.Unit.CurrentOrder))
                ordersQuantity[u.Unit.CurrentOrder]++;
            else
                ordersQuantity.Add(u.Unit.CurrentOrder, 1);
        });
        return order;
    }
}
