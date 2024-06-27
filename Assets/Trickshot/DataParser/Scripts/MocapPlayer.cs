using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using RootMotion.FinalIK;

public class MocapPlayer : MonoBehaviour
{
    public bool autoHeight;
    public float heightInCm = 180;

    [Space]
    public HawkEyeParse hawkEye;
    public SOBoneMap map;
    public int playerIndex;
    public bool useRootPosition = false;

    [Space]
    public Animator animator;
    public FullBodyBipedIK ik;
    public LookAtIK look;

    [System.Serializable]
    public class MocapPoint
    {
        public HumanBodyBones bone;
        public Vector3 position;
        public Transform target;

        public Vector3 GetLocalPosition(MocapPoint hips)
        {
            Vector3 root = hips.position;

            if (this.bone == HumanBodyBones.Hips)
            {
                return new Vector3(0, root.y, 0);
            }
            else
            {
                Vector3 pos = (this.position - root);
                pos = new Vector3(pos.x, pos.y + root.y, pos.z);

                return pos;
            }
        }
    }

    public List<MocapPoint> points = new List<MocapPoint>();

    public MocapPoint GetRoot()
    {
        foreach(MocapPoint point in points)
        {
            if (point.bone == HumanBodyBones.Hips) return point;
        }

        return null;
    }

    
    public MocapPoint GetPoint(HumanBodyBones bone)
    {
        foreach (MocapPoint point in points)
        {
            if (point.bone == bone) return point;
        }

        return null;
    }
    

    /*
    public MocapPoint GetPoint(HumanBodyBones bone)
    {
        string key = bone.ToString() + "_" + playerIndex;
        if (hawkEye.pointDictionary.TryGetValue(key, out Point point))
        {
            return point;
        }
        return null;
    }
    */

    public float smoothingFactor = 0.1f;
    public bool initiated = false;
    public Transform ground;

    public List<HawkEyeParse.Point> updatePoints;

    private void Update()
    {
        if (hawkEye.hawkEyeFiles.isReady == false) return;

        if (initiated)
        {
            // Calculate the rotation towards the forward direction
            Quaternion targetRotation = Quaternion.LookRotation(GetForwardDirection());

            // Only apply rotation on the Y axis
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            // Apply the rotation to the game object
            ik.transform.rotation = targetRotation;

            updatePoints = hawkEye.GetPlayerPoints(playerIndex);
        }
    }

    private void FixedUpdate()
    {
        if (hawkEye.hawkEyeFiles.isReady == false) return;

        
        //updatePoints = hawkEye.GetPlayerPoints(playerIndex);

        // Initiate
        if (this.points == null || this.points.Count == 0 && initiated == false)
        {
            updatePoints = hawkEye.GetPlayerPoints(playerIndex);

            this.points = new List<MocapPoint>();

            foreach (HawkEyeParse.Point point in updatePoints)
            {
                HumanBodyBones b = map.NameToBone(point.name);

                if (b != HumanBodyBones.LastBone)
                {
                    MocapPoint mocap = new MocapPoint();
                    mocap.position = point.vector;
                    mocap.bone = b;
                    mocap.target = new GameObject().transform;
                    mocap.target.gameObject.name = b.ToString();
                    mocap.target.SetParent(transform);
                    //mocap.target.localPosition = mocap.
                    this.points.Add(mocap);
                }

            }

            InitiateIK();
        }

        // Update
        else
        {
            foreach (HawkEyeParse.Point point in updatePoints)
            {
                HumanBodyBones b = map.NameToBone(point.name);

                if (b != HumanBodyBones.LastBone)
                {
                    MocapPoint mocapPoint = GetPoint(b);
                    if (mocapPoint != null)
                    {
                        mocapPoint.position = point.vector;
                        MocapPoint rootPoint = GetRoot();
                        if (rootPoint != null)
                        {
                            // Smooth the transition to the new position
                            Vector3 targetLocalPosition = mocapPoint.GetLocalPosition(rootPoint);

                            if(ground != null && targetLocalPosition.y <= ground.position.y)
                            {
                                targetLocalPosition = new Vector3(targetLocalPosition.x, ground.position.y+0.5f, targetLocalPosition.z);
                            }

                            mocapPoint.target.localPosition = Vector3.Lerp(mocapPoint.target.localPosition, targetLocalPosition, smoothingFactor);
                        }
                    }
                }
            }

            if (useRootPosition)
            {
                transform.position = new Vector3(GetRoot().position.x, transform.position.y, GetRoot().position.z);
                    //GetRoot().position;
            }
            else
            {
                
            }
           
        }
    }

    /*
    public Vector3 leftFootOffset;

    public Transform leftToe;

    public Transform leftFoot;

    */
    /*
    private void LateUpdate()
    {
        if (initiated == false) return;

        



        /*
        leftToe = GetPoint(HumanBodyBones.LeftToes).target;
        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);

        //Get 'leftFoot' to look at 'leftToe', with rotational offsets incase the forward direction is incorrect

        // Make the left foot look at the left toe
        leftFoot.LookAt(leftToe);

        // Apply rotational offsets if necessary
        leftFoot.Rotate(leftFootOffset); // Adjust the values as needed
        */
    //}



