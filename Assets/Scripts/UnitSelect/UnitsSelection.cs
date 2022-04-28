using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitsSelection : MonoBehaviour
{
    [SerializeField] private DistantTarget distantTargetMarker;
    public Dictionary<GameObject, SelectableUnit> AllGameObjectUnits;
    public List<SelectableUnit> SelectedUnits { private set; get; } = new List<SelectableUnit>();
    private Dictionary<KeyCode, List<SelectableUnit>> bindedUnits = new Dictionary<KeyCode, List<SelectableUnit>>();

    public static UnitsSelection Instance { get; private set; }
    public bool DragSelection { get; set; }
    public Action<UnitOrderEnum?> OnOrderChanged;
    public UnitOrderEnum CurrentOrder;
    public Action<UnitFormationEnum> OnFormationChanged;
    public UnitFormationEnum CurrentFormation;

    private List<KeyCode> bindingKeys;
    private void Awake()
    {
        Instance = this;
        AllGameObjectUnits = new Dictionary<GameObject, SelectableUnit>();
        SelectedUnits = new List<SelectableUnit>();
        distantTargetMarker.gameObject.SetActive(false);
        bindingKeys = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0,
        };
    }

    private void Start()
    {
        ChangeFormation(CurrentFormation);
    }

    private void Update()
    {
        KeyCode bindingKey = bindingKeys.FirstOrDefault(key => Input.GetKeyUp(key));
        if (bindingKey != default(KeyCode))
        {
            if (!bindedUnits.ContainsKey(bindingKey))
                bindedUnits.Add(bindingKey, new List<SelectableUnit>());
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                bindedUnits[bindingKey] = new List<SelectableUnit>(SelectedUnits);
            }
            else
            {
                List<SelectableUnit> units = bindedUnits[bindingKey].ToList().Where(u => AllGameObjectUnits.ContainsKey(u?.gameObject)).ToList();
                DeselectAll();
                SelectedUnits = units;
                SelectedUnits.ForEach(u => u.SelectOrDeselect(true));
                SetSameOrderToAll();
            }
        }
    }

    public void ClickSelect(GameObject gameObject)
    {
        DeselectAll();
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
        {
            SelectedUnits.Add(unit);
            unit.SelectOrDeselect(true);
            OnOrderChanged(unit.Unit.CurrentOrder);
        }
    }
    public void ShiftClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
        {
            bool isAdd = !SelectedUnits.Contains(unit);
            if (isAdd)
            {
                SelectedUnits.Add(unit);
            }
            else
            {
                SelectedUnits.Remove(unit);
            }
            unit.SelectOrDeselect(isAdd);
        }
        SetSameOrderToAll();
    }
    public void DragClickSelect(GameObject gameObject)
    {
        if (AllGameObjectUnits.TryGetValue(gameObject, out SelectableUnit unit))
        {
            if (!SelectedUnits.Contains(unit))
            {
                SelectedUnits.Add(unit);
                unit.SelectOrDeselect(true);
            }
        }
        SetSameOrderToAll();
    }
    public void DeselectAll()
    {
        SelectedUnits.ForEach(u => u.SelectOrDeselect(false));
        SelectedUnits.Clear();
        OnOrderChanged(null);
    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        if (CurrentOrder == UnitOrderEnum.DistantAttack)
        {
            MoveForDistantAttack(targetPosition);
            return;
        }

        List<Vector3> positions = Assets.Scripts.UnitSelect.UnitFormationHelper.GetFormationPositions(CurrentFormation, targetPosition, SelectedUnits.Count);
        for (int i = 0; i < SelectedUnits.Count; ++i)
        {
            SelectableUnit selectedUnit = SelectedUnits[i];
            selectedUnit.Unit.MoveTo(positions[i], MoveTypeEnum.ToPosition);
        }
    }

    public void HideMarkers()
    {
        distantTargetMarker.gameObject.SetActive(false);
    }

    public void ChangeOrder(UnitOrderEnum unitOrder)
    {
        if (SelectedUnits == null)
            return;

        distantTargetMarker.Hide();

        if (unitOrder == UnitOrderEnum.DistantAttack)
        {
            if (!SelectedUnits.Any(u => u.Unit.Unit.UnitType == UnitTypeEnum.Distant))
                return;
            distantTargetMarker.MoveWithCursor(GetMaxDistantOffset());
        }

        SelectedUnits.ForEach(u => u.Unit.ChangeOrder(unitOrder));
        CurrentOrder = unitOrder;
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
        if (SelectedUnits == null)
            return;

        UnitOrderEnum popularOrder = GetMajorityOrder();
        ChangeOrder(popularOrder);
    }

    private void MoveForDistantAttack(Vector2 targetPosition)
    {
        if (distantTargetMarker.IsCursor)
        {
            List<BaseUnitController> distanceUnits = SelectedUnits.Where(u => u.Unit.Unit.UnitType == UnitTypeEnum.Distant).Select(u => u.Unit).ToList();

            distantTargetMarker.Hide();
            distanceUnits.ForEach(u => u.SetTarget(targetPosition));
        }
        else
            distantTargetMarker.MoveWithCursor(GetMaxDistantOffset());
    }

    private float GetMaxDistantOffset()
    {
        if (SelectedUnits == null)
            return 0;
        return SelectedUnits.Max(u => u.Unit.Unit.DistantSpotOffset);
    }

    private UnitOrderEnum GetMajorityOrder()
    {
        var order = UnitOrderEnum.Attack;
        if (!SelectedUnits.Any())
            return order;

        var ordersQuantity = new Dictionary<UnitOrderEnum, int>();
        SelectedUnits.ForEach(u =>
        {
            if (ordersQuantity.ContainsKey(u.Unit.CurrentOrder))
                ordersQuantity[u.Unit.CurrentOrder]++;
            else
                ordersQuantity.Add(u.Unit.CurrentOrder, 1);
        });
        order = ordersQuantity.First(o => o.Value == ordersQuantity.Max(o => o.Value)).Key;
        return order;
    }
}
