using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;    // The target for the camera to look at
    public float damping = 5.0f; // The damping factor for smooth rotation

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }

        // Calculate the desired rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);

        // Smoothly rotate towards the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, damping * Time.deltaTime);
    }
}
