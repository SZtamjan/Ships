using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] public int szer = 10;
    [SerializeField] public int wys = 10;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] public Vector2 przes;


    private void Start()
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
            }
        }
        Vector2 lewo = new Vector2(-8, 2.50f);
        gameObject.transform.position = przes;
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

}
