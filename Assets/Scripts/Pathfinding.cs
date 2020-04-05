using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
  public Grid NodeGrid;
  private GridNode mCurrentNode;

  // Start is called before the first frame update
  void Start()
  {
    if (NodeGrid != null)
    {
      mCurrentNode = NodeGrid.GetNodeFromPosition(transform.position.x, transform.position.z);

      NodeGrid.UpdateNode(mCurrentNode, node => node.Color = Color.green);
      foreach(var nearNode in mCurrentNode.NearNodes)
      {
        NodeGrid.UpdateNode(nearNode, node => node.Color = Color.yellow);
      }
    }
  }

  public void OnGridClicked(GridNode node)
  {
    mCurrentNode.DistanceToTarget = Vector3.Distance(mCurrentNode.CenterPoint, node.CenterPoint);
    NodeGrid.UpdateNode(node, updateNode => updateNode.Color = Color.blue);
    UpdateDistanceToTarget(node, mCurrentNode);
    //NodeGrid.UpdateNode(mCurrentNode.NearNodes.First(x => x.DistanceToTarget == mCurrentNode.NearNodes.Min(y => y.DistanceToTarget)), updateNode => updateNode.Color = Color.black);
  }

  private void UpdateDistanceToTarget(GridNode target, GridNode node)
  {
    NodeGrid.UpdateNode(node, updateNode => updateNode.Color = Color.black);
    if (node.DistanceToTarget > 1f)
    {
      foreach (var nearNode in node.NearNodes)
      {
        nearNode.DistanceToSource = 0;
        nearNode.DistanceToTarget = Vector3.Distance(nearNode.CenterPoint, target.CenterPoint);
      }
      UpdateDistanceToTarget(target, node.NearNodes.First(x => x.DistanceToTarget == node.NearNodes.Min(y => y.DistanceToTarget)));
    }
  }
}