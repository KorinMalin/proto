using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Footprint : MonoBehaviour {

    #region Attributes

    // PUBLIC PROPERTIES
    [Header("Foot properties")]
    [Tooltip("Size of the footprint. For a better effect, use same proportions as the foot texture used as albedo in the footMaterial.")]
    public Vector2 footScale = new Vector2(0.8f, 1.4f);
    [Tooltip("Default look should be the left foot. Put a texture foot as albedo and set the rendering mode to \"Transparent\".")]
    public Material footMaterial;

    [Header("Foot movement")]
    private Transform movingObject;
    [Tooltip("Distance between two footprints.")]
    [Range(0.0f, 4.0f)]
    public float distanceStep = 1.2f;
    [Tooltip("Distance between the left foot and the right foot.")]
    [Range(0.0f, 1.0f)]
    public float leftRightOffset = 0.3f;

    [Header("Foot physics")]
    [Tooltip("The maximum distance between the center of the moving object and the ground for leaving a footprint.")]
    public float groundDistance = 1.0f;
    [Tooltip("Select the layers on which you can let footprints.")]
    public LayerMask groundLayers;
    public FirstPersonController fpc;

    [Header("Debug")]
    [Tooltip("When activated, you can see red rays draw accorded to the \"groundDistance\" in your editor scene.")]
    [SerializeField]
    private bool drawRayInEditor = false;
    [Tooltip("This is an Y value added to the position of the footprint for resolve Z fighting conflict with the ground display.")]
    [Range(0.0f, 0.1f)]
    [SerializeField]
    private float zFightingSolver = 0.01f;

    // PRIVATE PROPERTIES
    private bool rightFoot = true;
    private float distanceMoved = 0;
    private Vector3 positionLastFrame;
    private Vector3 originMovingObject;

    private MeshFilter meshFilter;
    #endregion

    void Start () {
        movingObject = transform.parent.gameObject.transform;
        originMovingObject = movingObject.position;
        transform.parent = null;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(1, 1, 1);
        // Reset mesh filter
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();

        // Assign foot material to mesh renderer
        GetComponent<MeshRenderer>().sharedMaterial = footMaterial;

        // Initialize position last frame
        positionLastFrame = movingObject.position;
    }

    // Update is called once per frame
    void Update () {
        if(fpc != null)
        {
            if (!fpc.OSOL())
                return;
        }
        // If the moving object haven't move at all, do nothing. (It can't let any footprint)
        if(movingObject.position != positionLastFrame)
        {
            // Add the distance done by the moving object since the last frame (only horizontaly (x+z))
            distanceMoved += Mathf.Abs(movingObject.position.x - positionLastFrame.x) + Mathf.Abs(movingObject.position.z - positionLastFrame.z);
            // If we have go throught a full step, try to see if we can put a footprint on the ground
            if(distanceMoved >= distanceStep)
            {
                // Debug
                if (drawRayInEditor)
                    Debug.DrawRay(movingObject.position, Vector3.down, Color.red, 5);
                // Create a footprint only if we have a reasonable distance from the ground.
                RaycastHit raycastHit = new RaycastHit();
                if (Physics.Raycast(new Ray(movingObject.position, Vector3.down), out raycastHit, groundDistance, groundLayers))
                {
                    Debug.Log(raycastHit.distance);
                    Vector3 offset = new Vector3(0, -raycastHit.distance + zFightingSolver, 0);
                    CreateNewFootprint(offset);
                }
                // Change current foot and reset distance between steps
                rightFoot = !rightFoot;
                distanceMoved -= distanceStep;
            }
            // Remember the position of the moving object for next frame
            positionLastFrame = movingObject.position;
        }
    }

    #region Methods
    /// <summary>
    /// Create a new footprint (horizontal quad mesh with a material)
    /// </summary>
    public void CreateNewFootprint(Vector3 quadOffset)
    {
        // Set offset right/left
        quadOffset += (rightFoot ? (Vector3.right * leftRightOffset) : (Vector3.left * leftRightOffset));

        // Create GameObject Footprint
        GameObject newFootprint = new GameObject();

        // - - Mesh filter
        MeshFilter newMeshFilter = newFootprint.AddComponent<MeshFilter>();
        newMeshFilter.mesh = CreateHorizontalQuadMesh(quadOffset, footScale, rightFoot);
        // - - Mesh renderer
        MeshRenderer newMeshRenderer = newFootprint.AddComponent<MeshRenderer>();
        newMeshRenderer.material = footMaterial;

        // Combine
        CombineInstance combineInstance = new CombineInstance();
        combineInstance.mesh = newMeshFilter.mesh;
        combineInstance.transform = movingObject.localToWorldMatrix;

        CombineInstance combineInstance2 = new CombineInstance();
        combineInstance2.mesh = meshFilter.mesh;
        combineInstance2.transform = transform.localToWorldMatrix;

        CombineInstance[] combineInstances = { combineInstance, combineInstance2 };
        Mesh combinaison = new Mesh();
        combinaison.CombineMeshes(combineInstances);
        meshFilter.mesh = combinaison;

        Destroy(newFootprint);
    }

    /// <summary>
    /// Create an horizontal quad mesh with a scale "scale" at the position "offset". Can invert the uv with "invertUV".
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public Mesh CreateHorizontalQuadMesh(Vector3 offset, Vector2 scale, bool invertUV)
    {
        // Create mesh
        Mesh quad = new Mesh();

        // - - - Vertices
        // Initialization
        Vector3[] vertices = new Vector3[4];
        // Vertices
        vertices[0] = offset + new Vector3(0, 0, 0);
        vertices[1] = offset + new Vector3(scale.x, 0, 0);
        vertices[2] = offset + new Vector3(0, 0, scale.y);
        vertices[3] = offset + new Vector3(scale.x, 0, scale.y);
        // Assignation
        quad.vertices = vertices;

        // - - - Triangles
        // Initialization
        int[] tri = new int[6];
        // Triangle 1
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        // Triangle 2
        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;
        // Assignation
        quad.triangles = tri;

        // - - - Normals
        // Initialization
        Vector3[] normals = new Vector3[4];
        // Normals     
        normals[0] = Vector3.up;
        normals[1] = Vector3.up;
        normals[2] = Vector3.up;
        normals[3] = Vector3.up;
        // Assignation
        quad.normals = normals;

        // - - - UVs
        // Initialization
        Vector2[] uv = new Vector2[4];
        // UVs
        if(!invertUV)
        {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);
        }
        else
        {
            uv[0] = new Vector2(1, 0);
            uv[1] = new Vector2(0, 0);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(0, 1);
        }
        // Assignation
        quad.uv = uv;

        // Return mesh
        return quad;
    }
    #endregion
}
