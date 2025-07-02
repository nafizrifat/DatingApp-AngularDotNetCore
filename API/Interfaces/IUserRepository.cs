using System;
using System;
using System.Numerics;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAsync();
    Task<IEnumerable<AppUser>> GetAllUserAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);

    Task<PagedList<MemberDto>> GetMemberAsync(UserParams userParams);
    Task<MemberDto?> GetMemberAsync(string username);


}
