using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.FinalIK;

public class IKController : MonoBehaviour
{

    [SerializeField] private Animator animator;

    [SerializeField] private Transform leftHip;
    [SerializeField] private Transform rightHip;
    [SerializeField] private Transform midPelvis;

    [SerializeField] private FullBodyBipedIK targetIk;
    [SerializeField] private JointModule joints;

    private void Start()
    {
        //InitiateAsAvatar();

        //AddFBBIK(animator);
    }


    [ContextMenu("InitateIK")]
    public void InitateIK(JointModule jm)
    {
        transform.localPosition = Vector3.zero;

        joints = jm;
        targetIk = AddFBBIK(animator);

        leftHip = jm.GetJoint(HumanBodyBones.LeftUpperLeg);
        rightHip = jm.GetJoint(HumanBodyBones.RightUpperLeg);
        midPelvis = jm.GetJoint(HumanBodyBones.Hips);
    }

    private void InitiateAsAvatar()
    {
        //targetRoot.leftHip = joints.GetJoint("LeftHip");
    }


    FullBodyBipedIK AddFBBIK(Animator animator, BipedReferences references = null)
    {
        if (references == null)
        {
            BipedReferences.AutoDetectReferences(ref references, transform, BipedReferences.AutoDetectParams.Default);
        }

        var ik = gameObject.AddComponent<FullBodyBipedIK>();
        ik.SetReferences(references, animator.GetBoneTransform(HumanBodyBones.Spine));
        ik.solver.SetLimbOrientations(BipedLimbOrientations.UMA);

        // -------- body
        ik.solver.bodyEffector.target = joints.GetJoint(HumanBodyBones.Hips);
        ik.solver.bodyEffector.positionWeight = 1;


        // -------- left arm
        ik.solver.leftHandEffector.target = joints.GetJoint(HumanBodyBones.LeftHand);
        ik.solver.leftHandEffector.positionWeight = 1;

        ik.solver.leftShoulderEffector.target = joints.GetJoint(HumanBodyBones.LeftShoulder); //Maybe UpperArm?
        ik.solver.leftShoulderEffector.positionWeight = 1;

        IKConstraintBend leftElbo = ik.solver.GetBendConstraint(FullBodyBipedChain.LeftArm);
        leftElbo.bendGoal = joints.GetJoint(HumanBodyBones.LeftLowerArm);
        leftElbo.weight = 1;


        // -------- right arm
        ik.solver.rightHandEffector.target = joints.GetJoint(HumanBodyBones.RightHand);
        ik.solver.rightHandEffector.positionWeight = 1;

        ik.solver.rightShoulderEffector.target = joints.GetJoint(HumanBodyBones.RightShoulder);
        ik.solver.rightShoulderEffector.positionWeight = 1;

        IKConstraintBend rightElbo = ik.solver.GetBendConstraint(FullBodyBipedChain.RightArm);
        rightElbo.bendGoal = joints.GetJoint(HumanBodyBones.RightLowerArm);
        rightElbo.weight = 1;


        // -------- left leg
        ik.solver.leftFootEffector.target = joints.GetJoint(HumanBodyBones.LeftFoot);
        ik.solver.leftFootEffector.positionWeight = 1;

        IKConstraintBend leftKnee = ik.solver.GetBendConstraint(FullBodyBipedChain.LeftLeg);
        leftKnee.bendGoal = joints.GetJoint(HumanBodyBones.LeftLowerLeg);
        leftKnee.weight = 1;


        // -------- right leg
        ik.solver.rightFootEffector.target = joints.GetJoint(HumanBodyBones.RightFoot);
        ik.solver.rightFootEffector.positionWeight = 1;

        IKConstraintBend rightKnee = ik.solver.GetBendConstraint(FullBodyBipedChain.RightLeg);
        rightKnee.bendGoal = joints.GetJoint(HumanBodyBones.RightLowerLeg);
        rightKnee.weight = 1;

        return ik;
    }

    public Vector3 posOffset = Vector3.zero;
    

    private void Update()
    {
        // Rotate towards mocap facing
        Vector3 hipDirection = rightHip.position - leftHip.position;
        Vector3 upDirection = Vector3.up; // Global up direction

        // Calculate the forward direction
        Vector3 forwardDirection = Vector3.Cross(hipDirection, upDirection).normalized;

        // Draw the Debug.Ray
        Debug.DrawRay(midPelvis.position, forwardDirection * 5f, Color.red, 0.1f, false);

        // Calculate the target position for LookAt
        Vector3 targetPosition = midPelvis.position + forwardDirection;

        // Rotate the midPelvis to look at the target position
        midPelvis.transform.LookAt(targetPosition);

        transform.localPosition = midPelvis.localPosition + posOffset;
        transform.LookAt(targetPosition);


        // Rotate hands and feet to suitable points
        Transform lFinger = joints.GetJoint(HumanBodyBones.LeftIndexDistal);
    }


}
