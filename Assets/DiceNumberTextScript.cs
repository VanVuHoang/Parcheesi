using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiceNumberTextScript : MonoBehaviour
{
    Vector3 diceVelocity;
    TMP_Text diceText;
    public static int diceNumber1;
    public static int diceNumber2;
    float diceRotation;
    int tokenIndex;
    string token;
    string diceUsage;

    // Start is called before the first frame update
    void Start()
    {
        diceText = GetComponent<TMP_Text>();
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = DiceScript.diceVelocity;
        diceRotation = DiceScript.diceRotation;
        tokenIndex = DiceScript.tokenIndex % 8;

        switch (tokenIndex)
        {
            case -2:
                token = "Red";
                break;
            case 0:
                token = "Red";
                break;
            case 2:
                token = "Green";
                break;
            case 4:
                token = "Yellow";
                break;
            case 6:
                token = "Blue";
                break;
        }

        diceUsage = "";
        if(Token.isRollingLeft == true)
        {
            diceUsage += "\nLeft dice was used.";
        }
        if(Token.isRollingRight == true)
        {
            diceUsage += "\nRight dice was used.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && diceRotation >= 0.00012)
        {
            diceText.text = "Left dice: " + (diceNumber1).ToString() + "\nRight dice: " + (diceNumber2).ToString() + "\nTurn: " + (token).ToString() + diceUsage;
            Token.diceNumber1 = diceNumber1;
            Token.diceNumber2 = diceNumber2;
        }
        else
        {
            diceText.text = "Left dice: ?" + "\nRight dice: ?" + "\nTurn: " + (token).ToString();
            Token.diceNumber1 = 0;
            Token.diceNumber2 = 0;
            Token.isRollingLeft = false;
            Token.isRollingRight = false;
        }
    }
}
