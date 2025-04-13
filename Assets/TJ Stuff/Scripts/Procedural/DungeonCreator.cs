using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class DungeonCreator : MonoBehaviour 
{
    public Singleton singleton;
  

    [SerializeField] int corridorWidth = 5;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset = 1;
    public GameObject wallVertical, wallHorizontal;

    #region NPCSpawning
    public GameObject npcPrefab, waypointsPrefab;
	[SerializeField] List<GameObject> npcs = new List<GameObject>();
    [SerializeField] List<GameObject> npcPrefabs = new List<GameObject>();


    #endregion


    [SerializeField] List<GameObject> props = new List<GameObject>();
    [SerializeField] List<GameObject> propPrefabs = new List<GameObject>();
    [SerializeField] int numberofHatches = 1;
    [SerializeField] List<GameObject> hatches = new List<GameObject>();
    [SerializeField] List<GameObject> hatchPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> torches = new List<GameObject>();
    [SerializeField] List<GameObject> torchPrefabs = new List<GameObject>();
  
  
    [SerializeField] GameObject playerSpawn;
    public bool start;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    public float minDistanceFromWall;
    public float minDistanceFromHatch;
    public float minDistanceFromProp;
    public float minDistanceFromTorch;
    [SerializeField] int numberofProps;
    [SerializeField] int numberofNPCs ;
    [SerializeField] int numberofTorches;
    [SerializeField] int dungeonWidth;
    [SerializeField] int dungeonLength ;
    [SerializeField] int roomWidthMin ;
    [SerializeField] int roomLengthMin ;
    [SerializeField] int maxIterations;
    

    public NavMeshSurface navMeshSurface;
    void Start()
    {
        StartCoroutine(WaitForSingletonAndInitialize());
    }

    private IEnumerator WaitForSingletonAndInitialize()
    {
        while (Singleton.singleton == null)
        {
            Debug.LogWarning("Wait for Singleton");
            yield return null;
        }

        singleton = Singleton.singleton;
        start = false;


        InitializeDungeonParameters();
        StartCoroutine(CreateDungeon());
    }

    public void InitializeDungeonParameters()
    {
        if (singleton == null)
        {
            singleton = GameObject.FindWithTag("singleton").GetComponent<Singleton>();
        }
        if (singleton.levelsComplete == 0)
        {
            numberofProps = 10;
            numberofNPCs = 5;
            numberofTorches = 5;
            dungeonWidth = 50;
            dungeonLength = 40;
            roomWidthMin = 10;
            roomLengthMin = 20;
            maxIterations = 5;
        }
    
        if (singleton.levelsComplete == 1)
        {
            numberofProps = 12;
            numberofNPCs = 6;
            numberofTorches = 5;
            dungeonWidth = 50;
            dungeonLength = 40;
            roomWidthMin = 12;
            roomLengthMin = 22;
            maxIterations = 6;
        }
        else if (singleton.levelsComplete == 2)
        {
            numberofProps = 15;
            numberofNPCs = 8;
            numberofTorches = 6;
            dungeonWidth = 50;
            dungeonLength = 42;
            roomWidthMin = 12;
            roomLengthMin = 24;
            maxIterations = 7;
        
        }
        else if (singleton.levelsComplete == 3)
        {
            numberofProps = 18;
            numberofNPCs = 10;
            numberofTorches = 8;
            dungeonWidth = 50;
            dungeonLength = 40;
            roomWidthMin = 14;
            roomLengthMin = 24;
            maxIterations = 8;
        }
        else if (singleton.levelsComplete == 4)
        {
            numberofProps = 20;
            numberofNPCs = 10;
            numberofTorches = 10;
            dungeonWidth = 50;
            dungeonLength = 45;
            roomWidthMin = 15;
            roomLengthMin = 25;
            maxIterations = 9;
        }
        else if (singleton.levelsComplete >= 5)
        {
            numberofProps = 20;
            numberofNPCs = 10;
            numberofTorches = 10;
            dungeonWidth = 50;
            dungeonLength = 50;
            roomWidthMin = 15;
            roomLengthMin = 25;
            maxIterations = 9;
        }
    }

    public IEnumerator CreateDungeon()
    {
        yield return new WaitForSeconds(2);
        DestroyAllChildren();
     
        DungeonGen generator = new DungeonGen(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);

        DestroyNPCS();
        DestroyHatch();
        DestroyProps();
        DestroyTorches();
       
        navMeshSurface.BuildNavMesh();
        SpawnNPCs();
        SpawnHatch();
        SpawnProps();
        SpawnTorches();
        SpawnPlayer();
    
    }
    private void SpawnNPCs()
{
    for (int i = 0; i < numberofNPCs; i++)
    {
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(0, dungeonWidth),
            0,
            UnityEngine.Random.Range(0, dungeonLength)
        );
        GameObject randomNpcPrefab = npcPrefabs[UnityEngine.Random.Range(0, npcPrefabs.Count)];
        GameObject npc = Instantiate(randomNpcPrefab, randomPosition, Quaternion.identity);
        npcs.Add(npc);
    }
   
}

