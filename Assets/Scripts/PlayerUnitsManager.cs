using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitsManager : MonoBehaviour
{
    public static PlayerUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, BaseUnitController> PlayerUnits { private set; get; }
    public int PlayerMoney
    {
        get { return _playerMoney; }
        set
        {
            _playerMoney = value;
            UpdateMoneyText();
        }
    }
    [SerializeField] private BaseUnitController unitPrefab;
    [SerializeField] private int _playerMoney;
    [SerializeField] GameObject unitsRoot;
    private Camera camera;
    private Unit unit;
    [SerializeField] private TextMeshProUGUI moneyText;

    public void RemoveUnit(GameObject unitForRemove)
    {
        if (PlayerUnits.ContainsKey(unitForRemove))
        {
            PlayerUnits.Remove(unitForRemove);
        }
        if (!PlayerUnits.Any() && PlayerMoney < unit.Cost)
        {
            GameManager.Instance.Defeat();
        }
    }

    public void GetReward(int reward)
    {
        PlayerMoney += reward;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        camera = Camera.main;
        PlayerUnits = new Dictionary<GameObject, BaseUnitController>();
        UpdateMoneyText();
    }

    private void Start()
    {
        unit = GameManager.Instance.GetUnitById(unitPrefab.UnitId);
    }


    private void Update()
    {
        CheckSpawnUnit();
    }

    private void CheckSpawnUnit()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            if (PlayerMoney >= unit.Cost)
            {
                Vector2 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                SpawnUnit(spawnPosition);

                PlayerMoney -= unit.Cost;
            }
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = $"{_playerMoney}";
    }

    private void SpawnUnit(Vector2 spawnPosition)
    {
        var unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, unitsRoot.transform);
        unit.Init(null);
    }
}
