using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
  public ClickEvent OnClick;
  public LayerMask ObstacleMask;
  public GameObject Ground;
  public int NodesPerLine = 1;

  private GridNode[,] mGridNodes;
  private Vector3 mNodeSize;
  private const float NodeHeight = 0.25f;
  private const float CollisionRadius = 0.25f;

  float bottomLeftX => Ground.transform.position.x - Ground.transform.localScale.x / 2 + mNodeSize.x / 2;
  float bottomLeftY => Ground.transform.position.y;
  float bottomLeftZ => Ground.transform.position.z - Ground.transform.localScale.z / 2 + mNodeSize.z / 2;

  void Start()
  {
    mGridNodes = new GridNode[NodesPerLine, NodesPerLine];

    mNodeSize = GetNodeSize();

    for (int i = 0; i < NodesPerLine; i++)
    {
      for (int j = 0; j < NodesPerLine; j++)
      {
        var pos = new Vector3(bottomLeftX + mNodeSize.x * i, bottomLeftY, bottomLeftZ + mNodeSize.z * j);
        mGridNodes[i, j] = new GridNode(pos, mNodeSize, i, j);
        mGridNodes[i,j].Occupied = Physics.CheckSphere(pos, CollisionRadius, ObstacleMask);
        mGridNodes[i, j].Color = mGridNodes[i, j].Occupied ? Color.red : Color.white;
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

    return mGridNodes[coords.z, coords.x];
  }

  public void UpdateNode(GridNode node, Action<GridNode> updateFunction)
  {
    var actualNode = mGridNodes[node.CoordZ, node.CoordX];
    updateFunction(actualNode);
  }

  public bool IsGridNodeOccupied(GridNode node)
  {
    return GetNodeFromPosition(node.Position.x, node.Position.z).Occupied;
  }

  public void ResetNodes()
  {
    foreach(var node in mGridNodes)
    {
      node.ResetCost();
      node.ResetColor();
    }
  }

  public List<GridNode> GetNearNodes(GridNode node)
  {
    var list = new List<GridNode>();
    for(int i = -1; i <= 1; i++)
    {
      for (int j = -1; j <= 1; j++)
      {
        if(node.CoordX + i >= 0 && node.CoordX + i <= mGridNodes.Length)
        {
          if (node.CoordZ + j >= 0 && node.CoordZ + j <= mGridNodes.Length)
          {
            var currentNode = mGridNodes[node.CoordX + i, node.CoordZ + j];
            if (currentNode != node)
            {
              list.Add(currentNode);
            }
          }
        }
      }
    }
    return list;
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
    //InvokeOnAllNodes(node => node.ResetColor());

    var coords = GetCoordinatesFromPosition(x, z);

    OnClick?.Invoke(mGridNodes[coords.z, coords.x]);
    mGridNodes[coords.x, coords.z].Color = Color.blue;
    Debug.Log($"X: {coords.x} | Z: {coords.z}");
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    if (mGridNodes != null)
    {
      foreach (var node in mGridNodes)
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
    foreach (var node in mGridNodes)
    {
      act(node);
    }
  }
}