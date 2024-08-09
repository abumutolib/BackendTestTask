using BackendTestTask.Data;
using BackendTestTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendTestTask.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TreeNodeController : ControllerBase
{
    private readonly AppDbContext _context;

    public TreeNodeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TreeNode>> GetNode(int id)
    {
        var node = await _context.TreeNodes.FindAsync(id);

        if (node == null)
        {
            return NotFound();
        }

        return node;
    }

    [HttpPost]
    public async Task<ActionResult<TreeNode>> CreateNode(TreeNode node)
    {
        _context.TreeNodes.Add(node);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNode), new { id = node.Id }, node);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNode(int id, TreeNode node)
    {
        if (id != node.Id)
        {
            return BadRequest();
        }

        _context.Entry(node).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NodeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNode(int id)
    {
        var node = await _context.TreeNodes.FindAsync(id);
        if (node == null)
        {
            return NotFound();
        }

        _context.TreeNodes.Remove(node);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool NodeExists(int id)
    {
        return _context.TreeNodes.Any(e => e.Id == id);
    }
}
