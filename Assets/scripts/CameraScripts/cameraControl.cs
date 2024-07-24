using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float dumping = 1.5f;
    public Vector2 offset = new Vector2(2f, 1f);
    public bool isLeft = false;
    private Transform player;
    private int lastX;
    private int currentX;
    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        Vector3 target;
        if(playerIsLeft)
        {
            target = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
        }
        else
        {
            target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);

    }
    // Update is called once per frame
    void Update()
    {
        currentX = Mathf.RoundToInt(player.position.x);
        if(currentX > lastX) isLeft = false;
        else if(currentX < lastX) isLeft = true;
        lastX = currentX;
        FindPlayer(isLeft);
    }
}
