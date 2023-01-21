using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridGenerator;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GameState state;
    public GameObject Left;
    public GameObject Right;
    public static GameManager instance;
    public Color ships;
    public Color hitsShip;
    public Color miss;
    int MinDlStatkow = 0;
    public int maxDlStatkow = 5;

    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);
    }


    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GetComponent<MouseClick>().enabled = false;
                Left.GetComponent<GridGenerator>().InitateGrid();
                Right.GetComponent<GridGenerator>().InitateGrid();
                GameManager.instance.UpdateGameState(GameState.SpawnShips);
                break;
            case GameState.SpawnShips:
                StartCoroutine(StawianieStatkow());
                break;
            case GameState.EnemySpawnShips:
                break;
            case GameState.PlayerTurn:
                StartCoroutine(Strzelanie());
                break;
            case GameState.EnemyTurn:
                EnemyStrzela();
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void EnemyStrzela()
    {
        int x = GetRandomInt();
        int y = GetRandomInt() * (-1);
        var lista = Left.GetComponent<GridGenerator>().plansza;
        foreach (Plansza item in lista)
        {
            if (item.loc.x == x && item.loc.y == y)
            {
                item.pole.GetComponent<SpriteRenderer>().color = hitsShip;
            }
        }
        GameManager.instance.UpdateGameState(GameState.PlayerTurn);
    }

    int GetRandomInt()
    {
        Random rand = new Random();
        int asd = rand.Next(0, 10);
        return asd;

    }

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator StawianieStatkow() //Ciekawe kim by³ ten Statkow
    {
        while (MinDlStatkow != maxDlStatkow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.GetComponent<Transform>().position.x < 0)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    if (clickedObject.GetComponent<Transform>().localPosition.y > -10 + MinDlStatkow)
                    {
                        Debug.Log("Clicked on: " + clickedObject.GetComponent<Transform>().localPosition + clickedObject.GetComponent<Transform>().position);
                        Vector2 dwa = new Vector2() { x = clickedObject.GetComponent<Transform>().localPosition.x, y = clickedObject.GetComponent<Transform>().localPosition.y };
                        foreach (var item in Left.GetComponent<GridGenerator>().plansza)
                        {
                            for (int i = 0; i <= MinDlStatkow; i++)
                            {
                                if (item.loc.y == dwa.y - i && item.loc.x == dwa.x)
                                {
                                    item.pole.GetComponent<SpriteRenderer>().color = ships;
                                }
                            }
                        }
                        MinDlStatkow++;
                    }

                }

            }
            yield return null;
        }
        GameManager.instance.UpdateGameState(GameState.EnemyTurn);

    }

    private IEnumerator Strzelanie() //Ciekawe kim by³ ten Statkow
    {
        bool xd = false;
        while (!xd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.GetComponent<Transform>().position.x >= 0)
                {
                    GameObject clickedObject = hit.collider.gameObject;

                    Debug.Log("Clicked on: " + clickedObject.GetComponent<Transform>().localPosition + clickedObject.GetComponent<Transform>().position);
                    Vector2 dwa = new Vector2() { x = clickedObject.GetComponent<Transform>().localPosition.x, y = clickedObject.GetComponent<Transform>().localPosition.y };
                    foreach (var item in Right.GetComponent<GridGenerator>().plansza)
                    {

                        if (item.loc.y == dwa.y && item.loc.x == dwa.x)
                        {
                            item.pole.GetComponent<SpriteRenderer>().color = ships;
                            xd = true;
                        }

                    }


                }

            }
            yield return null;
        }
        GameManager.instance.UpdateGameState(GameState.EnemyTurn);

    }

    private void StawianieStatku1(int lonk)
    {
        GameObject pole = GetComponent<MouseClick>().GetTile();
    }


    public enum GameState
    {
        GenerateGrid,
        SpawnShips,
        EnemySpawnShips,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Lose
    }
}
