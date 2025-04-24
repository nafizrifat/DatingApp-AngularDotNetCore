using System;
using System;
using System.Numerics;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAsync();
    Task<IEnumerable<AppUser>> GetAllUserAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);

    Task<IEnumerable<MemberDto>> GetMemberAsync();
    Task<MemberDto?> GetMemberAsync(string username);


}