    public Vector3 GetForwardDirection()
    {
        //Vector3 hip = GetPoint(HumanBodyBones.Hips).position;
        Vector3 hipLeft = GetPoint(HumanBodyBones.LeftUpperLeg).position;
        Vector3 hipRight = GetPoint(HumanBodyBones.RightUpperLeg).position;

        Vector3 hipDirection = hipRight - hipLeft;
        Vector3 upDirection = Vector3.up;
        Vector3 forwardDirection = Vector3.Cross(hipDirection, upDirection).normalized;

        return forwardDirection;
    }

    private void OnDrawGizmos()
    {
        Vector3 forwardDirection = GetForwardDirection();
        Vector3 localForwardDirection = transform.TransformDirection(forwardDirection);

        // Draw the arrow
        Gizmos.color = Color.white;
        GizmosExtensions.DrawArrow(transform.position, localForwardDirection * 1f);
        GizmosExtensions.DrawWireCircle(transform.position, 0.5f);

        // Draw the points
        foreach (MocapPoint point in points)
        {
            Gizmos.color = Color.red;

            if (point.bone == HumanBodyBones.LeftHand || point.bone == HumanBodyBones.RightHand || point.bone == HumanBodyBones.LeftFoot || point.bone == HumanBodyBones.RightFoot)
            {
                Gizmos.color = Color.yellow;
            }

            if(point.bone == HumanBodyBones.Head)
            {
                Gizmos.color = Color.yellow;
            }

            //Vector3 localPosition = transform.TransformPoint(point.GetLocalPosition(GetRoot()));
            GizmosExtensions.DrawWireSphere(point.target.position, 0.05f);
        }
    }

    void InitiateIK()
    {
        // -------- left arm
        ik.solver.leftHandEffector.target = GetPoint(HumanBodyBones.LeftHand).target;
        ik.solver.leftHandEffector.positionWeight = 1;

        ik.solver.leftShoulderEffector.target = GetPoint(HumanBodyBones.LeftShoulder).target;
        ik.solver.leftShoulderEffector.positionWeight = 1;

        /*
        IKConstraintBend leftElbo = ik.solver.GetBendConstraint(FullBodyBipedChain.LeftArm);
        leftElbo.bendGoal = GetPoint(HumanBodyBones.LeftLowerArm).target;
        leftElbo.weight = 1;
        */


        // -------- right arm
        ik.solver.rightHandEffector.target = GetPoint(HumanBodyBones.RightHand).target;
        ik.solver.rightHandEffector.positionWeight = 1;

        ik.solver.rightShoulderEffector.target = GetPoint(HumanBodyBones.RightShoulder).target;
        ik.solver.rightShoulderEffector.positionWeight = 1;

        /*
        IKConstraintBend rightElbo = ik.solver.GetBendConstraint(FullBodyBipedChain.RightArm);
        rightElbo.bendGoal = GetPoint(HumanBodyBones.RightLowerArm).target;
        rightElbo.weight = 1;
        */


        // -------- left leg
        ik.solver.leftFootEffector.target = GetPoint(HumanBodyBones.LeftFoot).target;
        ik.solver.leftFootEffector.positionWeight = 1;

        ik.solver.leftThighEffector.target = GetPoint(HumanBodyBones.LeftUpperLeg).target;
        ik.solver.leftThighEffector.positionWeight = 1;

        
        IKConstraintBend leftKnee = ik.solver.GetBendConstraint(FullBodyBipedChain.LeftLeg);
        leftKnee.bendGoal = GetPoint(HumanBodyBones.LeftLowerLeg).target;
        leftKnee.weight = 1;
        


        // -------- right leg
        ik.solver.rightFootEffector.target = GetPoint(HumanBodyBones.RightFoot).target;
        ik.solver.rightFootEffector.positionWeight = 1;

        ik.solver.rightThighEffector.target = GetPoint(HumanBodyBones.RightUpperLeg).target;
        ik.solver.rightThighEffector.positionWeight = 1;

        
        IKConstraintBend rightKnee = ik.solver.GetBendConstraint(FullBodyBipedChain.RightLeg);
        rightKnee.bendGoal = GetPoint(HumanBodyBones.RightLowerLeg).target;
        rightKnee.weight = 1;
        


        //look.solver.target = GetPoint(HumanBodyBones.Head).target;

        initiated = true;

        
    }

    [ContextMenu("DoHeight")]
    public void DoHeight()
    {
        StartCoroutine(AutoHeight());
    }

    IEnumerator AutoHeight()
    {
        yield return new WaitForSeconds(0.1f);

        //ik.transform.localScale = Vector3.one * 0.5f;

        float closest = 100;
        float size = ik.transform.localScale.x;

        for(int i = 90; i < 110; i++)
        {
            ik.transform.localScale = Vector3.one * (i / 100f);
            //size = ik.transform.localScale.x;

            Vector3 leftKneeActual = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position;
            Vector3 leftKneeRef = GetPoint(HumanBodyBones.LeftLowerLeg).target.position;
            float dist = Vector3.Distance(leftKneeActual, leftKneeRef);

            if(dist < closest)
            {
                closest = dist;
                size = ik.transform.localScale.x;

            }

            Debug.Log("Distance; " + dist + ", Size; " + size + ", Closest; " + closest);

            yield return new WaitForSeconds(0.1f);
        }

        ik.transform.localScale = new Vector3(size - 0.05f, size - 0.05f, size - 0.05f);
    }


}
