/*==============================================
 * World space
 * Scales a box and reverses normals to generate
 * a room space of a given size
 * =============================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class WorldSpace : MonoBehaviour {

    public Vector3 size;

    Mesh room;
    MeshCollider roomCollider;

	// Use this for initialization
	void Start () {
        room = GetComponent<MeshFilter>().mesh;
        SetRoomSize(size);
        room = ReverseNormals(room);

        roomCollider = GetComponent<MeshCollider>();
        roomCollider.sharedMesh = room;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetRoomSize(Vector3 roomSize)
    {
        this.transform.localScale = size;
        TranslateWorldSpace.setWorldSize(size);
    }

    Mesh ReverseNormals(Mesh meshToReverse)
    {
            Mesh mesh = meshToReverse;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }

            return mesh;
    }
}
