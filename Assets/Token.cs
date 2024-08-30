using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    Transform[] tokens; Renderer[] rends;
    bool isMoving; int steps; int token;
    public static bool firstRolled; public static bool secondRolled;
    public static List<int> routePosition = new List<int>();  
    public List<Transform> tokenTransform = new List<Transform>();
    public List<Renderer> tokenRenderer = new List<Renderer>();
    public static List<Transform> childNodeSpawnCall; public static List<Transform> childNodeGoal; public static List<Transform> childNodeList;
    public static int diceNumber1; public static int diceNumber2;
    public static int tokenIndex; public static int index;

    // Start is called before the first frame update
    void Start() 
    {    
        for(int i = 1; i < 5; i++){for(int j = 0; j < 4; j++){Token.routePosition.Add(-i);}}  

        tokens = GetComponentsInChildren<Transform>();
        foreach(Transform child in tokens){if(child != this.transform){tokenTransform.Add(child);}}

        rends = GetComponentsInChildren<Renderer>();
        foreach(Renderer child in rends){if(child != this.GetComponent<Renderer>()){tokenRenderer.Add(child);}}
    }

    // Update is called once per frame
    void Update()
    {
        // Each dice can only be used once per turn
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
        // Token choice
        if(Input.GetKeyDown(KeyCode.Alpha1)){index = 0;}
        if(Input.GetKeyDown(KeyCode.Alpha2)){index = 1;}
        if(Input.GetKeyDown(KeyCode.Alpha3)){index = 2;}
        if(Input.GetKeyDown(KeyCode.Alpha4)){index = 3;}
    }

    // Fixed update is called in intervals
    void FixedUpdate()
    {
        token = 4 * tokenIndex + index;
        // Highlight
        for(int i = 0; i < 4; i++){tokenRenderer[i].material.color = Color.red;} 
        for(int i = 4; i < 8; i++){tokenRenderer[i].material.color = Color.green;} 
        for(int i = 8; i < 12; i++){tokenRenderer[i].material.color = Color.yellow;} 
        for(int i = 12; i < 16; i++){tokenRenderer[i].material.color = Color.blue;} 
        tokenRenderer[token].material.color = Color.cyan;
    }

    IEnumerator Move(int diceValue)
    {
        isMoving = true;
        steps = diceValue;

        // Prevention
        for(int i = 0; i < 16; i++)
        {   
            if(routePosition[token] >= 0 || routePosition[token] <= -5)
            {
                int a = routePosition[token] - 56 * (int)(routePosition[token] / 56);
                if(a <= -5){a = -14 * (routePosition[token] + 5);}
                int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                int c = (a + steps) - 56 * (int)((a + steps) / 56);
                if(routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[token] < 56 + 14 * tokenIndex && i != token)
                {
                    // Prevent overflow of like-colored token 
                    if((int)((routePosition[token] + steps) / (56 + 14 * tokenIndex)) * ((routePosition[token] + steps) % (56 + 14 * tokenIndex)) > 0) 
                    {
                        c = a + ((55 + 14 * tokenIndex) % 56 - a) - (steps - ((55 + 14 * tokenIndex) % 56 - a) - 2);
                        if(c < b && a > b){steps = -1;} 
                        if(c == b && (int)(i / 4) == (int)(token / 4)){steps = -1;} 
                    }
                    else
                    {
                        // Prevent bypass of token
                        if(c > b && a < b || c > b && a + steps > 56){steps = -1;} 
                        // Prevent more than one like-colored token on a node
                        if(c == b && a < b && (int)(i / 4) == (int)(token / 4) || c == b && a + steps > 56 && (int)(i / 4) == (int)(token / 4)){steps = -1;} 
                    }
                }
            }
            // Prevent more than one like-colored token being called
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

        // Movement
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
                        steps = 0; routePosition[token] -= 4;
                        if(diceValue < 5){firstRolled = true; secondRolled = true;}
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
                        for(int i = 0; i < 16; i++)
                        {   
                            if(routePosition[i] < 156 && i != token)
                            {
                                int c = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                                int d = b - c + 1;
                                if(c > a && c <= b || d % 56 == 0 && (int)(i / 4) == (int)(token / 4))
                                {
                                    nextPos = childNodeList[a].position;
                                    routePosition[token] = a + 56 * (int)(routePosition[token] / 56);
                                }
                            }
                        }
                    }
                    while(MoveToNextNode(nextPos, token)){yield return null;}
                    yield return new WaitForSeconds(0.1f);
                    steps--; routePosition[token]++;
                }
                else
                {
                    if(steps < diceValue)
                    {
                        // Steps overflow on the home column
                        for(int i = steps; i >= 0; i--)
                        {
                            steps--; routePosition[token]--;
                            Vector3 nextPos = childNodeList[routePosition[token] - 56 * (int)(routePosition[token] / 56)].position;
                            while(MoveToNextNode(nextPos, token)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        steps++; routePosition[token]++;
                    }
                    else
                    {
                        // Prevent more than one like-colored token on a node
                        for(int i = 0; i < 16; i++)
                        {   
                            for(int j = diceValue; j > (int)(routePosition[token] / 100); j--)
                            {
                                if(routePosition[i] == routePosition[token] % 100 + 100 * j){diceValue = (int)(routePosition[token] / 100); steps = -1;}
                            }
                        }
                        // Prevent backward movement
                        if(routePosition[token] < routePosition[token] % 100 + 100 * diceValue)
                        {
                            routePosition[token] = routePosition[token] % 100 + 100 * diceValue;
                            Vector3 nextPos = childNodeGoal[diceValue + 6 * (int)((routePosition[token] % 100) / 14) - 25].position;
                            while(MoveToNextNode(nextPos, token)){yield return null;}
                            yield return new WaitForSeconds(0.1f);
                        }
                        else{steps = -1;}
                        if(steps > -1){steps = 0;}
                    }
                }
            }
        }

        // Check elimination and victory
        if(steps == 0)
        {
            for(int i = 0; i < 16; i++)
            {    
                // Elimination
                int a = routePosition[token] - 56 * (int)(routePosition[token] / 56);
                int b = routePosition[i] - 56 * (int)(routePosition[i] / 56);
                if(a == b && i != token && routePosition[i] >= 0 && routePosition[i] < 156 && routePosition[token] >= 0 && routePosition[token] < 156)
                {
                    Vector3 nextPos = childNodeSpawnCall[(int)(i / 4) + i].position;
                    while(MoveToNextNode(nextPos, i)){yield return null;}   
                    yield return new WaitForSeconds(0.1f); 
                    routePosition[i] = -((int)(i / 4) + 1);
                }
                // Victory
                if(routePosition[i] == 656 + 14 * (int)(i / 4))
                {
                    for(int j = 4 * (int)(i / 4); j < 4 * ((int)(i / 4) + 1); j++)
                    {
                        if(routePosition[j] == 556 + 14 * (int)(i / 4) && j != i)
                        {
                            for(int k = 4 * (int)(i / 4); k < 4 * ((int)(i / 4) + 1); k++)
                            {
                                if(routePosition[k] == 456 + 14 * (int)(i / 4) && k != j && k != i)
                                {
                                    if(routePosition[16 * (int)(i / 4) + 6 - i - j - k] == 356 + 14 * (int)(i / 4)){DiceDisplay.txt = "Winner";}
                                }
                            }
                        }
                    }
                }
            }
        }

        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal, int i)
    {
        return goal != (tokenTransform[i * 2].position = Vector3.MoveTowards(tokenTransform[i * 2].position, goal, 1000f * Time.deltaTime));
    }
}
