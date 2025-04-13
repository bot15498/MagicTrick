#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using static System.Runtime.InteropServices.Marshal;

[RequireComponent(typeof(MeshSurfaceSampler))]
public class LeafInstancer : MonoBehaviour
{
    [SerializeField] LayerMask _grassLayer;
    [SerializeField] Mesh _leafMesh;
    [SerializeField] Material _material;

    public MeshSurfaceSampler _meshSurfaceSampler;
    RenderParams _renderParams;

    GraphicsBuffer commandBuffer;
    GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
    LeafData[] leafData;
    ComputeBuffer leafBuffer;

    const int commandCount = 1;
    bool isGenerated = false;

    struct LeafData
    {
        public Vector3 position;
        public Vector3 normal;
    }

    void OnEnable()
    {
        Setup();
        isGenerated = false;
        StartCoroutine(WaitUntilReadyAndGenerate());

    }

    void OnDisable()
    {
        commandBuffer?.Release();
        commandBuffer = null;
        leafBuffer?.Release();
        leafBuffer = null;
        isGenerated = false;
    }

    void Setup()
    {
        _meshSurfaceSampler = GetComponent<MeshSurfaceSampler>();
        if (_meshSurfaceSampler == null)
            Debug.LogError("MeshSurfaceSampler is null");

        if (_material == null)
            Debug.LogError("Material is null");

        if (_leafMesh == null)
            Debug.LogError("Leaf mesh is null");
    }

    IEnumerator WaitUntilReadyAndGenerate()
    {
        // Continuously check until points exist and are valid
        while (_meshSurfaceSampler == null ||
               !_meshSurfaceSampler.ArePointsGenerated() ||
               _meshSurfaceSampler.points == null ||
               _meshSurfaceSampler.points.Length == 0)
        {
            yield return null;
        }

        Generate();
    }

    public void GeneratePoints()
    {
        _meshSurfaceSampler.GeneratePoints();
    }

    public Bounds GetBoundingBox()
    {
        Bounds boundingBox = new Bounds();

        if (_meshSurfaceSampler.points != null && _meshSurfaceSampler.points.Length > 0)
        {
            Vector3 minPoint = _meshSurfaceSampler.points[0];
            Vector3 maxPoint = _meshSurfaceSampler.points[0];

            for (int i = 1; i < _meshSurfaceSampler.points.Length; i++)
            {
                minPoint = Vector3.Min(minPoint, _meshSurfaceSampler.points[i]);
                maxPoint = Vector3.Max(maxPoint, _meshSurfaceSampler.points[i]);
            }

            boundingBox.SetMinMax(minPoint, maxPoint);
        }

        if (_material != null && _material.HasProperty("_Extrude"))
        {
            var extrude = _material.GetFloat("_Extrude");
            boundingBox.Expand(boundingBox.size * extrude);
        }

        return boundingBox;
    }

    public void Generate()
    {
        if (_meshSurfaceSampler == null)
            Setup();

        if (!_meshSurfaceSampler.ArePointsGenerated())
            _meshSurfaceSampler.GeneratePoints();

        if (_meshSurfaceSampler.points == null || _meshSurfaceSampler.points.Length == 0)
        {
            Debug.LogWarning("LeafInstancer: No points found. Skipping buffer generation.");
            return;
        }

        var count = _meshSurfaceSampler.points.Length;

        leafData = new LeafData[count];
        leafBuffer?.Release();
        leafBuffer = new ComputeBuffer(count, SizeOf<LeafData>());
        commandBuffer?.Release();
        commandBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandCount, GraphicsBuffer.IndirectDrawIndexedArgs.size);
        commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[commandCount];

        for (int i = 0; i < count; i++)
        {
            leafData[i] = new LeafData
            {
                position = _meshSurfaceSampler.points[i],
                normal = _meshSurfaceSampler.normals[i]
            };
        }

        var indirectDrawIndexedArgs = new GraphicsBuffer.IndirectDrawIndexedArgs
        {
            indexCountPerInstance = _leafMesh.GetIndexCount(0),
            instanceCount = (uint)count,
            startIndex = _leafMesh.GetIndexStart(0),
            baseVertexIndex = _leafMesh.GetBaseVertex(0),
            startInstance = 0,
        };

        for (int i = 0; i < commandCount; i++)
            commandData[i] = indirectDrawIndexedArgs;

        commandBuffer.SetData(commandData);
        leafBuffer.SetData(leafData);

        var block = new MaterialPropertyBlock();
        block.SetBuffer("_LeafData", leafBuffer);

        _renderParams = new RenderParams(_material)
        {
            layer = (int)Mathf.Log(_grassLayer.value, 2),
            worldBounds = new Bounds(Vector3.zero, 10000 * Vector3.one),
            matProps = block,
            receiveShadows = true,
        };

        UpdateRotation();
        isGenerated = true;
    }

    void UpdateRotation()
    {
        var cam = Camera.main;
        if (cam == null || _renderParams.matProps == null) return;

        var target = cam.transform.position;
        var rotation = Quaternion.LookRotation(transform.position - target, Vector3.up);
        var quaternion = new Vector4(rotation.x, rotation.y, rotation.z, rotation.w);
        _renderParams.matProps.SetVector("_Rotation", quaternion);
    }

    void Update()
    {
        if (!isGenerated || leafBuffer == null || leafData == null)
            return;

        UpdateRotation();

        var cam = Camera.main;
        if (cam == null || _renderParams.matProps == null)
            return;

        var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (!GeometryUtility.TestPlanesAABB(frustumPlanes, GetBoundingBox()))
            return;

        Graphics.RenderMeshIndirect(_renderParams, _leafMesh, commandBuffer, commandCount);
    }
    
}

#if UNITY_EDITOR
// Custom editor which adds a button to generate the terrain
[CustomEditor(typeof(LeafInstancer))]
public class LeafInstancerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (LeafInstancer)target;
        DrawDefaultInspector();
        GUILayout.Space(10);
        if (GUILayout.Button("Generate Leaves"))
        {
            script.GeneratePoints();
            script.Generate();
        }
    }
}
#endif