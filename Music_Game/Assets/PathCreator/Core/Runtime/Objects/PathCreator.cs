using System.Collections.Generic;
using UnityEditor;
using PathCreation.Utility;
using UnityEngine;

namespace PathCreation {
    public class PathCreator : MonoBehaviour {

        //float timer;
        /// This class stores data for the path editor, and provides accessors to get the current vertex and bezier path.
        /// Attach to a GameObject to create a new path editor.

        public event System.Action pathUpdated;

        [SerializeField, HideInInspector]
        PathCreatorData editorData;
        [SerializeField, HideInInspector]
        bool initialized;
        

        GlobalDisplaySettings globalEditorDisplaySettings;

        private void Awake()
        {
            Generate(300);
        }

        public void GenerateNextStage()
        {
            
            //int count = bezierPath.points.Count;

            //for (int i = 0; i < 30; i++)
            //{
            //    bezierPath.DeleteSegment(0);
            //}

            Generate(30);
            //InitializeEditorData(true);
        }

        void Generate(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Vector3 pos = new Vector3(Random.Range(bezierPath.points[bezierPath.points.Count - 1].x + 10, bezierPath.points[bezierPath.points.Count - 1].x + 20), Random.Range(bezierPath.points[bezierPath.points.Count - 1].y, bezierPath.points[bezierPath.points.Count - 1].y - 10f), Random.Range(bezierPath.points[bezierPath.points.Count - 1].z, bezierPath.points[bezierPath.points.Count - 1].z - 10f));
                bezierPath.AddSegmentToEnd(pos);
            }

            
        }

        // Vertex path created from the current bezier path
        public VertexPath path {
            get {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                return editorData.GetVertexPath(transform);
            }
        }

        // The bezier path created in the editor
        public BezierPath bezierPath {
            get {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                return editorData.bezierPath;
            }
            set {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                editorData.bezierPath = value;
            }
        }

        #region Internal methods

        /// Used by the path editor to initialise some data
        public void InitializeEditorData (bool in2DMode) {
            if (editorData == null) {
                editorData = new PathCreatorData ();
            }
            editorData.bezierOrVertexPathModified -= TriggerPathUpdate;
            editorData.bezierOrVertexPathModified += TriggerPathUpdate;

            editorData.Initialize (in2DMode);
            initialized = true;
        }

        public PathCreatorData EditorData {
            get {
                return editorData;
            }

        }

        public void TriggerPathUpdate () {
            if (pathUpdated != null) {
                pathUpdated ();
            }
        }

#if UNITY_EDITOR

        // Draw the path when path objected is not selected (if enabled in settings)
        void OnDrawGizmos () {

            // Only draw path gizmo if the path object is not selected
            // (editor script is resposible for drawing when selected)
            GameObject selectedObj = UnityEditor.Selection.activeGameObject;
            if (selectedObj != gameObject) {

                if (path != null) {
                    path.UpdateTransform (transform);

                    if (globalEditorDisplaySettings == null) {
                        globalEditorDisplaySettings = GlobalDisplaySettings.Load ();
                    }

                    if (globalEditorDisplaySettings.visibleWhenNotSelected) {

                        Gizmos.color = globalEditorDisplaySettings.bezierPath;

                        for (int i = 0; i < path.NumPoints; i++) {
                            int nextI = i + 1;
                            if (nextI >= path.NumPoints) {
                                if (path.isClosedLoop) {
                                    nextI %= path.NumPoints;
                                } else {
                                    break;
                                }
                            }
                            //Gizmos.DrawLine (path.GetPoint (i), path.GetPoint (nextI));
                        }
                    }
                }
            }
        }
#endif

        #endregion
    }
}