private void SpawnProps()
{
    minDistanceFromWall = 3.5f;
    minDistanceFromHatch = 1.0f;

   
    for (int i = 0; i < numberofProps; i++)
    {
        Vector3 randomPosition;
        bool validPosition;
        Vector3 hatchPosition = hatches[0].transform.position; 
      


        do
        {
            validPosition = true;
            randomPosition = new Vector3(
                UnityEngine.Random.Range(0, dungeonWidth),
                0,
                UnityEngine.Random.Range(0, dungeonLength)
            );

            // Check distance from all wall positions
            foreach (var wallPosition in possibleWallHorizontalPosition)
            {
                if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                foreach (var wallPosition in possibleWallVerticalPosition)
                {
                    if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }
            
    if (validPosition)
    {
        if (Vector3.Distance(randomPosition, hatchPosition) < minDistanceFromHatch)
        {
            validPosition = false;
        }
    }
        } while (!validPosition);

        GameObject randomPropPrefab = propPrefabs[UnityEngine.Random.Range(0, propPrefabs.Count)];
        GameObject prop = Instantiate(randomPropPrefab, randomPosition, Quaternion.identity);
        props.Add(prop);
    }
}
  public void SpawnTorches()
{
    minDistanceFromWall = 5.0f;
    minDistanceFromHatch = 5.0f;
    minDistanceFromProp = 2.0f;
    minDistanceFromTorch = 10.0f;
 
   
    for (int i = 0; i < numberofTorches; i++)
    {
        Vector3 randomPosition;
        bool validPosition;
        Vector3 hatchPosition = hatches[0].transform.position;
    Vector3 propPosition = props[0].transform.position;
    Vector3 torchPosition;
   
        
        do
        {
            validPosition = true;
            randomPosition = new Vector3(
                UnityEngine.Random.Range(0, dungeonWidth),
                0,
                UnityEngine.Random.Range(0, dungeonLength)
            );
                foreach (var wallPosition in possibleWallHorizontalPosition)
            {
                if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                foreach (var wallPosition in possibleWallVerticalPosition)
                {
                    if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }

            // Check distance from the hatch
            if (Vector3.Distance(randomPosition, hatchPosition) < minDistanceFromHatch)
            {
                validPosition = false;
                break;
            }
             // Check distance from the props
            if (Vector3.Distance(randomPosition, propPosition) < minDistanceFromProp)
            {
                validPosition = false;
                break;
            }
            if (validPosition && torches.Count > 0)
            {
                
                // Check distance from the other torches
                foreach (var torch in torches)
                {
                    torchPosition = torches[0].transform.position;
                    if (Vector3.Distance(randomPosition, torchPosition) < minDistanceFromTorch)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }
            
        
        } while (!validPosition);

        if (validPosition)
        {
            GameObject randomTorchPrefab = torchPrefabs[UnityEngine.Random.Range(0, torchPrefabs.Count)];
            GameObject torch = Instantiate(randomTorchPrefab, randomPosition, Quaternion.identity);
            torches.Add(torch);
        }
     
    }
}

private void DestroyTorches()
{
    foreach (var torch in torches)
    {
        Destroy(torch);
    }
    torches.Clear();
}
public void SpawnPlayer()
{
    minDistanceFromWall = 5.0f;
    minDistanceFromHatch = 1.0f;
    minDistanceFromProp = 2.0f;
    minDistanceFromTorch = 3.0f;

    Vector3 randomPosition;
    bool validPosition;

    do
    {
        validPosition = true;
        randomPosition = new Vector3(
            UnityEngine.Random.Range(0, dungeonWidth),
            0,
            UnityEngine.Random.Range(0, dungeonLength)
        );

        // Check distance from all wall positions
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
            {
                validPosition = false;
                break;
            }
        }

        if (validPosition)
        {
            foreach (var wallPosition in possibleWallVerticalPosition)
            {
                if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                {
                    validPosition = false;
                    break;
                }
            }
        }

        // Check distance from the hatch
        if (validPosition && hatches.Count > 0)
        {
            Vector3 hatchPosition = hatches[0].transform.position;
            if (Vector3.Distance(randomPosition, hatchPosition) < minDistanceFromHatch)
            {
                validPosition = false;
            }
        }

        // Check distance from the props
        if (validPosition && props.Count > 0)
        {
            foreach (var prop in props)
            {
                if (Vector3.Distance(randomPosition, prop.transform.position) < minDistanceFromProp)
                {
                    validPosition = false;
                    break;
                }
            }
        }

        // Check distance from the torches
        if (validPosition && torches.Count > 0)
        {
            foreach (var torch in torches)
            {
                if (Vector3.Distance(randomPosition, torch.transform.position) < minDistanceFromTorch)
                {
                    validPosition = false;
                    break;
                }
            }
        }

    } while (!validPosition);
    if (playerSpawn != null)
    {
        Instantiate(playerSpawn, randomPosition, Quaternion.identity);
        Debug.Log($"Player spawned at {randomPosition}");
        start = true;
    }
    else
    {
        Debug.LogError("Player prefab not assigned");
    }
}

private void SpawnHatch()
{
    minDistanceFromWall = 4.5f;


    for (int i = 0; i < numberofHatches; i++)
    {
        Vector3 randomPosition;
        bool validPosition;

        do
        {
            validPosition = true;
            randomPosition = new Vector3(
                UnityEngine.Random.Range(0, dungeonWidth),
                0,
                UnityEngine.Random.Range(0, dungeonLength)
            );

            // Check distance from all wall positions
            foreach (var wallPosition in possibleWallHorizontalPosition)
            {
                if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                foreach (var wallPosition in possibleWallVerticalPosition)
                {
                    if (Vector3.Distance(randomPosition, wallPosition) < minDistanceFromWall)
                    {
                        validPosition = false;
                        break;
                    }
                }
            }
        } while (!validPosition);

        GameObject randomHatchPrefab = hatchPrefabs[UnityEngine.Random.Range(0, hatchPrefabs.Count)];
        GameObject hatch = Instantiate(randomHatchPrefab, randomPosition, Quaternion.identity);
        hatches.Add(hatch);
    }
}

private void DestroyProps()
{
    foreach (var prop in props)
    {
        Destroy(prop);
    }
    props.Clear();
}
private void DestroyHatch()
{
    foreach (var hatch in hatches)
    {
        Destroy(hatch);
    }

    hatches.Clear();
}


 private void DestroyNPCS()
    {
        foreach (var npc in npcs)
        {
            Destroy(npc);
        }
        npcs.Clear();
    }
    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        MeshCollider meshCollider = dungeonFloor.AddComponent<MeshCollider>();
        print(mesh.isReadable);
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)){
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
