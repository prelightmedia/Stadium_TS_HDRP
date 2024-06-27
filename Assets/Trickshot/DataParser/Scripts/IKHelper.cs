using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHelper : MonoBehaviour
{
    public Transform leftHip;
    public Transform rightHip;
    public Transform midPelvis;

    // Update is called once per frame
    void Update()
    {
        if (midPelvis == null) return;

        Vector3 hipDirection = rightHip.position - leftHip.position;
        Vector3 upDirection = Vector3.up; // Global up direction

        // Calculate the forward direction
        Vector3 forwardDirection = Vector3.Cross(hipDirection, upDirection).normalized;

        // Draw the Debug.Ray
        Debug.DrawRay(midPelvis.position, forwardDirection * 5f, Color.red, 0.1f, false);

        //transform.LookAt(forwardDirection);
    }
}
