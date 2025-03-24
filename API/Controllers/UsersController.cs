using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController(DataContext context) : BaseApiController
{
    // Old way to get data context
    // private readonly DataContext _context;

    // public UsersController(DataContext context)
    // {
    //     // this.context = context;
    //     _context = context;
    // }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        //_context
        var users = await context.Users.ToListAsync();
        // return Ok(users);
        return users;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        //_context
        var user = await context.Users.FindAsync(id);
        if(user == null) return NotFound();
        return user;
    }

}
