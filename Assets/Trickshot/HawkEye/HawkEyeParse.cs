using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class HawkEyeParse : MonoBehaviour
{
   
    public HawkEyeFiles hawkEyeFiles;


    public float currentFrame = 0;
    public float frameRate = 30;
    public int currentIndex = 0;

    public int maxFrame = 500;

    [Header("IK Systems")]
    public SOBoneMap map;
    public GameObject dummy;
    public bool spawn = false;

    private void Awake()
    {
        Application.targetFrameRate = 50;
        hawkEyeFiles.OnReady.AddListener(HandlerOnReady);
    }



    public Dictionary<string, Point> pointDictionary;

    private void HandlerOnReady()
    {
        // Initialize the dictionary firs
        pointDictionary = new Dictionary<string, Point>();

        // Parse the initial data
        string data = hawkEyeFiles.GetJoint(0);
        JSONNode json = JSON.Parse(data);

        for (int i = 0; i < json["samples"]["people"].AsArray.Count; i++)
        {
            Transform player = new GameObject().transform;
            player.SetParent(transform);
            player.name = "Player " + i;

            UpdateJoints(json["samples"]["people"][i]["joints"][0].AsObject, i, player);

            if (spawn)
            {
                //IK avatar mapper
                JointModule jm = player.gameObject.AddComponent<JointModule>();
                jm.Initiate(map);

                IKController iKController = Instantiate(dummy).GetComponent<IKController>();
                iKController.transform.SetParent(player);
                iKController.InitateIK(jm);
            }
        }

        
    }

    private void Start()
    {
       

        /*
        // Initialise the data and create the nodes
        string data = hawkEyeFiles.GetJoint(0);
        JSONNode json = JSON.Parse(data);

        for (int i = 0; i < json["samples"]["people"].AsArray.Count; i++)
        {
            Transform player = new GameObject().transform;
            player.SetParent(transform);
            player.name = "Player " + i;

            UpdateJoints(json["samples"]["people"][i]["joints"][0].AsObject, i, player);

            if (spawn)
            {
                //IK Dummy
                JointModule jm = player.gameObject.AddComponent<JointModule>();
                jm.Initiate(map);

                IKController iKController = Instantiate(dummy).GetComponent<IKController>();
                iKController.transform.SetParent(player);
                //iKController.transform.SetParent(jm.GetJoint(HumanBodyBones.Hips));
                //iKController.transform.localPosition = new Vector3(0, 0, 0);
                //iKController.transform.localEulerAngles = Vector3.zero;
                iKController.InitateIK(jm);
            }
           
        }


        string dataBall = hawkEyeFiles.GetBall(0);
        JSONNode jsonBall = JSON.Parse(dataBall);
        JSONArray vec = jsonBall["samples"]["ball"][0]["pos"].AsArray;

        ball = new Point(vec, "ball", -1, transform);
        ball.node = this.ballTransform;
        this.ballTransform.transform.SetParent(transform);
        */
    }



    private void FixedUpdate()
    {
        if (hawkEyeFiles.isReady == false) return;

        currentFrame += Time.deltaTime * frameRate;
        currentIndex = (int)currentFrame;  // Truncate to avoid skipping frames too early

        if (currentIndex >= maxFrame)
        {
            currentFrame = 0;
            currentIndex = 0;
        }

        string dataJoint = hawkEyeFiles.GetJoint(currentIndex);
        JSONNode jsonJoint = JSON.Parse(dataJoint);

        // Interpolation logic here if necessary
        for (int i = 0; i < jsonJoint["samples"]["people"].AsArray.Count; i++)
        {
            UpdateJoints(jsonJoint["samples"]["people"][i]["joints"][0].AsObject, i);
        }
    }

    /*
    private void FixedUpdate()
    {
        if (hawkEyeFiles.isReady == false) return;

        currentFrame += Time.deltaTime * frameRate;
        currentIndex = Mathf.RoundToInt(currentFrame);

        if (currentFrame >= maxFrame)
        {
            currentFrame = 0;
            currentIndex = 0;
        }

        string dataJoint = hawkEyeFiles.GetJoint(currentIndex);
        if (dataJoint == null) return;

        JSONNode jsonJoint = JSON.Parse(dataJoint);
        JSONArray peopleArray = jsonJoint["samples"]["people"].AsArray;

        for (int i = 0; i < peopleArray.Count; i++)
        {
            UpdateJoints(peopleArray[i]["joints"][0].AsObject, i);
        }

        /*
        if (hawkEyeFiles.isReady == false) return;

        currentFrame += Time.deltaTime * frameRate;
        currentIndex = Mathf.RoundToInt(currentFrame);

        if(currentFrame >= maxFrame)
        {
            currentFrame = 0;
            currentIndex = 0;
        }

        string dataJoint = hawkEyeFiles.GetJoint(currentIndex);
        JSONNode jsonJoint = JSON.Parse(dataJoint);

        
        for(int i = 0; i < jsonJoint["samples"]["people"].AsArray.Count; i++)
        {
            UpdateJoints(jsonJoint["samples"]["people"][i]["joints"][0].AsObject, i);
        }
        */


    //UpdateJoints(jsonJoint["samples"]["people"][0]["joints"][0].AsObject, 0);



    /*
    string dataBall = hawkEyeFiles.GetBall(currentIndex);
    JSONNode jsonBall = JSON.Parse(dataBall);
    JSONArray vec = jsonBall["samples"]["ball"][0]["pos"].AsArray;
    Vector3 vector = new Vector3(vec[0].AsFloat, vec[1].AsFloat, vec[2].AsFloat);

    ball.vector = vector;
    ball.node.localPosition = vector;
    */

    //}

    /*
    void UpdateJoints(JSONObject joints, int playerIndex, Transform player = null)
    {
        JSONArray lAnkle = joints["lAnkle"].AsArray;
        JSONArray lBigToe = joints["lBigToe"].AsArray;
        JSONArray lEar = joints["lEar"].AsArray;
        JSONArray lElbow = joints["lElbow"].AsArray;
        JSONArray lEye = joints["lEye"].AsArray;
        JSONArray lHeel = joints["lHeel"].AsArray;
        JSONArray lHip = joints["lHip"].AsArray;
        JSONArray lKnee = joints["lKnee"].AsArray;
        JSONArray lPinky = joints["lPinky"].AsArray;
        JSONArray lShoulder = joints["lShoulder"].AsArray;
        JSONArray lSmallToe = joints["lSmallToe"].AsArray;
        JSONArray lThumb = joints["lThumb"].AsArray;
        JSONArray lWrist = joints["lWrist"].AsArray;

        JSONArray midHip = joints["midHip"].AsArray;
        JSONArray neck = joints["neck"].AsArray;
        JSONArray nose = joints["nose"].AsArray;

        JSONArray rAnkle = joints["rAnkle"].AsArray;
        JSONArray rBigToe = joints["rBigToe"].AsArray;
        JSONArray rEar = joints["rEar"].AsArray;
        JSONArray rElbow = joints["rElbow"].AsArray;
        JSONArray rEye = joints["rEye"].AsArray;
        JSONArray rHeel = joints["rHeel"].AsArray;
        JSONArray rHip = joints["rHip"].AsArray;
        JSONArray rKnee = joints["rKnee"].AsArray;
        JSONArray rPinky = joints["rPinky"].AsArray;
        JSONArray rShoulder = joints["rShoulder"].AsArray;
        JSONArray rSmallToe = joints["rSmallToe"].AsArray;
        JSONArray rThumb = joints["rThumb"].AsArray;
        JSONArray rWrist = joints["rWrist"].AsArray;

        UpdatePoint(lAnkle, "lAnkle", playerIndex, player);
        UpdatePoint(lBigToe, "lBigToe", playerIndex, player);
        UpdatePoint(lEar, "lEar", playerIndex, player);
        UpdatePoint(lElbow, "lElbow", playerIndex, player);
        UpdatePoint(lEye, "lEye", playerIndex, player);
        UpdatePoint(lHeel, "lHeel", playerIndex, player);
        UpdatePoint(lHip, "lHip", playerIndex, player);
        UpdatePoint(lKnee, "lKnee", playerIndex, player);
        UpdatePoint(lPinky, "lPinky", playerIndex, player);
        UpdatePoint(lShoulder, "lShoulder", playerIndex, player);
        UpdatePoint(lSmallToe, "lSmallToe", playerIndex, player);
        UpdatePoint(lThumb, "lThumb", playerIndex, player);
        UpdatePoint(lWrist, "lWrist", playerIndex, player);

        UpdatePoint(midHip, "midHip", playerIndex, player);
        UpdatePoint(neck, "neck", playerIndex, player);
        UpdatePoint(nose, "nose", playerIndex, player);

        UpdatePoint(rAnkle, "rAnkle", playerIndex, player);
        UpdatePoint(rBigToe, "rBigToe", playerIndex, player);
        UpdatePoint(rEar, "rEar", playerIndex, player);
        UpdatePoint(rElbow, "rElbow", playerIndex, player);
        UpdatePoint(rEye, "rEye", playerIndex, player);
        UpdatePoint(rHeel, "rHeel", playerIndex, player);
        UpdatePoint(rHip, "rHip", playerIndex, player);
        UpdatePoint(rKnee, "rKnee", playerIndex, player);
        UpdatePoint(rPinky, "rPinky", playerIndex, player);
        UpdatePoint(rShoulder, "rShoulder", playerIndex, player);
        UpdatePoint(rSmallToe, "rSmallToe", playerIndex, player);
        UpdatePoint(rThumb, "rThumb", playerIndex, player);
        UpdatePoint(rWrist, "rWrist", playerIndex, player);
    }
    */

    void UpdateJoints(JSONObject joints, int playerIndex, Transform player = null)
    {
        foreach (string jointName in joints.Keys)
        {
            if (jointName == "time") { }

            else
            {
                //Debug.Log("jointName; " + jointName);
                JSONArray jointData = joints[jointName].AsArray;
                UpdatePoint(jointData, jointName, playerIndex, player);
            }
        }
    }

    /*
    void UpdatePoint(JSONArray node, string name, int playerIndex, Transform player)
    {
        Point point = null;
        Vector3 vector = new Vector3(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
        Vector3 correction = new Vector3(vector.x, vector.z, vector.y);

        foreach (Point p in points)
        {
            if(p.name == name && p.playerIndex == playerIndex)
            {
                point = p;
                p.vector = correction;
            }
        }

        if(point == null)
        {
            Debug.Log("Point not found; " + name + ", creating...");
            point = new Point(node, name, playerIndex, player);
            points.Add(point);
        }

        point.node.localPosition = correction;
    }
    */

    void UpdatePoint(JSONArray node, string name, int playerIndex, Transform player)
    {
        Vector3 vector = new Vector3(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
        Vector3 correction = new Vector3(vector.x, vector.z, vector.y);

        // Check if the point already exists in the dictionary
        string pointKey = name + "_" + playerIndex;  // Unique key for each point
        if (!pointDictionary.TryGetValue(pointKey, out Point point))
        {
            // If it doesn't exist, create new point and add to dictionary
            Debug.Log("Point not found; " + name + ", creating...");
            point = new Point(node, name, playerIndex, player); // Assuming constructor sets up the point correctly
            pointDictionary.Add(pointKey, point);
        }

        // Update the point's position
        point.node.localPosition = correction;
        point.vector = correction;
    }


    /*
    public List<Point> GetPlayerPoints(int playerIndex)
    {
        //int index = 0;
        List<Point> list = new List<Point>();

        foreach(Point point in points)
        {
            if(point.playerIndex == playerIndex)
            {
                list.Add(point);
            }
        }

        return list;
    }
    */

    public List<Point> GetPlayerPoints(int playerIndex)
    {
        List<Point> list = new List<Point>();
        string searchKey = "_" + playerIndex;  // Assuming keys are like "jointName_playerIndex"

        foreach (var kvp in pointDictionary)
        {
            // Check if the key ends with the current player index
            if (kvp.Key.EndsWith(searchKey))
            {
                list.Add(kvp.Value);
            }
        }

        return list;
    }









    /*
    [Space]
    public TextAsset file;

    [ContextMenu("Parse")]
    public void Parse()
    {
        JSONNode json = JSON.Parse(file.text);

        //JSONObject lAnkle = json["samples"]["people"][0]["joints"][0]["lAnkle"].AsObject;

        JSONObject player = json["samples"]["people"][0].AsObject;
        Debug.Log("Player: " + player.ToString());


        JSONObject joints = player["joints"][0].AsObject;
        Debug.Log("Joints: " + joints.ToString());

        ParseJoints(joints);
    }

    void ParseJoints(JSONObject joints)
    {

        JSONArray lAnkle = joints["lAnkle"].AsArray;
        JSONArray lBigToe = joints["lBigToe"].AsArray;
        JSONArray lEar = joints["lEar"].AsArray;
        JSONArray lElbow = joints["lElbow"].AsArray;
        JSONArray lEye = joints["lEye"].AsArray;
        JSONArray lHeel = joints["lHeel"].AsArray;
        JSONArray lHip = joints["lHip"].AsArray;
        JSONArray lKnee = joints["lKnee"].AsArray;
        JSONArray lPinky = joints["lPinky"].AsArray;
        JSONArray lShoulder = joints["lShoulder"].AsArray;
        JSONArray lSmallToe = joints["lSmallToe"].AsArray;
        JSONArray lThumb = joints["lThumb"].AsArray;
        JSONArray lWrist = joints["lWrist"].AsArray;

        JSONArray midHip = joints["midHip"].AsArray;
        JSONArray neck = joints["neck"].AsArray;
        JSONArray nose = joints["nose"].AsArray;

        JSONArray rAnkle = joints["rAnkle"].AsArray;
        JSONArray rBigToe = joints["rBigToe"].AsArray;
        JSONArray rEar = joints["rEar"].AsArray;
        JSONArray rElbow = joints["rElbow"].AsArray;
        JSONArray rEye = joints["rEye"].AsArray;
        JSONArray rHeel = joints["rHeel"].AsArray;
        JSONArray rHip = joints["rHip"].AsArray;
        JSONArray rKnee = joints["rKnee"].AsArray;
        JSONArray rPinky = joints["rPinky"].AsArray;
        JSONArray rShoulder = joints["rShoulder"].AsArray;
        JSONArray rSmallToe = joints["rSmallToe"].AsArray;
        JSONArray rThumb = joints["rThumb"].AsArray;
        JSONArray rWrist = joints["rWrist"].AsArray;


        points.Add(new Point(lAnkle, "lAnkle"));
        points.Add(new Point(lBigToe, "lBigToe"));
        points.Add(new Point(lEar, "lEar"));
        points.Add(new Point(lElbow, "lElbow"));
        points.Add(new Point(lEye, "lEye"));
        points.Add(new Point(lHeel, "lHeel"));
        points.Add(new Point(lHip, "lHip"));
        points.Add(new Point(lKnee, "lKnee"));
        points.Add(new Point(lPinky, "lPinky"));
        points.Add(new Point(lShoulder, "lShoulder"));
        points.Add(new Point(lSmallToe, "lSmallToe"));
        points.Add(new Point(lThumb, "lThumb"));
        points.Add(new Point(lWrist, "lWrist"));

        points.Add(new Point(midHip, "midHip"));
        points.Add(new Point(neck, "neck"));
        points.Add(new Point(nose, "nose"));

        points.Add(new Point(rAnkle));
        points.Add(new Point(rBigToe));
        points.Add(new Point(rEar));
        points.Add(new Point(rElbow));
        points.Add(new Point(rEye));
        points.Add(new Point(rHeel));
        points.Add(new Point(rHip));
        points.Add(new Point(rKnee));
        points.Add(new Point(rPinky));
        points.Add(new Point(rShoulder));
        points.Add(new Point(rSmallToe));
        points.Add(new Point(rThumb));
        points.Add(new Point(rWrist));


    }

    [System.Serializable]
    public class Joint
    {
        public HumanBodyBones bone;
        public Vector3 vector;

        public Joint(HumanBodyBones bone, JSONArray node)
        {
            this.bone = bone;
            this.vector = new Vector3(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
        }
    }

    */
    [System.Serializable]
    public class Point
    {
        public string name;
        public Vector3 vector;
        public int playerIndex;

        public Transform node;
        public Transform parent;

        public Point(JSONArray node, string name, int playerIndex, Transform player)
        {
            this.parent = player;
            //IKHelper ikHelper = this.parent.gameObject.AddComponent<IKHelper>();

            //this.vector = new Vector3(node[0].AsFloat, node[1].AsFloat, node[2].AsFloat);
            this.node = new GameObject().transform;
            this.node.SetParent(player);
            this.node.localPosition = this.vector;
            this.playerIndex = playerIndex;

            
            if(string.IsNullOrEmpty(name) == false)
            {
                this.name = name;
                this.node.name = "[" + playerIndex + "] " + name;
            }

            this.node.transform.parent = player.transform;

        }
    }

    //public List<Point> points = new List<Point>();
    public Point ball;
    public Transform ballTransform;

}
