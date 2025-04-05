using System.Collections;
using UnityEngine;

namespace HieuDev
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SimpleProceduralMesh : MonoBehaviour
    {
        Mesh mesh;

        public Vector3[] vertices;
        public int[] triangles;

        public int xSize = 20;
        public int zSize = 20;

        public float strength = 0.3f;

        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            StartCoroutine(CreateShape());
        }

        private void Update()
        {
            UpdateMesh();
        }

        IEnumerator CreateShape()
        {
            vertices = new Vector3[(xSize + 1) * (zSize + 1)];

            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    // float y = Mathf.PerlinNoise(x * strength, z * strength) * 2f;
                    vertices[i] = new Vector3(x, 0, z);
                    i++;
                }
            }

            triangles = new int[xSize * zSize * 6];

            int vert = 0;
            int high = 0;

            for (int z = 0; z < zSize; z++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    triangles[high + 0] = vert + 0;
                    triangles[high + 1] = vert + xSize + 1;
                    triangles[high + 2] = vert + 1;
                    triangles[high + 3] = vert + 1;
                    triangles[high + 4] = vert + xSize + 1;
                    triangles[high + 5] = vert + xSize + 2;

                    vert++;
                    high += 6;

                    yield return new WaitForSeconds(.2f);
                }
                vert++;
            }
        }

        void UpdateMesh()
        {
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
        }

        void UpdateTexture()
        {
            // mesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
            // mesh.triangles = new int[] { 0, 2, 1 };
            // mesh.uv = new Vector2[] { Vector2.zero, Vector2.right, Vector2.up };
            // mesh.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back };
            // mesh.tangents = new Vector4[] {
            // new Vector4(1f, 0f, 0f, -1f),
            // new Vector4(1f, 0f, 0f, -1f),
            // new Vector4(1f, 0f, 0f, -1f)
            // };
        }
    }
}
