using System;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]

public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    // Old way to get data context
    // private readonly DataContext _context;

    // public UsersController(DataContext context)
    // {
    //     // this.context = context;
    //     _context = context;
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        //_context
        var users = await userRepository.GetMemberAsync();
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //_context
        var user = await userRepository.GetMemberAsync(username);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) return BadRequest();

        var user = await userRepository.GetUserByUsernameAsync(username);

        if (user == null) return BadRequest("Can not find user");

        mapper.Map(memberUpdateDto, user);
        if (await userRepository.SaveAsync())
            return NoContent();
        return BadRequest("Failded to update the user");

    }

}
