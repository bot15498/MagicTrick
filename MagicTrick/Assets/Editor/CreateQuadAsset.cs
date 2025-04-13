using UnityEngine;
using UnityEditor;

public class CreateQuadAsset
{
    [MenuItem("Assets/Create/Custom Readable Quad")]
    static void CreateMeshAsset()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CustomReadableQuad";

        mesh.vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0)
        };

        mesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mesh.triangles = new int[]
        {
            0, 2, 1,
            2, 3, 1
        };

        mesh.RecalculateNormals();

        string path = "Assets/CustomReadableQuad.asset";
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        Debug.Log("Saved readable quad mesh at: " + path);
    }
}
