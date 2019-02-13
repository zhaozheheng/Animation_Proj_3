using UnityEngine;
using System.Collections;
using Assets;
using System.Collections.Generic;
using System.IO;

public class ParseSkelAndSkin : MonoBehaviour {
	public Material mat;
	private GameObject rootobj;
	string skelPath = "wasp4unity.skel";
	string skinPath = "wasp4unity.skin";
	//private firstName = "";
	// Use this for initialization
	void Start () {
		rootobj = this.gameObject;
		parseSkel();
		parseSkin();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void parseSkel() {
		Stack<CubeNode> stackCube = new Stack<CubeNode>();

		List<string> lines = new List<string> (File.ReadAllLines(skelPath));

		string childName = "";
		CubeNode root = null;
		string[] childSplit;
		float[] nowOffset = {0,0,0};
		float[] nowBoxmin = {-0.1f,-0.1f,-0.1f};
		float[] nowBoxmax = {0.1f,0.1f,0.1f};
		float[] nowPose = {0,0,0};
		float[] nowRotxlimit = {-100000,100000};
		float[] nowRotylimit = {-100000,100000};
		float[] nowRotzlimit = {-100000,100000};

		for (int i = 0; i < lines.Count; i = i + 1) {
            lines[i] = lines[i].Replace("      ", " ");
            lines[i] = lines[i].Replace("     ", " ");
            lines[i] = lines[i].Replace("    ", " ");
            lines[i] = lines[i].Replace("   ", " ");
            lines[i] = lines[i].Replace("  ", " ");
            if (lines[i][0] == ' ') lines[i] = lines[i].Substring(1);
        }

		for (int i = 0; i < lines.Count; i = i + 1) {

			if (lines[i].Contains("{")) {
				childSplit = lines [i].Split (' ');
				childName = childSplit [1];
				i = i + 1;
				print (childName);
				while (!lines[i].Contains("{") && !lines[i].Contains("}")) {
					if (lines[i].Contains("offset")) {
						childSplit = lines [i].Split (' ');
						nowOffset[0] = float.Parse(childSplit[1]);
						nowOffset[1] = float.Parse(childSplit[2]);
						nowOffset[2] = float.Parse(childSplit[3]);
						
					}

					if (lines[i].Contains("boxmin")) {
						childSplit = lines [i].Split (' ');
						nowBoxmin[0] = float.Parse(childSplit[1]);
						nowBoxmin[1] = float.Parse(childSplit[2]);
						nowBoxmin[2] = float.Parse(childSplit[3]);
						
					}

					if (lines[i].Contains("boxmax")) {
						childSplit = lines [i].Split (' ');
						nowBoxmax[0] = float.Parse(childSplit[1]);
						nowBoxmax[1] = float.Parse(childSplit[2]);
						nowBoxmax[2] = float.Parse(childSplit[3]);
						
					}

					if (lines[i].Contains("pose")) {
						childSplit = lines [i].Split (' ');
						nowPose[0] = float.Parse(childSplit[1]);
						nowPose[1] = float.Parse(childSplit[2]);
						nowPose[2] = float.Parse(childSplit[3]);
						
					}

					if (lines[i].Contains("rotxlimit")) {
						childSplit = lines [i].Split (' ');
						nowRotxlimit[0] = float.Parse(childSplit[1]);
						nowRotxlimit[1] = float.Parse(childSplit[2]);
						
					}

					if (lines[i].Contains("rotylimit")) {
						childSplit = lines [i].Split (' ');
						nowRotylimit[0] = float.Parse(childSplit[1]);
						nowRotylimit[1] = float.Parse(childSplit[2]);
						
					}

					if (lines[i].Contains("rotzlimit")) {
						childSplit = lines [i].Split (' ');
						nowRotzlimit[0] = float.Parse(childSplit[1]);
						nowRotzlimit[1] = float.Parse(childSplit[2]);
						
					}

					i = i + 1;
				}

				CubeNode childNode = null;
				if (root == null) 
				{
					childNode = new CubeNode(childName, null, nowOffset, nowBoxmin, nowBoxmax, nowPose, nowRotxlimit, nowRotylimit, nowRotzlimit);
					root = childNode;
					drawMesh(childNode, false);
				}else {
					childNode = new CubeNode(childName, stackCube.Peek(), nowOffset, nowBoxmin, nowBoxmax, nowPose, nowRotxlimit, nowRotylimit, nowRotzlimit);
					drawMesh(childNode, true);
				}
				stackCube.Push(childNode);
				
				i = i - 1;
				
			}
			if (lines[i].Contains("}") && stackCube.Count!=0) {
				stackCube.Pop ();
			}
		}
	}

	void drawMesh(CubeNode cnode, bool isChild){
		
		GameObject gameObject;
		if (isChild) 
		{
			gameObject = new GameObject(cnode.objectName);
		}else {
			gameObject = rootobj;
			gameObject.name = cnode.objectName;
		}

		gameObject.AddComponent<Rigidbody>();
		gameObject.GetComponent<Rigidbody>().isKinematic = true;

		//GameObject gameObject = new GameObject (cnode.objectName);
		
		if (isChild) 
		{
			gameObject.transform.SetParent (GameObject.Find(cnode.father.objectName).transform);
			gameObject.transform.localPosition = new Vector3 (cnode.offset[0],cnode.offset[1],cnode.offset[2]);

			float rotx = Mathf.Clamp(cnode.pose[0]*Mathf.Rad2Deg, cnode.rxlimit[0]*Mathf.Rad2Deg, cnode.rxlimit[1]*Mathf.Rad2Deg);
			float roty = Mathf.Clamp(cnode.pose[1]*Mathf.Rad2Deg, cnode.rylimit[0]*Mathf.Rad2Deg, cnode.rylimit[1]*Mathf.Rad2Deg);
			float rotz = Mathf.Clamp(cnode.pose[2]*Mathf.Rad2Deg, cnode.rzlimit[0]*Mathf.Rad2Deg, cnode.rzlimit[1]*Mathf.Rad2Deg);

			gameObject.transform.localRotation = Quaternion.AngleAxis(rotz, Vector3.forward)*Quaternion.AngleAxis(roty, Vector3.up)*Quaternion.AngleAxis(rotx, Vector3.right);
		}
	}

	void parseSkin() {
		List<string> lines = new List<string> (File.ReadAllLines(skinPath));
		List<Vector3> positions = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Matrix4x4> bindings = new List<Matrix4x4>();
		List<BoneWeight> skinweights = new List<BoneWeight>();
		string[] nameSplit;

		for (int i = 0; i < lines.Count; i = i + 1) {
            lines[i] = lines[i].Replace("      ", " ");
            lines[i] = lines[i].Replace("     ", " ");
            lines[i] = lines[i].Replace("    ", " ");
            lines[i] = lines[i].Replace("   ", " ");
            lines[i] = lines[i].Replace("  ", " ");
            if (lines[i][0] == ' ') lines[i] = lines[i].Substring(1);
        }
        
		for (int i = 0; i < lines.Count; i = i + 1) {
			if (lines[i].Contains("{")) {
				nameSplit = lines[i].Split(' ');
				string name = nameSplit[0];
				i = i + 1;
				if (name == "positions") 
				{
					while (!lines[i].Contains("{") && !lines[i].Contains("}")) {
						nameSplit = lines[i].Split (' ');
						positions.Add(new Vector3(float.Parse(nameSplit[0]),float.Parse(nameSplit[1]),float.Parse(nameSplit[2])));
						i = i + 1;
					}
				}else if (name == "normals") {
					while (!lines[i].Contains("{") && !lines[i].Contains("}")) {
						nameSplit = lines[i].Split (' ');
						normals.Add(new Vector3(float.Parse(nameSplit[0]),float.Parse(nameSplit[1]),float.Parse(nameSplit[2])));
						i = i + 1;
					}
				}else if (name == "triangles") {
					while (!lines[i].Contains("{") && !lines[i].Contains("}")) {
						nameSplit = lines[i].Split (' ');
						triangles.Add(int.Parse(nameSplit[0]));
						triangles.Add(int.Parse(nameSplit[1]));
						triangles.Add(int.Parse(nameSplit[2]));
						i = i + 1;
					}
				}else if (name == "bindings") {
					while (lines[i].Contains("matrix")) 
					{
						Matrix4x4 matrix = new Matrix4x4();
						matrix.m00 = float.Parse(lines[i+1].Split(' ')[0]);
                        matrix.m01 = float.Parse(lines[i+2].Split(' ')[0]);
                        matrix.m02 = float.Parse(lines[i+3].Split(' ')[0]);
                        matrix.m03 = float.Parse(lines[i+4].Split(' ')[0]);
                        matrix.m10 = float.Parse(lines[i+1].Split(' ')[1]);
                        matrix.m11 = float.Parse(lines[i+2].Split(' ')[1]);
                        matrix.m12 = float.Parse(lines[i+3].Split(' ')[1]);
                        matrix.m13 = float.Parse(lines[i+4].Split(' ')[1]);
                        matrix.m20 = float.Parse(lines[i+1].Split(' ')[2]);
                        matrix.m21 = float.Parse(lines[i+2].Split(' ')[2]);
                        matrix.m22 = float.Parse(lines[i+3].Split(' ')[2]);
                        matrix.m23 = float.Parse(lines[i+4].Split(' ')[2]);
                        matrix.m30 = 0;
                        matrix.m31 = 0;
                        matrix.m32 = 0;
                        matrix.m33 = 1;
                        bindings.Add(matrix.inverse);
						i = i + 6;
					}
				}else if (name == "skinweights") {
					while (!lines[i].Contains("{") && !lines[i].Contains("}")) {
						nameSplit = lines[i].Split (' ');
						int note = int.Parse(nameSplit[0]);
						BoneWeight bones = new BoneWeight();
						switch (note) 
						{
							case 1:
							  	bones.boneIndex0 = int.Parse(nameSplit[1]);
								bones.weight0 = float.Parse(nameSplit[2]);
							  break;
							case 2:
							  	bones.boneIndex0 = int.Parse(nameSplit[1]);
								bones.weight0 = float.Parse(nameSplit[2]);
								bones.boneIndex1 = int.Parse(nameSplit[3]);
								bones.weight1 = float.Parse(nameSplit[4]);
							  break;
							case 3:
							  	bones.boneIndex0 = int.Parse(nameSplit[1]);
								bones.weight0 = float.Parse(nameSplit[2]);
								bones.boneIndex1 = int.Parse(nameSplit[3]);
								bones.weight1 = float.Parse(nameSplit[4]);
								bones.boneIndex2 = int.Parse(nameSplit[5]);
								bones.weight2 = float.Parse(nameSplit[6]);
							  break;
							case 4:
							  	bones.boneIndex0 = int.Parse(nameSplit[1]);
								bones.weight0 = float.Parse(nameSplit[2]);
								bones.boneIndex1 = int.Parse(nameSplit[3]);
								bones.weight1 = float.Parse(nameSplit[4]);
								bones.boneIndex2 = int.Parse(nameSplit[5]);
								bones.weight2 = float.Parse(nameSplit[6]);
								bones.boneIndex3 = int.Parse(nameSplit[7]);
								bones.weight3 = float.Parse(nameSplit[8]);
							  break;
							default:
							  
							  break;
						}
						skinweights.Add(bones);
						i = i + 1;
					}
				}
			}
		}

		GameObject gameObject = rootobj;

		SkinnedMeshRenderer mr = gameObject.GetComponent<SkinnedMeshRenderer>();

		mr.material = mat;
		Mesh m = new Mesh();
		m.vertices = positions.ToArray();
		m.triangles = triangles.ToArray();
        m.normals = normals.ToArray();
        m.boneWeights = skinweights.ToArray();
        m.bindposes = bindings.ToArray();
        mr.bones = gameObject.GetComponentsInChildren<Transform>();

        mr.sharedMesh = m;
	}
}
