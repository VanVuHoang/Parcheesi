using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceNumberTextScript : MonoBehaviour
{
    Vector3 diceVelocity;
    TMP_Text diceText;
    public static int diceNumber1;
    public static int diceNumber2;
    float diceRotation;

    // Start is called before the first frame update
    void Start()
    {
        diceText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        diceVelocity = DiceScript.diceVelocity;
        diceRotation = DiceScript.diceRotation;
        // rb.transform.rotation.eulerAngles.(x/y/z) < 0.00001 for static dice
        if(diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && diceRotation >= 0.00003)
        {
            diceText.text = "Left dice: " + (diceNumber1).ToString() + "\nRight dice: " + (diceNumber2).ToString();
            Token.diceNumber1 = diceNumber1;
            Token.diceNumber2 = diceNumber2;
            Token.diceNumber = diceNumber1 + diceNumber2;
        }
        else
        {
            diceText.text = "Left dice: " + "?" + "\nRight dice: " + "?";
            Token.diceNumber1 = 0;
            Token.diceNumber2 = 0;
        }
    }
}
