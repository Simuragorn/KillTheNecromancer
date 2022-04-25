using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderButton : MonoBehaviour
{
    [SerializeField] private UnitOrderEnum order;
    [SerializeField] private Button button;
    [SerializeField] private GameObject orderActiveObject;
    [SerializeField] private KeyCode activationButton;

    private void Start()
    {
        orderActiveObject.SetActive(false);
        button.onClick.AddListener(ChangeOrder);
        UnitsSelection.Instance.OnOrderChanged += OnOrderChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationButton) && UnitsSelection.Instance.selectedUnits != null && UnitsSelection.Instance.selectedUnits.Any())
            ChangeOrder();
    }

    private void ChangeOrder()
    {
        UnitsSelection.Instance.ChangeOrder(order);
    }

    private void OnOrderChanged(UnitOrderEnum? currentOrder)
    {
        orderActiveObject.SetActive(currentOrder == order);
    }

}
