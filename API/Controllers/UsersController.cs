using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(DataContext context) : ControllerBase
{
    // Old way to get data context
    // private readonly DataContext _context;

    // public UsersController(DataContext context)
    // {
    //     // this.context = context;
    //     _context = context;
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        //_context
        var users = await context.Users.ToListAsync();
        // return Ok(users);
        return users;
    }

        [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        //_context
        var user = await context.Users.FindAsync(id);
        if(user == null) return NotFound();
        return user;
    }

}
