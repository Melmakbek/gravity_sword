using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int jumps = 2;
    private int jumpsBuffer = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }
    private void FixedUpdate() 
    {
        if(CheckGround())
            jumpsBuffer = jumps;
    }
    private void Update()
    {
        if(Input.GetButton("Horizontal"))
            Run();
        if((jumpsBuffer > 0) && Input.GetButtonDown("Jump"))
        {
            jumpsBuffer = jumpsBuffer - 1;
            Jump();
        }
    }
    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private bool CheckGround() 
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 1f);
        return collider.Length > 1;
    }
}
