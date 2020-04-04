using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
  public LayerMask ObstacleMask;
  public int NodesPerLine = 1;

  private GridNode[,] GridNodes;
  private Vector3 mNodeSize;
  private const float NodeHeight = 0.25f;
  private const float CollisionRadius = 0.05f;

  float bottomLeftX => transform.position.x - transform.localScale.x / 2 + mNodeSize.x / 2;
  float bottomLeftY => transform.position.y;
  float bottomLeftZ => transform.position.z - transform.localScale.z / 2 + mNodeSize.z / 2;

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
        GridNodes[i, j].Color = Physics.CheckBox(pos, new Vector3(CollisionRadius, CollisionRadius, CollisionRadius), transform.rotation, ObstacleMask) ? Color.red : Color.white;
      }
    }
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

    var idxX = (int)Mathf.Floor(Mathf.Abs(transform.position.x - transform.localScale.x / 2 - x) / mNodeSize.x);
    var idxZ = (int)Mathf.Floor(Mathf.Abs(transform.position.z - transform.localScale.z / 2 - z) / mNodeSize.z);

    GridNodes[idxX, idxZ].Color = Color.blue;
    Debug.Log($"X: {idxX} | Z: {idxZ}");
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
      Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
  }

  private Vector3 GetNodeSize()
  {
    if (NodesPerLine > 0)
    {
      var width = transform.localScale.x;
      var height = transform.localScale.z;

      return new Vector3(width / NodesPerLine, NodeHeight, height / NodesPerLine);
    }

    Debug.LogError("Nodes per line must be larger than 0!");
    return Vector3.one;
  }

  private void InvokeOnAllNodes(Action<GridNode> act)
  {
    foreach(var node in GridNodes)
    {
      act(node);
    }
  }
}