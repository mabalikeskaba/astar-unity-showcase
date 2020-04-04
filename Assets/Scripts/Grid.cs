using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
  public LayerMask ObstacleMask;
  public GameObject Ground;
  public int NodesPerLine = 1;

  private GridNode[,] GridNodes;
  private Vector3 mNodeSize;
  private const float NodeHeight = 0.25f;
  private const float CollisionRadius = 0.25f;

  float bottomLeftX => Ground.transform.position.x - Ground.transform.localScale.x / 2 + mNodeSize.x / 2;
  float bottomLeftY => Ground.transform.position.y;
  float bottomLeftZ => Ground.transform.position.z - Ground.transform.localScale.z / 2 + mNodeSize.z / 2;

  void Start()
  {
    GridNodes = new GridNode[NodesPerLine, NodesPerLine];

    mNodeSize = GetNodeSize();

    for (int i = 0; i < NodesPerLine; i++)
    {
      for (int j = 0; j < NodesPerLine; j++)
      {
        var pos = new Vector3(bottomLeftX + mNodeSize.x * i, bottomLeftY, bottomLeftZ + mNodeSize.z * j);
        GridNodes[i, j] = new GridNode(pos, mNodeSize);
        GridNodes[i, j].Color = Physics.CheckSphere(pos, CollisionRadius, ObstacleMask) ? Color.red : Color.white;
        
      }
    }

    AddNearestNodes();
  }

  private void AddNearestNodes()
  {
    for (int i = 0; i < NodesPerLine; i++)
    {
      for (int j = 0; j < NodesPerLine; j++)
      {
        // Left
        if (j - 1 > 0)
          GridNodes[i, j].NearNodes.Add(GridNodes[i, j - 1]);
        // Right
        if (j + 1 < NodesPerLine)
          GridNodes[i, j].NearNodes.Add(GridNodes[i, j + 1]);
        // Bottom
        if (i - 1 >= 0)
          GridNodes[i, j].NearNodes.Add(GridNodes[i - 1, j]);
        // Bottom Left
        if (i - 1 >= 0 && j - 1 > 0)
          GridNodes[i, j].NearNodes.Add(GridNodes[i - 1, j - 1]);
        // Bottom Right
        if (i - 1 >= 0 && j + 1 < NodesPerLine)
          GridNodes[i, j].NearNodes.Add(GridNodes[i - 1, j + 1]);
        // Top
        if (i + 1 < NodesPerLine)
          GridNodes[i, j].NearNodes.Add(GridNodes[i + 1, j]);
        // Top Left
        if (i + 1 < NodesPerLine && j - 1 > 0)
          GridNodes[i, j].NearNodes.Add(GridNodes[i + 1, j - 1]);
        // Top Right
        if (i + 1 < NodesPerLine && j + 1 < NodesPerLine)
          GridNodes[i, j].NearNodes.Add(GridNodes[i + 1, j + 1]);
      }
    }
  }

  public (int x, int z) GetCoordinatesFromPosition(float x, float z)
  {
    var idxX = (int)Mathf.Floor(Mathf.Abs(Ground.transform.position.x - Ground.transform.localScale.x / 2 - x) / mNodeSize.x);
    var idxZ = (int)Mathf.Floor(Mathf.Abs(Ground.transform.position.z - Ground.transform.localScale.z / 2 - z) / mNodeSize.z);

    return (idxX, idxZ);
  }

  public GridNode GetNodeFromPosition(float x, float z)
  {
    var coords = GetCoordinatesFromPosition(x, z);

    return GridNodes[coords.z, coords.x];
  }

  public void UpdateNode(GridNode node, Action<GridNode> updateFunction)
  {
    var actualNode = GetNodeFromPosition(node.Position.x, node.Position.z);
    updateFunction(actualNode);
  }

  private void Update()
  {
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Input.GetMouseButtonDown(0))
    {
      Physics.Raycast(ray, out var hitInfo);
      Debug.Log($"HitInfo: {hitInfo.point.x} | {hitInfo.point.z}");
      UpdateTile(hitInfo.point.x, hitInfo.point.z);
    }
  }

  private void UpdateTile(float x, float z)
  {
    InvokeOnAllNodes(node => node.ResetColor());

    var coords = GetCoordinatesFromPosition(x, z);

    GridNodes[coords.x, coords.z].Color = Color.blue;
    Debug.Log($"X: {coords.x} | Z: {coords.z}");
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    if (GridNodes != null)
    {
      foreach (var node in GridNodes)
      {
        Gizmos.color = node.Color;
        Gizmos.DrawCube(node.Position, node.Size);
      }
    }
    else
    {
      Gizmos.DrawWireCube(Ground.transform.position, Ground.transform.localScale);
    }
  }

  private Vector3 GetNodeSize()
  {
    if (NodesPerLine > 0)
    {
      var width = Ground.transform.localScale.x;
      var height = Ground.transform.localScale.z;

      return new Vector3(width / NodesPerLine, NodeHeight, height / NodesPerLine);
    }

    Debug.LogError("Nodes per line must be larger than 0!");
    return Vector3.one;
  }

  private void InvokeOnAllNodes(Action<GridNode> act)
  {
    foreach (var node in GridNodes)
    {
      act(node);
    }
  }
}