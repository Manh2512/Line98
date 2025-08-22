using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class MainBoard : MonoBehaviour
{
    private Vector2 center = Vector2.zero;
    private Vector2 size = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        size = collider.bounds.size;
        center = collider.bounds.center;
        Debug.Log("Board Collider Size: " + size);
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Vector2 index = BoardPosition.FromCoorToIndex(mousePosition, size, center);
                Debug.Log("Clicked on: " + gameObject.name + " at " + mousePosition);
            }
        }
    }

}
