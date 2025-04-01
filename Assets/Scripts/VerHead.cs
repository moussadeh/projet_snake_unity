using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ver : MonoBehaviour
{
    Vector2 dir;
    List<Transform> segments = new List<Transform>();
    [SerializeField] Transform segmentPrefab;

    void Start()
    {
        Time.timeScale = 0.18f;
        dir = Vector2.right;
        segments.Add(transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && dir.x != 0)
        {
            dir = Vector2.up;
        } 
        else if (Input.GetKeyDown(KeyCode.DownArrow) && dir.x != 0)
        {
            dir = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && dir.y != 0)
        {
            dir = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && dir.y != 0)
        {
            dir = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }

        // Move the head of the snake
        float x = Mathf.Round(transform.position.x) + dir.x;
        float y = Mathf.Round(transform.position.y) + dir.y;

        transform.position = new Vector3(x, y);
    }

    void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Molecule") Grow();
    }
}
