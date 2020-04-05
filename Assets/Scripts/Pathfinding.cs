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
    NodeGrid.UpdateNode(node, updateNode => updateNode.Color = Color.blue);
    foreach(var nearNode in mCurrentNode.NearNodes)
    {
      nearNode.DistanceToSource = 0;
      nearNode.DistanceToTarget = Vector3.Distance(nearNode.CenterPoint, node.CenterPoint);
    }
    NodeGrid.UpdateNode(mCurrentNode.NearNodes.First(x => x.DistanceToTarget == mCurrentNode.NearNodes.Min(y => y.DistanceToTarget)), updateNode => updateNode.Color = Color.black);
  }
}