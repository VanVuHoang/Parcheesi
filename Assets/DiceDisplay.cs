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
    int tokenIndex; string token; string txt; int num; public static int ber;

    // Start is called before the first frame update
    void Start()
    {
        num = ber; ber++;
        diceText = GetComponent<TMP_Text>();
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = DiceRoll.diceVelocity;
        tokenIndex = DiceRoll.tokenIndex % 8;

        switch (tokenIndex)
        {
            case 0: token = "Red"; break;
            case 2: token = "Green"; break;
            case 4: token = "Yellow"; break;
            case 6: token = "Blue"; break;
        }

        txt = "Turn: " + (token).ToString();
        if(Token.firstRolled == true){txt += "\nDice #1 is used.";}
        if(Token.secondRolled == true){txt += "\nDice #2 is used.";}
    }

    // Update is called once per frame
    void Update()
    {
        if(diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f && DiceRoll.gameStarted)
        {
            if(num == 0){diceText.text = "Token: " + (Token.index + 1).ToString() + "\nDice #1: " + (diceNumber1).ToString() + "\nDice #2: " + (diceNumber2).ToString();}
            if(num == 1){diceText.text = txt;}
            Token.diceNumber1 = diceNumber1; Token.diceNumber2 = diceNumber2;
        }
        else
        {
            if(num == 0){diceText.text = "Token: " + (Token.index + 1).ToString() + "\nDice #1: ?" + "\nDice #2: ?";}
            if(num == 1){diceText.text = txt;}
            Token.diceNumber1 = 0; Token.diceNumber2 = 0;
            Token.firstRolled = false; Token.secondRolled = false;
        }
    }
}
