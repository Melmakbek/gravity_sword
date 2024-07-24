using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    [SerializeField] private float mass = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D collider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
