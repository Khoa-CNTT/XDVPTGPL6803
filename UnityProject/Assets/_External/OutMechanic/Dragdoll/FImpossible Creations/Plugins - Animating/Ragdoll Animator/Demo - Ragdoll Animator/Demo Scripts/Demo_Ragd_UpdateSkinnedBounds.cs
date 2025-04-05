using UnityEngine;

namespace FIMSpace.Basics
{
    [DefaultExecutionOrder(10000)]
    public class Demo_Ragd_UpdateSkinnedBounds : MonoBehaviour
    {
        public SkinnedMeshRenderer skinnedMesh;

        [Tooltip("Some bone of the skinned mesh (try using some child bones of root bone), when left empty then child of skinned mesh root bone is used")]
        public Transform boundsFollows;

        Vector3 off = Vector3.zero;
        void Start()
        {
            if (skinnedMesh == null) skinnedMesh = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMesh)
            {
                if (boundsFollows == null)
                {
                    boundsFollows = skinnedMesh.rootBone;
                    if (boundsFollows.childCount > 0) boundsFollows = skinnedMesh.rootBone.GetChild(0);
                }

                off = boundsFollows.InverseTransformPoint(skinnedMesh.bounds.center);
            }
            else
                Destroy(this);
        }

        void LateUpdate()
        {
            if (boundsFollows == null) { Destroy(this); return; }
            if (skinnedMesh.enabled == false) { return; }

            Bounds lateBounds = skinnedMesh.localBounds;
            lateBounds.center = boundsFollows.TransformPoint(off);
            skinnedMesh.localBounds = lateBounds;
        }
    }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(Demo_Ragd_UpdateSkinnedBounds))]
    public class Demo_Ragd_UpdateSkinnedBoundsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("Making SkinnedMeshRenderer bounds 'Extent' slightly bigger is recommended", UnityEditor.MessageType.Info);
            DrawDefaultInspector();
        }
    }
#endif

}