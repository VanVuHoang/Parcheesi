using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    static Rigidbody rb;
    public static Vector3 diceVelocity;
    public static float diceRotation;
    public static int tokenIndex;

    // Start is called before the first frame update
    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        tokenIndex = -2;
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = rb.velocity;
        diceRotation = rb.transform.rotation.eulerAngles.x + rb.transform.rotation.eulerAngles.y + rb.transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode. Space) && diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        { 
            if(DiceNumberTextScript.diceNumber1 == DiceNumberTextScript.diceNumber2 && diceRotation >= 0.00012){tokenIndex--;} // Only at the start of the game
            float dirX = Random.Range(600, 700); 
            float dirY = Random.Range(600, 700); 
            float dirZ = Random.Range(600, 700); 
            transform.position = new Vector3(0, 4, -30); 
            transform.rotation = Quaternion.identity; 
            float force = Random.Range(600, 700); 
            rb.AddForce(transform.up * force);
            rb.AddTorque(dirX, dirY, dirZ);
            tokenIndex++;
            Token.tokenIndex = (tokenIndex % 8) / 2;
            Token.isRollingLeft = false;
            Token.isRollingRight = false;
        }
    }
}
