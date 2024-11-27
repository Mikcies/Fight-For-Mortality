using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NodeConnector : MonoBehaviour
{
    [SerializeField]
    private List<Transform> nodes; 

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (nodes == null || nodes.Count == 0)
        {
            Debug.LogWarning("No nodes assigned to NodeConnector.");
            return;
        }

        lineRenderer.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(i, nodes[i].position);
        }

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
    }

    void OnDrawGizmos()
    {
        if (nodes != null && nodes.Count > 1)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
            }
        }
    }
}
