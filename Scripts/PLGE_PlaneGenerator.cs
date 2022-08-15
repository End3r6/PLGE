using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PLGE_PlaneGenerator : MonoBehaviour
{
    // public enum ChunkType
    // {
    //     Split,
    //     Expand
    // };

    [Header("Mesh Options")]
    // [Tooltip("How do you want your mesh to be chunked?")]
    // public ChunkType chunkType;

    [Tooltip("How many chunks do you want the mesh to split into (1 means it is only the single mesh)")]
    public int chunks;

    [Space(10)]

    [Header("Mesh Data")]
    [Tooltip("The Base Dimensions for the mesh")]
    public Vector2Int numberOfFaces = new Vector2Int(1, 1);

    [Space(10)]

    [Tooltip("How many faces you want in your mesh in addition to the dimensions. Range is 1 - 50")]
    [Range(1, 50)]
    public int meshResolution = 2;

    Vector3[] vertices;

    private Mesh mesh;

    public void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "PLGE_Plane";

        // if(indexFormat == MeshIndexFormat.SixteenBit)
        // {
        //     mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
        // }
        // else if(indexFormat == MeshIndexFormat.ThirtyTwoBit)
        // {
        //     mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        // }

        try
        {
            vertices = new Vector3[(numberOfFaces.x + 1) * (numberOfFaces.y + 1) * (meshResolution * meshResolution)];
            Vector2[] uv = new Vector2[vertices.Length];

            Vector4[] tangents = new Vector4[vertices.Length];
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            for (int i = 0, z = 0; z <= meshResolution; z++) 
            {
                for (int x = 0; x <= meshResolution; x++, i++) 
                {
                    vertices[i] = (new Vector3(x * numberOfFaces.x, 0, z * numberOfFaces.y) / meshResolution) - new Vector3(numberOfFaces.x / 2, 0, numberOfFaces.y / 2);
                    uv[i] = new Vector2((float)x, (float)z) / meshResolution;

                    tangents[i] = tangent;
                }
            }

            int[] triangles = new int[meshResolution * meshResolution * 6];
            for (int ti = 0, vi = 0, y = 0; y < meshResolution; y++, vi++) 
            {
                for (int x = 0; x < meshResolution; x++, ti += 6, vi++) 
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + meshResolution + 1;
                    triangles[ti + 5] = vi + meshResolution + 2;
                }
            }

            // Set the current mesh filter to use our generated mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.tangents = tangents;

            mesh.RecalculateNormals();
        }
        catch(OutOfMemoryException)
        {
            Debug.LogWarning("You are trying to create a mesh with more verticies than Unity allows on a single mesh\n"
            + "Consider spliting the mesh into chunks or changing the mesh index");
        }
    }
}