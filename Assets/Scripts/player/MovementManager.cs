using System.Collections.Generic;
using UnityEngine;

public class NodeMovementManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> nodes;
    [SerializeField]
    Transform player;
    [SerializeField]
    float offsetY = 1.0f; 
    private int currentNodeIndex = 0;

    void Start()
    {
        currentNodeIndex = PlayerPrefs.GetInt("CurrentNodeIndex", 0);
        MovePlayerToNode(currentNodeIndex);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToNode(currentNodeIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNode(currentNodeIndex + 1);
        }
    }

    void MoveToNode(int newNodeIndex)
    {
        if (newNodeIndex >= 0 && newNodeIndex < nodes.Count)
        {
            currentNodeIndex = newNodeIndex;
            MovePlayerToNode(currentNodeIndex);
        }
    }

    void MovePlayerToNode(int nodeIndex)
    {
        Vector3 targetPosition = nodes[nodeIndex].position;
        targetPosition.y += offsetY; 
        player.position = targetPosition;
    }
}
