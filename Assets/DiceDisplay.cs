using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiceDisplay : MonoBehaviour
{
    Vector3 diceVelocity;
    TMP_Text diceText;
    public static int diceNumber1; public static int diceNumber2;
    int tokenIndex; string token; string diceUsage;

    // Start is called before the first frame update
    void Start()
    {
        diceText = GetComponent<TMP_Text>();
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = DiceRoll.diceVelocity;
        tokenIndex = DiceRoll.tokenIndex % 8;

        switch (tokenIndex)
        {
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
        if(Token.firstRolled == true)
        {
            diceUsage += "\nDice #1 is used.";
        }
        if(Token.secondRolled == true)
        {
            diceUsage += "\nDice #2 is used.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && DiceRoll.gameStarted)
        {
            diceText.text = "Token number: " + (Token.index + 1).ToString() + "\nDice #1: " + (diceNumber1).ToString() + "\nDice #2: " + (diceNumber2).ToString() + "\nTurn: " + (token).ToString() + diceUsage;
            Token.diceNumber1 = diceNumber1;
            Token.diceNumber2 = diceNumber2;
        }
        else
        {
            diceText.text = "Token number: " + (Token.index + 1).ToString() + "\nDice #1: ?" + "\nDice #2: ?" + "\nTurn: " + (token).ToString();
            Token.diceNumber1 = 0;
            Token.diceNumber2 = 0;
            Token.firstRolled = false;
            Token.secondRolled = false;
        }
    }
}
