using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]

public class UsersController(IUnitOfWork unitOfWork, IMapper mapper,
    IPhotoService photoService) : BaseApiController
{
    // Old way to get data context
    // private readonly DataContext _context;

    // public UsersController(DataContext context)
    // {
    //     // this.context = context;
    //     _context = context;
    // }

    // [Authorize(Roles ="Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        //_context
        var users = await unitOfWork.UserRepository.GetMemberAsync(userParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }

    [Authorize(Roles ="Member")]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //_context
        var user = await unitOfWork.UserRepository.GetMemberAsync(username);
        if (user == null) return NotFound();
        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Can not find user");

        mapper.Map(memberUpdateDto, user);
        if (await unitOfWork.Complete())
            return NoContent();
        return BadRequest("Failded to update the user");

    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        if (user.Pothos.Count == 0) photo.IsMain = true;

        user.Pothos.Add(photo);

        if (await unitOfWork.Complete())
            return CreatedAtAction(nameof(GetUser),
                new { username = user.UserName }, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("Could not find the user");

        var photo = user.Pothos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Pothos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await unitOfWork.Complete()) return NoContent();

        return BadRequest("Problem setting main photo");
    }

[HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        // var user = await unitOfWork.unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
         var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("User not found");

        // var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);
        var photo = user.Pothos.FirstOrDefault(x => x.Id == photoId);


        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Pothos.Remove(photo);

        // if (await unitOfWork.Complete()) return Ok();
        if (await unitOfWork.Complete()) return Ok();

        return BadRequest("Problem deleting photo");
    }

}
