using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    int routePosition;
    int goalPosition;
    int steps;
    bool isMoving;
    public static List<Transform> childNodeCall;
    public static List<Transform> childNodeGoal;
    public static List<Transform> childNodeList;
    public static int diceNumber1;
    public static int diceNumber2;
    public static int diceNumber;

    // Start is called before the first frame update
    void Start () 
    {
        routePosition = -1;
    }

    void OnMouseDown()
    {
        // Destroy the gameObject after clicking on it
        Debug.Log("Mouse Down");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isMoving)
        {
            if(routePosition + diceNumber1 <= childNodeList.Count){steps = diceNumber1;}
            else{steps = -1;}
            StartCoroutine(Move(diceNumber1));
        }
        if(Input.GetKeyDown(KeyCode.RightShift) && !isMoving)
        {
            if(routePosition + diceNumber2 <= childNodeList.Count){steps = diceNumber2;}
            else{steps = -1;}
            StartCoroutine(Move(diceNumber2));
        }
    }

    IEnumerator Move(int dice)
    {
        if(isMoving){yield break;} // Save memory consumption
        isMoving = true;
        while(steps != 0)
        {
            if(routePosition < childNodeList.Count)
            {
                if(routePosition == -1)
                {
                    if(dice >= 5 || diceNumber1 + diceNumber2 == 5)
                    {
                        Vector3 nextPos = childNodeCall[0].position;
                        while(MoveToNextNode(nextPos)){yield return null;}   
                        yield return new WaitForSeconds(0.1f); 
                        routePosition++;
                    }
                    steps = 0;
                }
                else
                {
                    if(steps == -1){steps = 0;}
                    else
                    {
                        Vector3 nextPos = childNodeList[routePosition].position;
                        while(MoveToNextNode(nextPos)){yield return null;}
                        yield return new WaitForSeconds(0.1f);
                        steps--;
                        routePosition++;
                    }
                }
            }
            else
            {
                goalPosition = routePosition;
                // 14: [0 -> 5] ; 28: [6 -> 11] ; 42: [12 -> 17]; 56: [18 -> 23]
                Vector3 nextPos = childNodeGoal[dice + 6 * (goalPosition / 14 - 1) - 1].position;
                
                while(MoveToNextNode(nextPos)){yield return null;}
                yield return new WaitForSeconds(0.1f);
                steps = 0;
            }
        }
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 1000f * Time.deltaTime));
    }
}
