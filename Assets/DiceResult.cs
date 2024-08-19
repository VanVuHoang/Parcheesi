using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResult : MonoBehaviour
{
    Vector3 diceVelocity;

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        diceVelocity = DiceRoll.diceVelocity;
    }

    void OnTriggerStay(Collider col)
    {
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        {
            switch (col.gameObject.name)
            {
                case "Side1.1":
                    DiceDisplay.diceNumber1 = 6;
                    break;
                case "Side2.1":
                    DiceDisplay.diceNumber1 = 5;
                    break;
                case "Side3.1":
                    DiceDisplay.diceNumber1 = 4;
                    break;
                case "Side4.1":
                    DiceDisplay.diceNumber1 = 3;
                    break;
                case "Side5.1":
                    DiceDisplay.diceNumber1 = 2;
                    break;
                case "Side6.1":
                    DiceDisplay.diceNumber1 = 1;
                    break;
                case "Side1.2":
                    DiceDisplay.diceNumber2 = 6;
                    break;
                case "Side2.2":
                    DiceDisplay.diceNumber2 = 5;
                    break;
                case "Side3.2":
                    DiceDisplay.diceNumber2 = 4;
                    break;
                case "Side4.2":
                    DiceDisplay.diceNumber2 = 3;
                    break;
                case "Side5.2":
                    DiceDisplay.diceNumber2 = 2;
                    break;
                case "Side6.2":
                    DiceDisplay.diceNumber2 = 1;
                    break;
            }
        }
    }
}
