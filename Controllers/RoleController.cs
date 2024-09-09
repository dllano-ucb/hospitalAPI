using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly PgDbContext _context;

    public RolesController(PgDbContext context)
    {
        _context = context;
    }

    // GET: api/Role
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    // GET: api/Roles/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        return role;
    }

    // POST: api/Roles
    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }

    // PUT: api/Roles/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, Role role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }

        var existingRole = await _context.Roles.FindAsync(id);
        if (existingRole == null)
        {
            return NotFound();
        }

        existingRole.Name = role.Name;
        existingRole.Status = role.Status;
        existingRole.UpdatedAt = DateTime.UtcNow;

        _context.Entry(existingRole).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(id))
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

    // DELETE: api/Roles/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RoleExists(int id)
    {
        return _context.Roles.Any(e => e.Id == id);
    }
}