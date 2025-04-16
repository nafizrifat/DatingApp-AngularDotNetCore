using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var a = context.Users.Find(-1);
        if(a==null) return NotFound();
        return a;
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        var a = context.Users.Find(-1) ?? throw new Exception("Some ERROR happned");
        return a;
    }

    [HttpGet("bad-request")]
    public ActionResult<AppUser> GetBadRequest()
    {
        return BadRequest("This is BAD request");
    }


}
