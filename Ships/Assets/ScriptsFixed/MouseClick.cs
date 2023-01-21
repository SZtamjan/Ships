using System.Collections;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public GameObject pole;
    bool clicked = false;

    public GameObject GetTile()
    {
        StartCoroutine(MuseClick());
        return pole;
    }

    private IEnumerator MuseClick()
    {
        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {

                    GameObject clickedObject = hit.collider.gameObject;
                    // Do something with the clicked object
                    Debug.Log("Clicked on: " + clickedObject.GetComponent<Transform>().localPosition + clickedObject.GetComponent<Transform>().position);
                    pole = clickedObject;
                    yield return new WaitForSeconds(1f);

                }
            }

            yield return null;
        }
    }

}