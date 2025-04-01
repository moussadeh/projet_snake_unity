using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour
{
    [SerializeField] Collider2D zoneSpawn;

    void Start()
    {
        SpawnFood();
    }

    void Update()
    {
        
    }

    void SpawnFood()
    {
        Bounds bounds = zoneSpawn.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        x= Mathf.Round(x);
        y= Mathf.Round(y);

        transform.position = new Vector2(x, y);
    }

    void OnTriggerEnter2D()
    {
        SpawnFood();
    }
}
