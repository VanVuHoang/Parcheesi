using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    int token;
    int routePosition;
    bool isMoving;
    public static List<Transform> childNodeCall;
    public static List<Transform> childNodeGoal;
    public static List<Transform> childNodeList;
    public static int diceNumber1;
    public static int diceNumber2;
    public static int diceNumber;

    // Start is called before the first frame update
    void Start() 
    {
        token = Random.Range(0, 3);
        routePosition = 14 * token - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isMoving)
        {
            StartCoroutine(Move(diceNumber1));
        }
        if(Input.GetKeyDown(KeyCode.RightShift) && !isMoving)
        {
            StartCoroutine(Move(diceNumber2));
        }
    }

    IEnumerator Move(int dice)
    {
        isMoving = true;
        int steps = dice;
        while(steps > 0)
        {
            // If token is not in play
            if(routePosition == 14 * token - 1)
            {
                if(dice >= 5 || diceNumber1 + diceNumber2 == 5)
                {
                    Vector3 nextPos = childNodeCall[token].position;
                    while(MoveToNextNode(nextPos)){yield return null;}   
                    yield return new WaitForSeconds(0.1f); 
                    routePosition++;
                }
                steps = 0;
            }
            else
            {
                // If token is not on the home column
                if(routePosition < childNodeList.Count + 14 * token)
                {
                    Vector3 nextPos = childNodeList[routePosition - childNodeList.Count * (int)((routePosition / (childNodeList.Count)))].position;
                    while(MoveToNextNode(nextPos)){yield return null;}
                    yield return new WaitForSeconds(0.1f);
                    steps--;
                    routePosition++;
                }
                else
                {
                    if(steps < dice)
                    {
                        // Steps overflow on the home column
                        for (int i = steps; i >= 0; i--)
                        {
                            steps--;
                            routePosition--;
                            Vector3 nextPos = childNodeList[routePosition - childNodeList.Count * (int)((routePosition / (childNodeList.Count)))].position;
                            while(MoveToNextNode(nextPos)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        routePosition++;
                    }
                    else
                    {
                        int goalPosition = routePosition - childNodeList.Count * (int)((routePosition / (childNodeList.Count)));
                        Vector3 nextPos = childNodeGoal[dice + 6 * (goalPosition / 14) - 1].position;
                        while(MoveToNextNode(nextPos)){yield return null;}
                        yield return new WaitForSeconds(0.1f);
                        steps = 0;
                    }
                }
            }
        }
        isMoving = false; // Outside the loop to prevent interference
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 1000f * Time.deltaTime));
    }
}
