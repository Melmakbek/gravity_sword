using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resident : MonoBehaviour
{
    [SerializedField] private Vector2 pointA = new Vector2();
    [SerializedField] private int pointB;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D collider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        startPatrolling(pointA);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
