using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_CollisionLayers : MonoBehaviour
    {
        public float SetFixedTimeStep = 0.01f;
        public float TimeScale = 1f;
        public float GravityY = -9.81f;

        void Start()
        {
            Time.timeScale = TimeScale;
            Physics.gravity = new Vector3(Physics.gravity.x, GravityY, Physics.gravity.z);
            Time.fixedDeltaTime = SetFixedTimeStep;
            Physics.IgnoreLayerCollision(1, 4, true);
        }
    }


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Demo_Ragd_CollisionLayers))]
    public class Demo_Ragd_CollisionLayersEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("Setting fixed timestep for more precise impact detection on camera bullets", UnityEditor.MessageType.Info);
            DrawDefaultInspector();
            GUILayout.Space(4f);
            UnityEditor.EditorGUILayout.HelpBox("Disabling collision between TransparentFX and Water layer for playmode only!", UnityEditor.MessageType.Warning);
        }
    }
#endif
}