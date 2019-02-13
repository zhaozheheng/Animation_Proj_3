using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets{

	public class CubeNode {
		public string objectName = "";
		public float[] offset;
		public float[] boxmin;
		public float[] boxmax;
		public float[] pose;
		public float[] rxlimit;
		public float[] rylimit;
		public float[] rzlimit;

		public CubeNode father;
		public List<CubeNode> child = new List<CubeNode> ();

		public CubeNode(string objectName, CubeNode father, float[] offset, float[] boxmin, float[] boxmax, float[] pose, float[] rxlimit, float[] rylimit, float[] rzlimit) {
			this.objectName = objectName;
			this.offset = offset;
			this.boxmin = boxmin;
			this.boxmax = boxmax;
			this.pose = pose;
			this.rxlimit = rxlimit;
			this.rylimit = rylimit;
			this.rzlimit = rzlimit;
			this.father = father;
		}
	
		public CubeNode(string objectName, float[] offset, float[] boxmin, float[] boxmax, float[] pose, float[] rxlimit, float[] rylimit, float[] rzlimit) {
			this.objectName = objectName;
			this.offset = offset;
			this.boxmin = boxmin;
			this.boxmax = boxmax;
			this.pose = pose;
			this.rxlimit = rxlimit;
			this.rylimit = rylimit;
			this.rzlimit = rzlimit;
		}
	}

}
