// Script based on:
// * https://github.com/Ali10555/FakeRopeSimulation
// * Mathias Soeholm: https://gist.github.com/mathiassoeholm/15f3eeda606e9be543165360615c8bef
// Edited by Peter Dickx https://github.com/dickxpe

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class Rope : MonoBehaviour
{
    [Header("Rope Transforms")]
    [Tooltip("The rope will be anchored to this point")]
    [SerializeField]
    Transform AnchorPoint;

    [Tooltip("This will move at the center hanging from the rope")]
    [SerializeField]
    Transform MidPoint;

    [Tooltip("The rope will end at this point")]
    [SerializeField]
    Transform EndPoint;

    [Header("Rope Settings")]
    [Tooltip("How many points should the rope have, 3 would be a triangle with straight lines, 100 would be a very flexible rope with many parts")]
    [SerializeField]
    [Range(2, 100)]
    public int linePoints = 10;

    [Tooltip("Value highly dependent on use case, a metal cable would have high stifness, a rubber rope would have a low one")]
    [SerializeField]
    [Range(0, 250)]
    float stiffness = 50f;

    [SerializeField]
    [Range(1, 100)]
    float damping = 10f;

    [Tooltip("How long is the rope, it will hang more or less from starting point to end point depending on this value")]
    [SerializeField]
    [Range(0.001f, 1000)]
    float ropeLength = 20;

    [Tooltip("The Rope width set at start (changing this value during run time will produce no effect)")]
    [Range(0.01f, 100)]
    [SerializeField]
    float ropeWidth = 0.1f;

    [Tooltip("The number of sides the rope has (number of vertices on a circle))")]
    [SerializeField]
    [Range(3, 50)]
    int _sides = 10;

    [Tooltip("The prefab to attach to the attachpoints")]
    [SerializeField]
    GameObject _attachedPrefab;

    [Tooltip("Simulate basic gravity")]
    bool _simulateGravity = true;
    [SerializeField]
    List<Transform> _positions = new List<Transform>();

    float currentValue;
    float currentVelocity;
    float targetValue;
    float valueThreshold = 0.01f;
    float velocityThreshold = 0.01f;
    Vector3[] _vertices;
    Mesh _mesh;
    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;

    private void Start()
    {
        currentValue = GetPointOnCurve(0.5f).y;
    }

    private void Update()
    {
#if (UNITY_EDITOR)
        currentValue = GetPointOnCurve(0.5f).y;
#endif
        SetSplinePoint();
        GenerateMesh();
    }

    void SetSplinePoint()
    {
        if (EndPoint == null)
        {
            EndPoint = new GameObject("EndPoint").transform;
            EndPoint.transform.position = transform.position + new Vector3(5f, 0, 0);
            EndPoint.SetParent(transform);
        }

        if (AnchorPoint == null)
        {
            AnchorPoint = transform;
        }

        Vector3 mid = GetPointOnCurve(0.5f);
        if (_simulateGravity)
        {
            targetValue = mid.y;
            mid.y = currentValue;


            if (MidPoint != null)
                MidPoint.position = GetBezierPoint(EndPoint.position, mid, AnchorPoint.position, 0.5f);
        }

        int capacity = _positions.Count;

        if (linePoints < capacity)
        {
            List<GameObject> toRemove = new List<GameObject>();

            for (int i = linePoints; i < capacity; i++)
            {
                Transform remove = _positions[i];
                toRemove.Add(remove.gameObject);
            }

            _positions.RemoveRange(linePoints, capacity - linePoints);

            foreach (GameObject go in toRemove)
            {
                DestroyImmediate(go);
            }

        }

        capacity = _positions.Count;

        if (capacity < linePoints)
        {
            for (int i = 0; i < linePoints - capacity; i++)
            {
                GameObject go = new GameObject("Position " + (capacity + i));
                go.transform.SetParent(transform);
                if (_positions.Count > 0)
                {
                    go.transform.localPosition = _positions[_positions.Count - 1].localPosition;
                }
                _positions.Add(go.transform);

                if (_attachedPrefab != null)
                {
                    GameObject attachedObject = Instantiate(_attachedPrefab, go.transform);
                    attachedObject.transform.Translate(0, -ropeWidth, 0);
                }
            }
        }

        if (_simulateGravity)
        {
            for (int i = 0; i < linePoints + 1; i++)
            {
                Vector3 p = GetBezierPoint(EndPoint.position, mid, AnchorPoint.position, i / ((float)linePoints - 1));

                if (i < capacity)
                {
                    _positions[i].position = p;
                }
            }
        }
    }

    Vector3 GetPointOnCurve(float percentage)
    {
        var (startPointPosition, endPointPosition) = (EndPoint.position, AnchorPoint.position);
        Vector3 midpos = Vector3.Lerp(startPointPosition, endPointPosition, percentage);
        if (_simulateGravity)
        {
            float yFactor = ropeLength - Mathf.Min(Vector3.Distance(startPointPosition, endPointPosition), ropeLength);
            midpos.y -= yFactor;
        }
        return midpos;
    }

    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 point = Vector3.Lerp(a, b, t);

        return point;
    }


    void FixedUpdate()
    {
        SimulatePhysics();
    }


    void SimulatePhysics()
    {
        if (_simulateGravity)
        {
            float dampingFactor = Mathf.Max(0, 1 - damping * Time.fixedDeltaTime);
            float acceleration = (targetValue - currentValue) * stiffness * Time.fixedDeltaTime;
            currentVelocity = currentVelocity * dampingFactor + acceleration;
            currentValue += currentVelocity * Time.fixedDeltaTime;

            if (Mathf.Abs(currentValue - targetValue) < valueThreshold && Mathf.Abs(currentVelocity) < velocityThreshold)
            {
                currentValue = targetValue;
                currentVelocity = 0f;
            }
        }
    }

    public Material material
    {
        get { return _meshRenderer.material; }
        set { _meshRenderer.material = value; }
    }


    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        if (_meshFilter == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer == null)
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = GraphicsSettings.defaultRenderPipeline.defaultMaterial;
        }

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
    }

    private void OnEnable()
    {
        _meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _meshRenderer.enabled = false;
    }

    private void OnValidate()
    {
        // if (ropeLength > linePoints)
        // {
        //     linePoints = ropeLength;
        // }
        //  currentValue = GetPointOnCurve(0.5f).y;
    }

    private void GenerateMesh()
    {
        if (_mesh == null || _positions == null || _positions.Count <= 1)
        {
            _mesh = new Mesh();
            return;
        }

        var verticesLength = _sides * _positions.Count + 2;
        if (_vertices == null || _vertices.Length != verticesLength)
        {
            _vertices = new Vector3[verticesLength];

            var indices = GenerateIndices();
            var uvs = GenerateUVs();

            if (verticesLength > _mesh.vertexCount)
            {
                _mesh.vertices = _vertices;
                _mesh.triangles = indices;
                //_mesh.uv = uvs;
            }
            else
            {
                _mesh.triangles = indices;
                _mesh.vertices = _vertices;
                // _mesh.uv = uvs;
            }
        }

        var currentVertIndex = 0;

        for (int i = 0; i < _positions.Count; i++)
        {
            var circle = CalculateCircle(i);
            foreach (var vertex in circle)
            {
                _vertices[currentVertIndex++] = transform.InverseTransformPoint(vertex);
            }
        }

        _vertices[currentVertIndex++] = _positions[_positions.Count - 1].localPosition;
        _vertices[currentVertIndex++] = _positions[0].localPosition;

        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _meshFilter.mesh = _mesh;
    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[_positions.Count * _sides];

        for (int segment = 0; segment < _positions.Count; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var u = side / (_sides - 1f);
                var v = segment / (_positions.Count - 1f);

                uvs[vertIndex] = new Vector2(u, v);
            }
        }

        return uvs;
    }

    private int[] GenerateIndices()
    {
        // Two triangles and 3 vertices
        var indices = new int[_positions.Count * _sides * 2 * 3 + _sides * 3];

        var currentIndicesIndex = 0;
        for (int segment = 1; segment < _positions.Count; segment++)
        {
            for (int side = 0; side < _sides; side++)
            {
                var vertIndex = (segment * _sides + side);
                var prevVertIndex = vertIndex - _sides;

                // Triangle one
                indices[currentIndicesIndex++] = prevVertIndex;
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = vertIndex;


                // Triangle two
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
                indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                indices[currentIndicesIndex++] = prevVertIndex;
            }
        }

        for (int i = 0; i < _sides; i++)
        {
            // Cap on start of rope
            indices[currentIndicesIndex++] = _vertices.Length - 1;
            if (i == _sides - 1)
            {
                indices[currentIndicesIndex++] = 0;
            }
            else
            {
                indices[currentIndicesIndex++] = i + 1;
            }
            indices[currentIndicesIndex++] = i;

            //cap on end of rope
            indices[currentIndicesIndex++] = _vertices.Length - 2;
            indices[currentIndicesIndex++] = _vertices.Length - 2 - _sides + i;
            if (i == _sides - 1)
            {
                indices[currentIndicesIndex++] = _vertices.Length - 2 - _sides;
            }
            else
            {
                indices[currentIndicesIndex++] = _vertices.Length - 2 - _sides + i + 1;
            }

        }

        return indices;
    }

    private Vector3[] CalculateCircle(int index)
    {
        var dirCount = 0;
        var forward = Vector3.zero;

        // If not first index
        if (index > 0)
        {
            forward += (_positions[index].position - _positions[index - 1].position).normalized;
            dirCount++;
        }

        // If not last index
        if (index < _positions.Count - 1)
        {
            forward += (_positions[index + 1].position - _positions[index].position).normalized;
            dirCount++;
        }

        // Forward is the average of the connecting edges directions
        forward = (forward / dirCount).normalized;
        var side = Vector3.Cross(forward, forward + new Vector3(0, 1, 0)).normalized;
        var up = Vector3.Cross(forward, side).normalized;

        var circle = new Vector3[_sides];
        var angle = 0f;
        var angleStep = (2 * Mathf.PI) / _sides;

        var t = index / (_positions.Count - 1f);
        var radius = ropeWidth;

        for (int i = 0; i < _sides; i++)
        {

            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            circle[i] = _positions[index].position + side * x * radius + up * y * radius;

            angle += angleStep;

        }

        return circle;
    }


}