using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] blockPrefabs;
    public Transform player;
    public float spawnDistance = 5f;
    public float junctionSpacing = 20f; 
    private Vector3 lastPlayerPosition;
    private List<GameObject> activeBlocks = new List<GameObject>();

    void Start()
    {
        lastPlayerPosition = player.position;
        SpawnInitialBlocks();
    }

    void Update()
    {
        // Check if the player has moved far enough to spawn new blocks
        if (Vector3.Distance(player.position, lastPlayerPosition) >= spawnDistance)
        {
            Vector2Int currentJunctionIndex = CalculateJunctionIndex(player.position);
            Vector2Int lastJunctionIndex = CalculateJunctionIndex(lastPlayerPosition);

            if (currentJunctionIndex != lastJunctionIndex)
            {
                UpdateBlocks(currentJunctionIndex, lastJunctionIndex);
                lastPlayerPosition = player.position;
            }
        }
    }
    
    void SpawnInitialBlocks()
    {
        Vector2Int currentJunctionIndex = CalculateJunctionIndex(player.position);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 spawnPosition = CalculatePositionFromJunctionIndex(currentJunctionIndex) + new Vector3(i * junctionSpacing, 0f, j * junctionSpacing);
                SpawnBlockAtPosition(spawnPosition);
            }
        }
    }
    
    Vector2Int CalculateJunctionIndex(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / junctionSpacing),
            Mathf.FloorToInt(position.z / junctionSpacing)
        );
    }

    Vector3 CalculatePositionFromJunctionIndex(Vector2Int junctionIndex)
    {
        return new Vector3(junctionIndex.x * junctionSpacing, 0f, junctionIndex.y * junctionSpacing);
    }

    void UpdateBlocks(Vector2Int currentJunctionIndex, Vector2Int lastJunctionIndex)
    {
        Vector2Int movementDirection = currentJunctionIndex - lastJunctionIndex;
        // Deactivate blocks in the opposite direction of movement
        for (int i = 0; i <= 2; i++)
        {
            Vector2Int blockIndex = lastJunctionIndex - movementDirection + new Vector2Int(movementDirection.y, movementDirection.x) * i; 
            Vector3 blockPosition = CalculatePositionFromJunctionIndex(blockIndex);
            GameObject block = GetBlockAtPosition(blockPosition);
            if (block != null)
            {
                block.SetActive(false);
                activeBlocks.Remove(block);
            }
        }
        // Activate blocks in the direction of movement
        for (int i = 0; i <= 2; i++){
            Vector2Int blockIndex = lastJunctionIndex + movementDirection + new Vector2Int(movementDirection.y, movementDirection.x) * i; 
            Vector3 blockPosition = CalculatePositionFromJunctionIndex(blockIndex);
            GameObject block = GetBlockAtPosition(blockPosition);
            if (block == null)
            {
                // If no block exists at this position, spawn one
                block = SpawnBlockAtPosition(blockPosition);
            }
            else
            {
                // If the block already exists, reactivate it
                block.SetActive(true);
                activeBlocks.Add(block);
            }
        }
    }

    GameObject GetBlockAtPosition(Vector3 position)
    {
        foreach (GameObject block in activeBlocks)
        {
            if (block.transform.position == position)
            {
                return block;
            }
        }
        return null;
    }

    GameObject SpawnBlockAtPosition(Vector3 position)
    {
        // Instantiate a random block prefab at the specified position
        GameObject newBlock = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Length)], position, Quaternion.identity);
        activeBlocks.Add(newBlock);
        return newBlock;
    }
}
