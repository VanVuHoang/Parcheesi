using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    Transform[] tokens;
    bool isMoving;
    public static bool firstRolled;
    public static bool secondRolled;
    public List<int> routePosition = new List<int>();  
    public List<Transform> tokenList = new List<Transform>();
    public static List<Transform> childNodeSpawnCall;
    public static List<Transform> childNodeGoal;
    public static List<Transform> childNodeList;
    public static int diceNumber1;
    public static int diceNumber2;
    public static int tokenIndex;
    public static int index;
    int steps;
    int token;

    // Start is called before the first frame update
    void Start() 
    {    
        routePosition.Add(-1);
        routePosition.Add(-1);
        routePosition.Add(-1);
        routePosition.Add(-1);

        routePosition.Add(-2);
        routePosition.Add(-2);
        routePosition.Add(-2);
        routePosition.Add(-2);

        routePosition.Add(-3);
        routePosition.Add(-3);
        routePosition.Add(-3);
        routePosition.Add(-3);

        routePosition.Add(-4);
        routePosition.Add(-4);
        routePosition.Add(-4);
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
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isMoving && !firstRolled)
        {
            StartCoroutine(Move(diceNumber1));
            if(steps > -1){firstRolled = true;}
        }
        if(Input.GetKeyDown(KeyCode.RightShift) && !isMoving && !secondRolled)
        {
            StartCoroutine(Move(diceNumber2));
            if(steps > -1){secondRolled = true;}
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
        }

        token = 4 * tokenIndex + index;
    }

    IEnumerator Move(int diceValue)
    {
        isMoving = true;
        steps = diceValue;

        // Token prevention
        for (int i = 0; i < 16; i++)
        {   
            if(routePosition[token] >= 0 || routePosition[token] <= -5)
            {
                int a = routePosition[token] - 56 * (int)(routePosition[token] / 56);
                int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                if(a <= -5){a = -14 * (routePosition[token] + 5);}
                if(a >= 51 && a <= 55 && b >= 0 && b <= 5 && routePosition[i] >= 56){b += 56;}
                int c = (a + steps) - 2 * (int)((a + steps) / (56 + 14 * tokenIndex)) * ((a + steps) % (56 + 14 * tokenIndex));
                if(routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[token] < 56 + 14 * tokenIndex && i != token)
                {
                    // Prevent overflown of like-colored token
                    if((int)((a + steps) / (56 + 14 * tokenIndex)) * ((a + steps) % (56 + 14 * tokenIndex)) > 0)
                    {
                        if(c < b){steps = -1;} 
                        if(c == b && (int)(i / 4) == (int)(token / 4)){steps = -1;} 
                    }
                    else
                    {
                        // Prevent bypass of token
                        if(c > b && a < b){steps = -1;} 
                        // Prevent elimination of like-colored token
                        if(c == b && a < b && (int)(i / 4) == (int)(token / 4)){steps = -1;} 
                    }
                }
            }
            // No more than one like-colored token on standby
            else
            {
                if(diceValue >= 5 || firstRolled == false && secondRolled == false)
                {
                    if(diceValue >= 5 || diceNumber1 + diceNumber2 == 5 || diceNumber1 == diceNumber2)
                    {
                        if(routePosition[token] - routePosition[i] == 4){steps = -1;}
                    }
                }
            }
        }

        // Token movement
        while(steps > 0)
        {
            // Token is not in play
            if(routePosition[token] < 0 && routePosition[token] > -5)
            {
                if(diceValue >= 5 || firstRolled == false && secondRolled == false)
                {
                    if(diceValue >= 5 || diceNumber1 + diceNumber2 == 5 || diceNumber1 == diceNumber2)
                    {
                        Vector3 nextPos = childNodeSpawnCall[tokenIndex * 5 + 4].position;
                        while(MoveToNextNode(nextPos, token)){yield return null;}
                        yield return new WaitForSeconds(0.1f);
                        steps = 0;
                        routePosition[token] -= 4;
                        if(diceValue < 5)
                        {
                            firstRolled = true;
                            secondRolled = true;
                        }
                    }
                }
                if(steps > 0){steps = -1;}
            }
            else
            {
                if(routePosition[token] < 0){routePosition[token] = -14 * (routePosition[token] + 5);}
                // Token is not on the home column
                if(routePosition[token] < 56 + 14 * tokenIndex)
                {
                    Vector3 nextPos = childNodeList[routePosition[token] - 56 * (int)(routePosition[token] / 56)].position;
                    if(diceValue == 1)
                    {
                        int a = routePosition[token] - 56 * (int)(routePosition[token] / 56);
                        int b = 14 * (int)(a / 14) + 13;
                        nextPos = childNodeList[b].position;
                        routePosition[token] = b + 56 * (int)(routePosition[token] / 56);
                        for (int i = 0; i < 16; i++)
                        {   
                            if(routePosition[i] < 156 && i != token)
                            {
                                int c = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                                int d = b - c + 1;
                                if(c > a && c <= b || d % 56 == 0 && (int)(i / 4) == (int)(token / 4))
                                {
                                    nextPos = childNodeList[a].position;
                                    routePosition[token] = a;
                                }
                            }
                        }
                    }
                    while(MoveToNextNode(nextPos, token)){yield return null;}
                    yield return new WaitForSeconds(0.1f);
                    steps--;
                    routePosition[token]++;
                }
                else
                {
                    if(steps < diceValue)
                    {
                        // Steps overflow on the home column
                        for (int i = steps; i >= 0; i--)
                        {
                            steps--;
                            routePosition[token]--;
                            Vector3 nextPos = childNodeList[routePosition[token] - 56 * (int)(routePosition[token] / 56)].position;
                            while(MoveToNextNode(nextPos, token)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        steps++;
                        routePosition[token]++;
                    }
                    else
                    {
                        // Prevent token block
                        for (int i = 0; i < 16; i++)
                        {   
                            if(routePosition[i] == routePosition[token] % 100 + 100 * diceValue){diceValue = (int)(routePosition[token] / 100); steps = -1;}
                        }
                        if(routePosition[token] < routePosition[token] % 100 + 100 * diceValue)
                        {
                            routePosition[token] = routePosition[token] % 100 + 100 * diceValue;
                            Vector3 nextPos = childNodeGoal[diceValue + 6 * (int)((routePosition[token] % 100) / 14) - 25].position;
                            while(MoveToNextNode(nextPos, token)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        if(steps > -1){steps = 0;}
                    }
                }
            }
        }

        // Token elimination
        if(steps == 0)
        {
            for (int i = 0; i < 16; i++)
            {    
                int a = routePosition[token] - 56 * (int)(routePosition[token] / 56);
                int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                if(a == b && i != token && routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[token] >= 0 && routePosition[token] < 156)
                {
                    Vector3 nextPos = childNodeSpawnCall[(int)(i / 4) + i].position;
                    while(MoveToNextNode(nextPos, i)){yield return null;}   
                    yield return new WaitForSeconds(0.1f); 
                    routePosition[i] = -((int)(i / 4) + 1);
                }
            }
        }
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal, int index)
    {
        return goal != (tokenList[index * 2].position = Vector3.MoveTowards(tokenList[index * 2].position, goal, 1000f * Time.deltaTime));
    }
}
