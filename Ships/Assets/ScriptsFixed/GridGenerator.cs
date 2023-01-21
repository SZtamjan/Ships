using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] public int szer = 10;
    [SerializeField] public int wys = 10;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] public Vector2 przes;
    //[SerializeField] 
    public List<Plansza> plansza;
    [System.Serializable] 
    public class Plansza
    {
        public Vector2 loc;
        public GameObject pole;
    }


    public void InitateGrid()
    {
        przes = transform.position;
        transform.position = new Vector2(0, 0);
        GenerateGrid();

    }

    void GenerateGrid()
    {
        for(int x = 0; x < szer; x++)
        {
            for(int y = 0; y > wys*(-1); y--)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x,y),Quaternion.identity);
                node.transform.parent = transform;

                Plansza tmp = new Plansza() { loc = new Vector2(x, y), pole = node };
                plansza.Add(tmp);
            }
        }
        gameObject.transform.position = przes;
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

}
