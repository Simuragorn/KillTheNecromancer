using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _isSelected;
    [SerializeField] private SpriteRenderer selectedImage;
    [SerializeField] private GameObject unitPosition;
    [SerializeField] private UnitMove unitMove;

    private void Awake()
    {
        SelectOrDeselect(false);
    }

    private void Start()
    {
        UnitsSelection.Instance.AllGameObjectUnits.Add(gameObject, this);
    }

    public void SelectOrDeselect(bool isSelected)
    {
        _isSelected = isSelected;
        selectedImage.enabled = _isSelected;
    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        targetPosition -= (Vector2)unitPosition.transform.localPosition;
        unitMove.SetTarget(targetPosition);
    }
}
