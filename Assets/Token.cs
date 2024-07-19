using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    Transform[] tokens;
    bool isMoving;
    public static bool isRollingLeft;
    public static bool isRollingRight;
    public List<int> routePosition = new List<int>();  
    public List<Transform> tokenList = new List<Transform>();
    public static List<Transform> childNodeSpawnCall;
    public static List<Transform> childNodeGoal;
    public static List<Transform> childNodeList;
    public static int diceNumber1;
    public static int diceNumber2;
    public static int tokenIndex;

    // Start is called before the first frame update
    void Start() 
    {    
        routePosition.Add(-1);
        routePosition.Add(-2);
        routePosition.Add(-3);
        routePosition.Add(-4);

        tokens = GetComponentsInChildren<Transform>();
        foreach(Transform child in tokens)
        {
            if(child != this.transform)
            {
                tokenList.Add(child);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isMoving && !isRollingLeft)
        {
            StartCoroutine(Move(diceNumber1));
            isRollingLeft = true;
        }
        if(Input.GetKeyDown(KeyCode.RightShift) && !isMoving && !isRollingRight)
        {
            StartCoroutine(Move(diceNumber2));
            isRollingRight = true;
        }
    }

    IEnumerator Move(int diceValue)
    {
        isMoving = true;
        int steps = diceValue;

        // Prevent bypassing token
        for (int i = 0; i < 4; i++)
        {    
            int a = routePosition[tokenIndex] - 56 * (int)(routePosition[tokenIndex] / 56);
            int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
            if(a >= 51 && a<= 55 && b >= 0 && b <=5){b += 56;}
            int c = a + steps;
            if(c > b && a < b && routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[tokenIndex] >= 0 && routePosition[tokenIndex] < 56 + 14 * tokenIndex){steps = 0;} 
        }

        // Token movement
        while(steps > 0)
        {
            // Token is not in play
            if(routePosition[tokenIndex] < 0 && routePosition[tokenIndex] > -5)
            {
                if(diceValue >= 5 || diceNumber1 + diceNumber2 == 5)
                {
                    Vector3 nextPos = childNodeSpawnCall[tokenIndex * 2 + 1].position;
                    while(MoveToNextNode(nextPos, tokenIndex)){yield return null;}   
                    yield return new WaitForSeconds(0.1f); 
                    routePosition[tokenIndex] -= 4;
                    if(diceNumber1 + diceNumber2 == 5)
                    {
                        isRollingLeft = true;
                        isRollingRight = true;
                    }
                }
                steps = 0;
            }
            else
            {
                if(routePosition[tokenIndex] < 0){routePosition[tokenIndex] = - 14 * (routePosition[tokenIndex] + 5);}
                // Token is not on the home column
                if(routePosition[tokenIndex] < 56 + 14 * tokenIndex)
                {
                    Vector3 nextPos = childNodeList[routePosition[tokenIndex] - 56 * (int)(routePosition[tokenIndex] / 56)].position;
                    if(diceValue == 1)
                    {
                        int a = routePosition[tokenIndex] - 56 * (int)(routePosition[tokenIndex] / 56);
                        int b = (int)(a / 14) + 13 * ((int)(a / 14) + 1);
                        nextPos = childNodeList[b].position;
                        routePosition[tokenIndex] = b;
                        for (int i = 0; i < 4; i++)
                        {    
                            int c = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                            if(c > a && c < b)
                            {
                                nextPos = childNodeList[a].position;
                                routePosition[tokenIndex] = a;
                            }
                        }
                    }
                    while(MoveToNextNode(nextPos, tokenIndex)){yield return null;}
                    yield return new WaitForSeconds(0.1f);
                    steps--;
                    routePosition[tokenIndex]++;
                }
                else
                {
                    if(steps < diceValue)
                    {
                        // Step overflow on the home column
                        for (int i = steps; i >= 0; i--)
                        {
                            steps--;
                            routePosition[tokenIndex]--;
                            Vector3 nextPos = childNodeList[routePosition[tokenIndex] - 56 * (int)(routePosition[tokenIndex] / 56)].position;
                            while(MoveToNextNode(nextPos, tokenIndex)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        routePosition[tokenIndex]++;
                    }
                    else
                    {
                        if((int)((routePosition[tokenIndex] / 100)) < (int)(((routePosition[tokenIndex] % 100 + 100 * diceValue) / 100)))
                        {
                            routePosition[tokenIndex] += 100 * diceValue;
                            Vector3 nextPos = childNodeGoal[diceValue + 6 * (int)((routePosition[tokenIndex] % 100) / 14) - 25].position;
                            while(MoveToNextNode(nextPos, tokenIndex)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        steps = 0;
                    }
                }
            }
        }

        // Token elimination
        for (int i = 0; i < 4; i++)
        {    
            int a = routePosition[tokenIndex] - 56 * (int)(routePosition[tokenIndex] / 56);
            int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
            if(a == b && i != tokenIndex && routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[tokenIndex] >= 0 && routePosition[tokenIndex] < 156 && steps == 0)
            {
                Vector3 nextPos = childNodeSpawnCall[i * 2].position;
                while(MoveToNextNode(nextPos, i)){yield return null;}   
                yield return new WaitForSeconds(0.1f); 
                routePosition[i] = -(i + 1);
            }
        }
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal, int index)
    {
        return goal != (tokenList[index * 2].position = Vector3.MoveTowards(tokenList[index * 2].position, goal, 1000f * Time.deltaTime));
    }
}
