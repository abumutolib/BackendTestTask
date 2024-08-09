namespace BackendTestTask.Models;

public class TreeNode
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentNodeId { get; set; }

    public TreeNode ParentNode { get; set; }

    public ICollection<TreeNode> Children { get; set; } = new List<TreeNode>();

    public int TreeId { get; set; }
}
