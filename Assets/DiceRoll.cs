using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    static Rigidbody rb;
    public static Vector3 diceVelocity;
    public static int tokenIndex;
    public static bool gameStarted;

    // Start is called before the first frame update
    void Start () 
    {
        rb = GetComponent<Rigidbody>();
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode. Space) && diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        { 
            gameStarted = true;
            float dirX = Random.Range(600, 700); 
            float dirY = Random.Range(600, 700); 
            float dirZ = Random.Range(600, 700); 
            transform.position = new Vector3(0, 4, -30); 
            transform.rotation = Quaternion.identity; 
            float force = Random.Range(600, 700); 
            rb.AddForce(transform.up * force); rb.AddTorque(dirX, dirY, dirZ);
            tokenIndex++; Token.tokenIndex = (tokenIndex % 8) / 2;
            Token.firstRolled = false; Token.secondRolled = false;
            if(DiceDisplay.diceNumber1 == DiceDisplay.diceNumber2){tokenIndex--;}
        }
    }
}
