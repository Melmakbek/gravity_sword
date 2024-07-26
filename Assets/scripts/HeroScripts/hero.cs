using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class hero : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int jumps = 2;
    private bool rotating = false;
    private float rotateDuration = 0.2f;
    private int jumpsBuffer = 2;
    private float jumpCheckArea = 0.5f;
    private float boxCheckArea = 0.7f;
    private float accuracyCheckArea = 0.05f;
    private bool instantJump = false;
    private bool isGrounded = false;
    private bool isCarryingBox = false;
    private BoxCollider2D boxCollider = null;
    private Vector2 currentGravityDirection = new Vector2(0, -1);
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D playerCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(Input.GetButton("Horizontal"))
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
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
                Collider2D[] colliders;
                if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
                    colliders = GetCollidersOnSide("Down", jumpCheckArea);
                } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
                    colliders = GetCollidersOnSide("Up", jumpCheckArea);
                } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
                    colliders = GetCollidersOnSide("Right", jumpCheckArea);
                } else {
                    colliders = GetCollidersOnSide("Left", jumpCheckArea);
                }
                if (colliders.Length > 1)
                    instantJump = true;
            }
        }
        if(Input.GetKeyDown("e")){
            Debug.Log(isCarryingBox);
            if(isCarryingBox){
                isCarryingBox = false;
                boxCollider.transform.SetParent(null);
                boxCollider = null;
            } else {
                Collider2D[] colliders;
                if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
                    if(sprite.flipX){
                        colliders = GetCollidersOnSide("Left", boxCheckArea);
                    } else {
                        colliders = GetCollidersOnSide("Right", boxCheckArea);
                    }
                } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
                    if(sprite.flipX){
                        colliders = GetCollidersOnSide("Right", boxCheckArea);
                    } else {
                        colliders = GetCollidersOnSide("Left", boxCheckArea);
                    }
                } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
                    if(sprite.flipX){
                        colliders = GetCollidersOnSide("Down", boxCheckArea);
                    } else {
                        colliders = GetCollidersOnSide("Up", boxCheckArea);
                    }
                } else {
                    if(sprite.flipX){
                        colliders = GetCollidersOnSide("Up", boxCheckArea);
                    } else {
                        colliders = GetCollidersOnSide("Down", boxCheckArea);
                    }
                }
                for(int i = 0; i < colliders.Length; i++){
                    if(colliders[i].tag == "Box"){
                        boxCollider = (BoxCollider2D)colliders[i];
                        break;
                    }
                }
                if (boxCollider != null){
                    isCarryingBox = true;
                }
            }
        }
        if(Input.GetButton("Vertical"))
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                changeGravity("Up");
            }   
            if(Input.GetKeyDown(KeyCode.DownArrow))
                changeGravity("Down");
        if(isCarryingBox){
            boxCollider.gameObject.transform.SetParent(playerCollider.gameObject.transform);
            if(sprite.flipX){
                boxCollider.transform.SetLocalPositionAndRotation(new Vector3( - playerCollider.size.x / 2 - boxCollider.size.x / 2 - accuracyCheckArea, 0, 0), Quaternion.Euler(0, 0, 0));
            } else {
                boxCollider.transform.SetLocalPositionAndRotation(new Vector3( + playerCollider.size.x / 2 + boxCollider.size.x / 2 + accuracyCheckArea, 0, 0), Quaternion.Euler(0, 0, 0));
            }
            // if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            //     if(sprite.flipX){
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3( - playerCollider.size.x / 2 - boxCollider.size.x / 2 - accuracyCheckArea, 0, 0), playerRotation);
            //     } else {
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3( + playerCollider.size.x / 2 + boxCollider.size.x / 2 + accuracyCheckArea, 0, 0), playerRotation);
            //     }
            // } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            //     if(sprite.flipX){
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3( + playerCollider.size.x / 2 + boxCollider.size.x / 2 + accuracyCheckArea, 0, 0), playerRotation);
            //     } else {
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3( - playerCollider.size.x / 2 - boxCollider.size.x / 2 - accuracyCheckArea, 0, 0), playerRotation);
            //     }
            // } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            //     if(sprite.flipX){
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3(0,  - playerCollider.size.x / 2 - boxCollider.size.y / 2 - accuracyCheckArea, 0), playerRotation);
            //     } else {
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3(0,  + playerCollider.size.x / 2 + boxCollider.size.y / 2 + accuracyCheckArea, 0), playerRotation);
            //     }
            // } else {
            //     if(sprite.flipX){
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3(0,  + playerCollider.size.x / 2 + boxCollider.size.y / 2 + accuracyCheckArea, 0), playerRotation);
            //     } else {
            //         boxCollider.transform.SetLocalPositionAndRotation(new Vector3(0,  - playerCollider.size.x / 2 - boxCollider.size.y / 2 - accuracyCheckArea, 0), playerRotation);
            //     }
            // }
        }
    }
    private void Run()
    {
        Vector3 dir = currentGravityDirection.Perpendicular2() * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            sprite.flipX = dir.x < 0.0f;
        } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            sprite.flipX = dir.x > 0.0f;
        } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            sprite.flipX = dir.y < 0.0f;
        } else {
            sprite.flipX = dir.y > 0.0f;
        }
    }

    private void Jump()
    {   
        rb.velocity = Vector2.zero;
        rb.AddForce(-1 / 9.81F * jumpForce * Physics2D.gravity, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] colliders;
        if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            colliders = GetCollidersOnSide("Down", accuracyCheckArea);
        } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            colliders = GetCollidersOnSide("Up", accuracyCheckArea);
        } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            colliders = GetCollidersOnSide("Right", accuracyCheckArea);
        } else {
            colliders = GetCollidersOnSide("Left", accuracyCheckArea);
        }
        if (colliders.Length > 1){
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
        Collider2D[] colliders;
        if (Physics2D.gravity.x == 0 && Physics2D.gravity.y < 0){
            colliders = GetCollidersOnSide("Down", accuracyCheckArea);
        } else if (Physics2D.gravity.x == 0 && Physics2D.gravity.y > 0) {
            colliders = GetCollidersOnSide("Up", accuracyCheckArea);
        } else if (Physics2D.gravity.x > 0 && Physics2D.gravity.y == 0) {
            colliders = GetCollidersOnSide("Right", accuracyCheckArea);
        } else {
            colliders = GetCollidersOnSide("Left", accuracyCheckArea);
        }
        if (colliders.Length > 1){
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
        if (!rotating){
            switch(x)
            {
                case "Up":
                    Physics2D.gravity = new Vector3(0, 9.81F);
                    StartCoroutine(rotateTo(Quaternion.Euler(0.0f, 0.0f, 180.0f)));
                    break;
                case "Down":
                    Physics2D.gravity = new Vector3(0, -9.81F);
                    StartCoroutine(rotateTo(Quaternion.Euler(0.0f, 0.0f, 0.0f)));
                    break;
                case "Left":
                    Physics2D.gravity = new Vector3(-9.81F, 0);
                    StartCoroutine(rotateTo(Quaternion.Euler(0.0f, 0.0f, -90.0f)));
                    break;
                case "Right":
                    Physics2D.gravity = new Vector3(9.81F, 0);
                    StartCoroutine(rotateTo(Quaternion.Euler(0.0f, 0.0f, 90.0f)));
                    break;
            }
            currentGravityDirection = Physics2D.gravity / 9.81F;
        }
    }

    IEnumerator rotateTo(Quaternion targetRotation){
        rotating = true;
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        while (timeElapsed < rotateDuration){
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotateDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        rotating = false;
    }
    private Collider2D[] GetCollidersOnSide(string side, float checkDepth){
        Vector2 lu = Vector2.zero;
        Vector2 rd = Vector2.zero;
        switch(side){
            case "Down":
                lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.min.y);
                rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.min.y - checkDepth);
                break;
            case "Up":
                lu = new Vector2(playerCollider.bounds.min.x + accuracyCheckArea, playerCollider.bounds.max.y + checkDepth);
                rd = new Vector2(playerCollider.bounds.max.x - accuracyCheckArea, playerCollider.bounds.max.y);
                break;
            case "Right":
                lu = new Vector2(playerCollider.bounds.max.x, playerCollider.bounds.max.y - accuracyCheckArea);
                rd = new Vector2(playerCollider.bounds.max.x + checkDepth, playerCollider.bounds.min.y + accuracyCheckArea);
                break;
            case "Left":
                lu = new Vector2(playerCollider.bounds.min.x - checkDepth, playerCollider.bounds.max.y - accuracyCheckArea);
                rd = new Vector2(playerCollider.bounds.min.x, playerCollider.bounds.min.y + accuracyCheckArea);
                break;
        }
        Collider2D[] colliders = Physics2D.OverlapAreaAll(lu, rd);
        return colliders;
    }
}
