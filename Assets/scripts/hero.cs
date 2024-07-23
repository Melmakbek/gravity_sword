using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class hero : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 0.1f;
    [SerializeField] private int jumps = 2;
    private int jumpsBuffer = 2;
    private float jumpCheckArea = 0.5f;
    private float accuracyCheckArea = 0.05f;
    private bool instantJump = false;
    private bool isGrounded = false;

    private Vector2 currentGravityDirection = new Vector2(0, -1);
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Collider2D playerCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(Input.GetButton("Horizontal"))
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("Up");
                changeGravity("Left");
            }   
            if(Input.GetKeyDown(KeyCode.RightArrow))
                changeGravity("Right");
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Run();
            }
        if(Input.GetButtonDown("Jump"))
        {

            if (jumpsBuffer > 0){
                jumpsBuffer--;
                Jump();
            }
            else {
                Vector2 lu = new Vector2(0, 0);
                Vector2 rd = new Vector2(0, 0);
                if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
                    lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.min.y);
                    rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.min.y - jumpCheckArea);
                } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
                    lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.max.y + jumpCheckArea);
                    rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.max.y);
                } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
                    lu = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.max.y - accuracyCheckArea);
                    rd = new Vector2(playerCollider.bounds.max.x + jumpCheckArea, playerCollider.bounds.min.y + accuracyCheckArea);
                } else {
                    lu = new Vector2(playerCollider.bounds.min.x - jumpCheckArea, playerCollider.bounds.max.y - accuracyCheckArea);
                    rd = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y + accuracyCheckArea);
                }
                Collider2D[] collider = Physics2D.OverlapAreaAll(lu, rd);
                if (collider.Length > 1)
                    instantJump = true;
            }
        }
        if(Input.GetButton("Vertical"))
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Up");
                changeGravity("Up");
            }   
            if(Input.GetKeyDown(KeyCode.DownArrow))
                changeGravity("Down");
    }
    private void Run()
    {
        Vector3 dir = currentGravityDirection.Perpendicular2() * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(-1 / 9.81F * jumpForce * Physics2D.gravity, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 lu = new Vector2(0, 0);
        Vector2 rd = new Vector2(0, 0);
        if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.min.y);
            rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.min.y - accuracyCheckArea);
        } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.max.y + accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.max.y);
        } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            lu = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.max.y - accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.max.x + accuracyCheckArea, playerCollider.bounds.min.y + accuracyCheckArea);
        } else {
            lu = new Vector2(playerCollider.bounds.min.x - accuracyCheckArea, playerCollider.bounds.max.y - accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y + accuracyCheckArea);
        }
        Collider2D[] collider = Physics2D.OverlapAreaAll(lu, rd);
        if (collider.Length > 1){
            if (instantJump){
                Jump();
                jumpsBuffer = jumps - 1;
                instantJump = false;
            } else {
                jumpsBuffer = jumps;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 lu = new Vector2(0, 0);
        Vector2 rd = new Vector2(0, 0);
        if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.min.y);
            rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.min.y - accuracyCheckArea);
        } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.max.y + accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.max.y);
        } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            lu = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.max.y - accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.max.x + accuracyCheckArea, playerCollider.bounds.min.y + accuracyCheckArea);
        } else {
            lu = new Vector2(playerCollider.bounds.min.x - accuracyCheckArea, playerCollider.bounds.max.y - accuracyCheckArea);
            rd = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y + accuracyCheckArea);
        }
        Collider2D[] collider = Physics2D.OverlapAreaAll(lu, rd);
        if (collider.Length > 1){
            isGrounded = true;
            jumpsBuffer = jumps;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void changeGravity(string x)
    {
        switch(x)
        {
            case "Up":
                Debug.Log("Up");
                Physics2D.gravity = new Vector3(0, 9.81F);
                transform.Rotate(0.0f, 0.0f, 180.0f - transform.eulerAngles[2]);
                break;
            case "Down":
                Physics2D.gravity = new Vector3(0, -9.81F);
                transform.Rotate(0.0f, 0.0f, 0.0f - transform.eulerAngles[2]);
                break;
            case "Left":
                Physics2D.gravity = new Vector3(-9.81F, 0);
                transform.Rotate(0.0f, 0.0f, -90.0f - transform.eulerAngles[2]);
                break;
            case "Right":
                Physics2D.gravity = new Vector3(9.81F, 0);
                transform.Rotate(0.0f, 0.0f, 90.0f - transform.eulerAngles[2]);
                Debug.Log(transform.eulerAngles);
                break;
        }
        currentGravityDirection = Physics2D.gravity / 9.81F;
    }
}
