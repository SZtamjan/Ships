using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static GridGenerator;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [Header("GameState")]
    public GameState state;
    public GameObject Left;
    public GameObject Right;
    public static GameManager instance;
    public GameObject screnn;

    [Header("Kolory")]
    public Color ships;
    public Color hitsShip;
    public Color miss;
    private Color EnemyShips;

    [Header("Statki")]
    public List<int> statki;
    private int statkiIndex = 0;
    private List<GameObject> doKolorawania;

    [Header("Punkty")]
    public int maxPoints;
    public int MyPoints;
    public int EnemyPoints;
    public GameObject MyShower;
    public GameObject EnemyShower;


    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);

    }

    private void Awake()
    {
        instance = this;
    }

    private void GetColor()
    {
        EnemyShips = Left.GetComponent<GridGenerator>().GetColor();
        EnemyShips.a = 0.99f;
    }


    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                screnn.SetActive(false);
                GetComponent<MouseClick>().enabled = false;
                Left.GetComponent<GridGenerator>().InitateGrid();
                Right.GetComponent<GridGenerator>().InitateGrid();
                GetColor();
                GameManager.instance.UpdateGameState(GameState.SpawnShips);
                //GameManager.instance.UpdateGameState(GameState.EnemySpawnShips);
                break;
            case GameState.SpawnShips:
                MaxPointsCounter();
                StartCoroutine(StawianieStatkow());
                break;
            case GameState.EnemySpawnShips:
                UpdatePoints();
                PlaceEnemyShips();
                break;
            case GameState.PlayerTurn:
                UpdatePoints();
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

    private void MaxPointsCounter()
    {
        maxPoints = 0;
        foreach (var item in statki)
        {
            maxPoints += item;
        }
        MyPoints = maxPoints;
        EnemyPoints = maxPoints;

        UpdatePoints();

    }

    private void UpdatePoints()
    {
        MyShower.GetComponent<TextMeshPro>().text = $"{MyPoints}/{maxPoints}";
        EnemyShower.GetComponent<TextMeshPro>().text = $"{EnemyPoints}/{maxPoints}";

        if (MyPoints == 0)
        {
            screnn.SetActive(true);
            screnn.GetComponent<TextMeshPro>().text = "Przegra³eœ";
            GameManager.instance.UpdateGameState(GameState.Lose);
        }
        else if (EnemyPoints == 0)
        {
            screnn.SetActive(true);
            screnn.GetComponent<TextMeshPro>().text = "Wygra³eœ";
            GameManager.instance.UpdateGameState(GameState.Victory);
        }

    }

    private void PlaceEnemyShips()
    {
        int x = 0;
        while (x < statki.Count)
        {
            Debug.Log($"stawiam statkek {x + 1}");
            GameObject place = null;
            float xPoz = GetRandomInt();
            float yPoz = GetRandomInt(10 - statki[x]);
            Vector2 pos = new Vector2() { x = xPoz, y = yPoz * (-1) };

            foreach (var item in Right.GetComponent<GridGenerator>().plansza)
            {
                if (item.loc.x == pos.x && item.loc.y == pos.y)
                {
                    place = item.pole;
                    break;
                }

            }



            List<GameObject> enemyDoKolorawania = new List<GameObject>();
            enemyDoKolorawania = CanMePlaceShipHere(place, Right, statki[x], EnemyShips);

            if (enemyDoKolorawania.Count == statki[x])
            {
                foreach (var item in enemyDoKolorawania)
                {
                    item.GetComponent<SpriteRenderer>().color = EnemyShips;
                }
                x++;
            }
        }

        GameManager.instance.UpdateGameState(GameState.PlayerTurn);
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

                if (item.pole.GetComponent<SpriteRenderer>().color == ships)
                {
                    item.pole.GetComponent<SpriteRenderer>().color = hitsShip;
                    MyPoints--;
                }
                else
                {
                    item.pole.GetComponent<SpriteRenderer>().color = miss;
                }

                break;
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
    int GetRandomInt(int max)
    {
        Random rand = new Random();
        int asd = rand.Next(0, max);
        return asd;

    }


    private IEnumerator StawianieStatkow() //Ciekawe kim by³ ten Statkow
    {
        while (statkiIndex != statki.Count)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.GetComponent<Transform>().position.x < 0)
                {
                    doKolorawania = new List<GameObject>();
                    GameObject clickedObject = hit.collider.gameObject;

                    doKolorawania = CanMePlaceShipHere(clickedObject, Left, statki[statkiIndex], ships);

                    if (doKolorawania.Count == statki[statkiIndex])
                    {
                        statkiIndex++;
                        foreach (var item in doKolorawania)
                        {
                            item.GetComponent<SpriteRenderer>().color = ships;
                        }
                    }

                }

            }
            yield return null;
        }

        GameManager.instance.UpdateGameState(GameState.EnemySpawnShips);

    }
    private List<GameObject> CanMePlaceShipHere(GameObject clickedObject, GameObject where, int shipSize, Color kolorStatku)
    {
        List<GameObject> result = new List<GameObject>();

        if (clickedObject.GetComponent<Transform>().localPosition.y > -11 + shipSize)
        {
            Debug.Log("Clicked on: " + clickedObject.GetComponent<Transform>().localPosition + clickedObject.GetComponent<Transform>().position);
            Vector2 clickedObjectLocalPos = new Vector2() { x = clickedObject.GetComponent<Transform>().localPosition.x, y = clickedObject.GetComponent<Transform>().localPosition.y };
            foreach (var item in where.GetComponent<GridGenerator>().plansza)
            {

                for (int i = 0; i < shipSize; i++)
                {
                    if (item.loc.y == clickedObjectLocalPos.y - i && item.loc.x == clickedObjectLocalPos.x)
                    {
                        if (item.pole.GetComponent<SpriteRenderer>().color != kolorStatku)
                        {
                            result.Add(item.pole);
                        }
                        else
                        {
                            Debug.Log("Nie mo¿na to przyp³ynaæ statkiem");
                        }


                        //item.pole.GetComponent<SpriteRenderer>().color = ships;
                    }
                }
            }

        }

        return result;
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


                    if (clickedObject.GetComponent<SpriteRenderer>().color == EnemyShips)
                    {
                        clickedObject.GetComponent<SpriteRenderer>().color = hitsShip;
                        EnemyPoints--;
                    }
                    else
                    {
                        clickedObject.GetComponent<SpriteRenderer>().color = miss;
                    }


                    xd = true;


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
