using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public GameObject leftWallPrefab;
    public GameObject rightWallPrefab;
    public GameObject itemPointPrefab;
    public GameObject obstaclePrefab;
    public GameObject leftBoundaryColliderPrefab; // Separate prefab for the left boundary
    public GameObject rightBoundaryColliderPrefab; // Separate prefab for the right boundary

    public int numberOfTiles = 10; // Number of tiles to be active at a time
    public float tileLength = 20.0f;
    public float wallOffset = 10.0f; // Adjust this value to place the walls correctly

    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeLeftWalls = new List<GameObject>();
    private List<GameObject> activeRightWalls = new List<GameObject>();
    private List<GameObject> activeLeftBoundaries = new List<GameObject>(); // List for left boundary colliders
    private List<GameObject> activeRightBoundaries = new List<GameObject>(); // List for right boundary colliders
    private float spawnZ = 0.0f;
    private float safeZone = 20.0f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile(i < 1); // Spawn empty tiles for the initial path
        }
    }

    void Update()
    {
        if (playerTransform.position.z - safeZone > (spawnZ - numberOfTiles * tileLength))
        {
            SpawnTile(false);
            DeleteTile();
        }
    }

    void SpawnTile(bool empty)
    {
        GameObject tile = Instantiate(groundTilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(tile);

        // Instantiate left and right walls
        Vector3 leftWallPosition = new Vector3(-wallOffset, 0.5f, spawnZ);
        Vector3 rightWallPosition = new Vector3(wallOffset, 0.5f, spawnZ);

        GameObject leftWall = Instantiate(leftWallPrefab, leftWallPosition, Quaternion.identity);
        GameObject rightWall = Instantiate(rightWallPrefab, rightWallPosition, Quaternion.identity);

        activeLeftWalls.Add(leftWall);
        activeRightWalls.Add(rightWall);

        // Instantiate boundary colliders
        GameObject leftBoundary = Instantiate(leftBoundaryColliderPrefab, leftWallPosition, Quaternion.identity);
        GameObject rightBoundary = Instantiate(rightBoundaryColliderPrefab, rightWallPosition, Quaternion.identity);
        activeLeftBoundaries.Add(leftBoundary);
        activeRightBoundaries.Add(rightBoundary);

        spawnZ += tileLength;

        if (!empty)
        {
            List<Vector3> usedPositions = new List<Vector3>();
            SpawnObstacles(tile, usedPositions);
            SpawnPoints(tile, usedPositions);
        }
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);

        Destroy(activeLeftWalls[0]);
        activeLeftWalls.RemoveAt(0);

        Destroy(activeRightWalls[0]);
        activeRightWalls.RemoveAt(0);

        // Destroy boundary colliders
        Destroy(activeLeftBoundaries[0]);
        activeLeftBoundaries.RemoveAt(0);
        Destroy(activeRightBoundaries[0]);
        activeRightBoundaries.RemoveAt(0);
    }

    void SpawnObstacles(GameObject tile, List<Vector3> usedPositions)
    {
        int obstacleCount = Random.Range(1, 3);
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 position;
            do
            {
                position = tile.transform.position + new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(1f, tileLength - 1f));
            } while (IsPositionTooClose(position, usedPositions));

            usedPositions.Add(position);
            GameObject obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
            obstacle.transform.SetParent(tile.transform);
            obstacle.transform.localScale = Vector3.one; // Ensure correct scale
        }
    }

    void SpawnPoints(GameObject tile, List<Vector3> usedPositions)
    {
        int pointCount = Random.Range(1, 5);
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 position;
            do
            {
                position = tile.transform.position + new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(1f, tileLength - 1f));
            } while (IsPositionTooClose(position, usedPositions));

            usedPositions.Add(position);
            GameObject point = Instantiate(itemPointPrefab, position, Quaternion.identity);
            point.transform.SetParent(tile.transform);
            point.transform.localScale = Vector3.one; // Ensure correct scale
        }
    }

    bool IsPositionTooClose(Vector3 position, List<Vector3> usedPositions)
    {
        foreach (Vector3 usedPosition in usedPositions)
        {
            if (Vector3.Distance(position, usedPosition) < 2.0f) // Minimum distance between objects
            {
                return true;
            }
        }
        return false;
    }
}
