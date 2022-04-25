using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _allyLayer;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private UnitDB _unitDB;

    public LayerMask EnemyLayer => _enemyLayer;
    public LayerMask AllyLayer => _allyLayer;

    public static GameManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Unit GetUnitById(UnitEnum unitId)
    {
        return _unitDB.GetUnitById((int)unitId);
    }

    public void Victory()
    {
        victoryPanel.SetActive(true);
    }

    public void Defeat()
    {
        defeatPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
