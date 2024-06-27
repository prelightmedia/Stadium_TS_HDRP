using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool drawRotation;
    public float rotationForwardAmount = .1f;
    public Color mainColor = Color.white;
    public Color forwardColor = Color.green;

    void OnDrawGizmos()
    {
        DrawGizmosOf(transform);
    }

    void DrawGizmosOf(Transform thisNode)
    {
        if (drawRotation)
        {
            Gizmos.color = forwardColor;
            Gizmos.DrawLine(thisNode.position, thisNode.position + thisNode.forward * rotationForwardAmount);
            Gizmos.color = mainColor;
        }

        for (int i = 0; i < thisNode.childCount; i++)
        {
            var thisChild = thisNode.GetChild(i);
            Gizmos.DrawLine(thisNode.position, thisChild.position);
            DrawGizmosOf(thisChild);
        }
    }
}