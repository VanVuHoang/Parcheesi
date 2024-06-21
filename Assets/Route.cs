using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();

    void OnDrawGizmos()
    {

        FillNodes();

        Gizmos.color = Color.green;

        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 currentPos = childNodeList[i].position;
            if(i > 0)
            {
                Vector3 prevPos = childNodeList[i-1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }

        switch (childNodeList.Count)
        {
            case 4:
                Token.childNodeCall = childNodeList;
                break;
            case 24:
                Token.childNodeGoal = childNodeList;
                break;
            case 56:
                Token.childNodeList = childNodeList;
                break;
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();

        childObjects = GetComponentsInChildren<Transform>();

        foreach(Transform child in childObjects)
        {
            if(child != this.transform)
            {
                childNodeList.Add(child);
            }
        }
    }
}
