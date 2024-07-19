using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();
    public Color color = Color.white;

    void OnDrawGizmos()
    {
        FillNodes();

        color.a = 0;
        Gizmos.color = color;

        // Position
        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 currentPos = childNodeList[i].position;
            if(i > 0)
            {
                Vector3 prevPos = childNodeList[i-1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();
        childObjects = GetComponentsInChildren<Transform>();
        
        // Node list
        foreach(Transform child in childObjects)
        {
            if(child != this.transform)
            {
                childNodeList.Add(child);
            }
        }

        // Token route
        switch (childNodeList.Count)
        {
            case 8:
                Token.childNodeSpawnCall = childNodeList;
                break;
            case 24:
                Token.childNodeGoal = childNodeList;
                break;
            case 56:
                Token.childNodeList = childNodeList;
                break;
        }
    }
}
