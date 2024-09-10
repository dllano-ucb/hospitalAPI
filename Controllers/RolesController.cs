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

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRole()
    {
        return await _context.Roles.ToListAsync();
    }

    // GET: api/Users/{id}
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

    // POST: api/Users
    [HttpPost]    
    public async Task<ActionResult<Role>> CreateRole(Role role)
    {
        // Encrypt the password before saving
        //user.PasswordHash = HashPassword(user.PasswordHash);

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }



    // PUT: api/Users/{id}
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

        // Encrypt the password if it has been changed
        /*if (existingUser.PasswordHash != user.PasswordHash)
        {
            existingUser.PasswordHash = HashPassword(user.PasswordHash);
        }*/

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

    private bool RoleExists(int id)
    {
        throw new NotImplementedException();
    }

    // DELETE: api/Users/{id}
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

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    // Utility function to hash the password
    /*private string HashPassword(string password)
    {
        // Generate a salt
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        // Return the hashed password with the salt
        return $"{Convert.ToBase64String(salt)}:{hashed}";
    }*/
}
