using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
  public Grid NodeGrid;
  private List<GridNode> mOpenList;
  private List<GridNode> mClosedList;
  private List<GridNode> mWaypointList;

  // Start is called before the first frame update
  void Start()
  {
    mWaypointList = new List<GridNode>();
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
    DoTheStar(GetCurrentActiveNode(), targetNode);
  }

  private void Update()
  {
    if (mWaypointList.Count > 0)
    {
      var waypoint = mWaypointList.First();
      var time = Time.deltaTime * 3.5f;
      var targetPoint = waypoint.CenterPoint - new Vector3(transform.localScale.x, 0, transform.localScale.y);
      var currentPoint = new Vector3(transform.position.x, 0, transform.position.z);
      var pos = Vector3.MoveTowards(currentPoint, targetPoint, time);
      if (Vector3.Distance(currentPoint, targetPoint) < 0.05f)
      {
        mWaypointList.Remove(waypoint);
      }
      transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }
  }

  private void DoTheStar(GridNode startNode, GridNode targetNode)
  {
    if (!targetNode.Occupied)
    {
      NodeGrid.ResetNodes();

      mOpenList = new List<GridNode> { startNode };
      mClosedList = new List<GridNode>();

      while (mOpenList.Count > 0)
      {
        var currentNode = mOpenList.FirstOrDefault(x => x.FCost <= mOpenList.Min(y => y.FCost));
        mOpenList.Remove(currentNode);
        mClosedList.Add(currentNode);

        if (currentNode == targetNode)
        {
          NodeGrid.UpdateNode(targetNode, updateNode => updateNode.Color = Color.blue);
          if (mClosedList.Count > 0)
          {
            mWaypointList.Clear();
            mWaypointList.AddRange(mClosedList);
            mWaypointList.Remove(mWaypointList.First());
          }
          return;
        }

        if (currentNode != null)
          NodeGrid.UpdateNode(currentNode, updateNode => updateNode.Color = Color.cyan);

        CheckOnNearNodes(startNode, targetNode, currentNode, currentNode.FCost);
      }
    }
  }

  private void CheckOnNearNodes(GridNode startNode, GridNode targetNode, GridNode currentNode, float currentFCost)
  {
    foreach (var nearNode in NodeGrid.GetNearNodes(currentNode))
    {

      if (!mClosedList.Contains(nearNode) && !NodeGrid.IsGridNodeOccupied(nearNode))
      {
        nearNode.Parent = currentNode;
        nearNode.GCost += GetDistance(currentNode, nearNode);
        nearNode.HCost = GetDistance(nearNode, targetNode);

        if (mOpenList.Contains(nearNode) && mOpenList.FirstOrDefault(x => x == nearNode) is GridNode node && nearNode.GCost > node.GCost)
          continue;
        NodeGrid.UpdateNode(nearNode, updateNode => updateNode.Color = Color.grey);
        mOpenList.Add(nearNode);
      }
    }
  }

  private float GetDistance(GridNode a, GridNode b)
  {
    return Mathf.Pow(a.CoordX - b.CoordX, 2) + Mathf.Pow(a.CoordZ - b.CoordZ, 2);
  }

  private GridNode GetCurrentActiveNode() => NodeGrid?.GetNodeFromPosition(transform.position.z, transform.position.x);
}