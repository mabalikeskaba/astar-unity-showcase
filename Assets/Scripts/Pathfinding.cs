using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
  public Grid NodeGrid;
  private List<GridNode> mOpenList;
  private List<GridNode> mClosedList;

  // Start is called before the first frame update
  void Start()
  {
    if (NodeGrid != null)
    {
      var currentNode = GetCurrentActiveNode();

      NodeGrid.UpdateNode(currentNode, node => node.Color = Color.green);
      foreach (var nearNode in NodeGrid.GetNearNodes(currentNode))
      {
        NodeGrid.UpdateNode(nearNode, node => node.Color = Color.yellow);
      }
    }
  }

  public void OnGridClicked(GridNode targetNode)
  {
    NodeGrid.UpdateNode(targetNode, updateNode => updateNode.Color = Color.blue);
    DoTheStar(GetCurrentActiveNode(), targetNode);
  }

  private void DoTheStar(GridNode startNode, GridNode targetNode)
  {
    NodeGrid.ResetNodes();

    mOpenList = new List<GridNode> { startNode };
    mClosedList = new List<GridNode>();

    while (mOpenList.Count > 0)
    {
      var currentNode = mOpenList.FirstOrDefault(x => x.FCost <= mOpenList.Min(y => y.FCost));
      mOpenList.Remove(currentNode);
      mClosedList.Add(currentNode);

      if (currentNode != null)
        NodeGrid.UpdateNode(currentNode, updateNode => updateNode.Color = Color.cyan);
      if (currentNode == targetNode)
      {
        return;
      }

      CheckOnNearNodes(startNode, targetNode, currentNode, currentNode.FCost);
    }
  }

  private void CheckOnNearNodes(GridNode startNode, GridNode targetNode, GridNode currentNode, float currentFCost)
  {
    foreach (var nearNode in NodeGrid.GetNearNodes(currentNode))
    {
      if(!mClosedList.Contains(nearNode) && !NodeGrid.IsGridNodeOccupied(nearNode))
      {
        nearNode.Parent = currentNode;
        nearNode.GCost += GetDistance(currentNode, nearNode);
        nearNode.HCost = GetDistance(nearNode, targetNode);

        if (mOpenList.Contains(nearNode) && mOpenList.FirstOrDefault(x => x == nearNode) is GridNode node && nearNode.GCost > node.GCost)
          continue;

        mOpenList.Add(nearNode);
        NodeGrid.UpdateNode(nearNode, updateNode => updateNode.Color = Color.grey);
      }
    }
  }

  private float GetDistance(GridNode a, GridNode b)
  {
    return Mathf.Pow(a.CoordX - b.CoordX, 2) + Mathf.Pow(a.CoordZ - b.CoordZ, 2);
  }

  private GridNode GetCurrentActiveNode() => NodeGrid?.GetNodeFromPosition(transform.position.x, transform.position.z);
}