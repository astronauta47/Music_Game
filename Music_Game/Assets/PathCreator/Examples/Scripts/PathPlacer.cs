using PathCreation;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples {

    [ExecuteInEditMode]
    public class PathPlacer : PathSceneTool {

        public GameObject prefab;
        public GameObject holder;
        public Material startMat;

        public List<Transform> Walls = new List<Transform>();
        int index;
        int dir = 1;

        float counterColor;

        //private void Update()
        //{
        //    counterColor += Time.deltaTime;
            
        //    if (counterColor > 20) counterColor = 0;

        //    startMat.color = Color.HSVToRGB(counterColor / 20, .33f, 1);
        //}

        public void Generate (float dst)
        {
            if (pathCreator != null && prefab != null && holder != null)
            {

                //DestroyObjects ();

                VertexPath path = pathCreator.path;

                Vector3 point = path.GetPointAtDistance(dst);
                Quaternion rot = path.GetRotationAtDistance(dst);

                index++;

                if (index >= Walls.Count)
                    index = 0;

                dir *= -1;

                Walls[index].GetChild(0).GetComponent<MeshRenderer>().material = startMat;
                Walls[index].position = point;
                Walls[index].rotation = rot;
                Walls[index].transform.GetChild(0).localPosition = new Vector3(-0.5f, 0.4f * dir, 0);

            }
        }

        void DestroyObjects () {
            int numChildren = holder.transform.childCount;
            for (int i = numChildren - 1; i >= 0; i--) {
                DestroyImmediate (holder.transform.GetChild (i).gameObject, false);
            }
        }

        protected override void PathUpdated () {
            //if (pathCreator != null) {
            //    Generate (0);
            //}
        }
    }
}