using System;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository (DataContext context,IMapper mapper): IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllUserAsync()
    {
        return await context.Users
        .Include(x=>x.Pothos)
        .ToListAsync();
    }

    public async Task<IEnumerable<MemberDto>> GetMemberAsync()
    {
        return await context.Users
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        .ToListAsync();

    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
       .Where(x => x.UserName == username)
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
       return await context.Users
       .FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
        .Include(x=>x.Pothos)
        .SingleOrDefaultAsync(x=>x.UserName==username);
    }

    public async Task<bool> SaveAsync()
    {
        return await context.SaveChangesAsync()>0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
