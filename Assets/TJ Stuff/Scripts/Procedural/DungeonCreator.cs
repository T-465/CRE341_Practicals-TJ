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
  


    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    public float minDistanceFromWall;
    public float minDistanceFromHatch;
    public float minDistanceFromProp;
    public float minDistanceFromTorch;
    [SerializeField] int numberofProps = 10;
    [SerializeField] int numberofNPCs = 5;
    [SerializeField] int numberofTorches = 4;
    [SerializeField] int dungeonWidth = 15;
    [SerializeField] int dungeonLength = 15;
    [SerializeField] int roomWidthMin = 3;
    [SerializeField] int roomLengthMin = 3;
    [SerializeField] int maxIterations = 10;
    

    public NavMeshSurface navMeshSurface;
    void Start()
    {
        
        if (singleton == null)
        {
            singleton = GameObject.FindWithTag("singleton").GetComponent<Singleton>();
        }
        

        // Ensure parameters are set before creating the dungeon
         InitializeDungeonParameters();
        CreateDungeon();
    }

    public void InitializeDungeonParameters()
    {
    
        if (singleton.levelsComplete == 1)
        {
            numberofProps = 12;
            numberofNPCs = 6;
            numberofTorches = 5;
            dungeonWidth = 50;
            dungeonLength = 40;
            roomWidthMin = 4;
            roomLengthMin = 4;
            maxIterations = 1;
        }
        else if (singleton.levelsComplete == 2)
        {
            numberofProps = 15;
            numberofNPCs = 8;
            numberofTorches = 6;
            dungeonWidth = 50;
            dungeonLength = 42;
            roomWidthMin = 5;
            roomLengthMin = 5;
            maxIterations = 2;
        
        }
        else if (singleton.levelsComplete == 3)
        {
            numberofProps = 18;
            numberofNPCs = 10;
            numberofTorches = 8;
            dungeonWidth = 50;
            dungeonLength = 40;
            roomWidthMin = 6;
            roomLengthMin = 6;
            maxIterations = 3;
        }
        else if (singleton.levelsComplete == 4)
        {
            numberofProps = 20;
            numberofNPCs = 10;
            numberofTorches = 10;
            dungeonWidth = 50;
            dungeonLength = 45;
            roomWidthMin = 7;
            roomLengthMin = 7;
            maxIterations = 4;
        }
        else if (singleton.levelsComplete >= 5)
        {
            numberofProps = 20;
            numberofNPCs = 10;
            numberofTorches = 10;
            dungeonWidth = 50;
            dungeonLength = 50;
            roomWidthMin = 8;
            roomLengthMin = 8;
            maxIterations = 5;
        }
    }

    public void CreateDungeon()
    {
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
