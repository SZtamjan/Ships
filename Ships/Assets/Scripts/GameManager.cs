using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    public GameState State;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }
    public void ChangeState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.GenerateEnemyGrid:
                //GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnShips:
                break;
            case GameState.EnemySpawnShips:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid,
    GenerateEnemyGrid,
    SpawnShips,
    EnemySpawnShips,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose
}