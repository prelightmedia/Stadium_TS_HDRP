using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Bone Map", order = 1)]
public class SOBoneMap : ScriptableObject
{
    [System.Serializable]
    public class Joint
    {
        public string name;
        public HumanBodyBones bone;
        public Transform source;
    }

    [System.Serializable]
    public class Map
    {
        public string name;
    }

    public List<Joint> joints = new List<Joint>();


    [Header("Body")]
    public Map Head = new Map();
    public Map Neck = new Map();
    public Map Chest = new Map();
    public Map UpperChest = new Map();
    public Map Spine = new Map();
    public Map Hips = new Map();

    [Header("Left Arm")]
    public Map LeftShoulder = new Map();
    public Map LeftUpperArm = new Map();
    public Map LeftLowerArm = new Map(); //Elbow
    public Map LeftHand = new Map(); //Wrist
    public Map LeftFinger = new Map();

    [Header("Right Arm")]
    public Map RightShoulder = new Map();
    public Map RightUpperArm = new Map();
    public Map RightLowerArm = new Map(); //Elbow
    public Map RightHand = new Map(); //Wrist
    public Map RightFinger = new Map();

    [Header("Left Leg")]
    public Map LeftUpperLeg = new Map(); //Hip
    public Map LeftLowerLeg = new Map(); //Knee
    public Map LeftFoot = new Map(); //Ankle

    [Header("Right Leg")]
    public Map RightUpperLeg = new Map(); //Hip
    public Map RightLowerLeg = new Map(); //Knee
    public Map RightFoot = new Map(); //Ankle

    /*
    public HumanBodyBones NameToBone(string name)
    {
        return HumanBodyBones.Hips;
    }

    public string BoneToName(HumanBodyBones bone)
    {
        if (bone == HumanBodyBones.Head) return Head.name;
        if (bone == HumanBodyBones.Neck) return Neck.name;
        if (bone == HumanBodyBones.Chest) return Chest.name;
        if (bone == HumanBodyBones.UpperChest) return UpperChest.name;
        if (bone == HumanBodyBones.Spine) return Spine.name;
        if (bone == HumanBodyBones.Hips) return Hips.name;

        if (bone == HumanBodyBones.LeftShoulder) return LeftShoulder.name;
        if (bone == HumanBodyBones.LeftUpperArm) return LeftUpperArm.name;
        if (bone == HumanBodyBones.LeftLowerArm) return LeftLowerArm.name;
        if (bone == HumanBodyBones.LeftHand) return LeftHand.name;

        if (bone == HumanBodyBones.RightShoulder) return RightShoulder.name;
        if (bone == HumanBodyBones.RightUpperArm) return RightUpperArm.name;
        if (bone == HumanBodyBones.RightLowerArm) return RightLowerArm.name;
        if (bone == HumanBodyBones.RightHand) return RightHand.name;

        return "NULL";
    }
    */
}
