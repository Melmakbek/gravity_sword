using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hero : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int jumps = 2;
    private int jumpsBuffer = 2;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }
    // private void FixedUpdate() 
    // {
    //     CheckGround();
    //     if(isGrounded)
    //         jumpsBuffer = jumps;
    // }
    private void Update()
    {
        if(Input.GetButton("Horizontal"))
            Run();
        if(jumpsBuffer > 0 && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            jumpsBuffer--;
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            changeGravity(1);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
            changeGravity(2);
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log("Enter" + isGrounded.ToString() + contact.point.y.ToString() + collision.otherCollider.bounds.min.y.ToString());
        if (Math.Abs(contact.point.y - collision.otherCollider.bounds.min.y) < 0.3f){
           jumpsBuffer = jumps;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log("Stay" + isGrounded.ToString() + contact.point.y.ToString() + collision.otherCollider.bounds.min.y.ToString());
        if (Math.Abs(contact.point.y - collision.otherCollider.bounds.min.y) < 0.01f){
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Debug.Log("Exit" + isGrounded.ToString() + contact.point.y.ToString() + collision.otherCollider.bounds.min.y.ToString());
        isGrounded = false;
    }

    private void CheckGround() 
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 7.5f);
        isGrounded = collider.Length > 1;
    }

    private void changeGravity(int x)
    {
        switch(x)
        {
            case 1:
                Debug.Log("Up");
                Physics2D.gravity = new Vector3(0, 9.81F);
                break;
            case 2:
                Physics2D.gravity = new Vector3(0, -9.81F);
                break;
        }
    }
}
