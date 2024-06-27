using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointModule : MonoBehaviour
{
    public SOBoneMap map;
    [Space]
    public Transform Hips;
    [Space]
    public Transform LeftHand;
    public Transform LeftShoulder;
    public Transform LeftLowerArm;
    public Transform LeftFinger;
    [Space]
    public Transform RightHand;
    public Transform RightShoulder;
    public Transform RightLowerArm;
    public Transform RightFinger;
    [Space]
    public Transform LeftFoot;
    public Transform LeftLowerLeg;
    public Transform LeftUpperLeg;

    [Space]
    public Transform RightFoot;
    public Transform RightLowerLeg;
    public Transform RightUpperLeg;

    public void Initiate(SOBoneMap map)
    {
        this.map = map;

        foreach(Transform child in transform)
        {
            if (Hips == null && child.name.Contains(map.Hips.name)) Hips = child;

            if (LeftHand == null && child.name.Contains(map.LeftHand.name)) LeftHand = child;
            if (LeftShoulder == null && child.name.Contains(map.LeftShoulder.name)) LeftShoulder = child;
            if (LeftLowerArm == null && child.name.Contains(map.LeftLowerArm.name)) LeftLowerArm = child;
            if (LeftFinger == null && child.name.Contains(map.LeftFinger.name)) LeftFinger = child;

            if (RightHand == null && child.name.Contains(map.RightHand.name)) RightHand = child;
            if (RightShoulder == null && child.name.Contains(map.RightShoulder.name)) RightShoulder = child;
            if (RightLowerArm == null && child.name.Contains(map.RightLowerArm.name)) RightLowerArm = child;
            if (RightFinger == null && child.name.Contains(map.RightFinger.name)) RightFinger = child;

            if (LeftFoot == null && child.name.Contains(map.LeftFoot.name)) LeftFoot = child;
            if (LeftLowerLeg == null && child.name.Contains(map.LeftLowerLeg.name)) LeftLowerLeg = child;
            if (LeftUpperLeg == null && child.name.Contains(map.LeftUpperLeg.name)) LeftUpperLeg = child;
            

            if (RightFoot == null && child.name.Contains(map.RightFoot.name)) RightFoot = child;
            if (RightLowerLeg == null && child.name.Contains(map.RightLowerLeg.name)) RightLowerLeg = child;
            if (RightUpperLeg == null && child.name.Contains(map.RightUpperLeg.name)) RightUpperLeg = child;
        }
    }

    public Transform GetJoint(HumanBodyBones bone)
    {
        if (bone == HumanBodyBones.Hips) return Hips;

        if (bone == HumanBodyBones.LeftHand) return LeftHand;
        if (bone == HumanBodyBones.LeftShoulder) return LeftShoulder;
        if (bone == HumanBodyBones.LeftLowerArm) return LeftLowerArm;
        if (bone == HumanBodyBones.LeftIndexDistal) return LeftFinger;

        if (bone == HumanBodyBones.RightHand) return RightHand;
        if (bone == HumanBodyBones.RightShoulder) return RightShoulder;
        if (bone == HumanBodyBones.RightLowerArm) return RightLowerArm;
        if (bone == HumanBodyBones.RightIndexDistal) return RightFinger;

        if (bone == HumanBodyBones.LeftFoot) return LeftFoot;
        if (bone == HumanBodyBones.LeftLowerLeg) return LeftLowerLeg;
        if (bone == HumanBodyBones.LeftUpperLeg) return LeftUpperLeg;

        if (bone == HumanBodyBones.RightFoot) return RightFoot;
        if (bone == HumanBodyBones.RightLowerLeg) return RightLowerLeg;
        if (bone == HumanBodyBones.RightUpperLeg) return RightUpperLeg;

        return null;
    }

    /*
    public Transform FindBone(HumanBodyBones bone)
    {
        foreach(Transform child in transform)
        {
            if(bone == finger)
        }
    }
    */
}
