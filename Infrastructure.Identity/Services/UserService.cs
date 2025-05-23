using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<PagedResponse<List<UserDto>>> GetAllUsersAsync(int pageNumber, int pageSize, string? search = null)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => EF.Functions.Like(u.UserName, $"%{search}%")
                          || EF.Functions.Like(u.Email, $"%{search}%"));
            }

            var totalUsers = query.Count();
            var users = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return new PagedResponse<List<UserDto>>(userDtos, pageNumber, pageSize, totalUsers);
        }

        public async Task<List<string>> GetAllUserNamesAsync()
        {
            return await _userManager.Users
                .Select(user => user.UserName)
                .ToListAsync();
        }

        public async Task<List<UserSelectionDto>> GetAllUserIdAndNamesAsync()
        {
            return await _userManager.Users
                .Select(user => new UserSelectionDto
                {
                    Id = user.Id,
                    UserName = user.UserName
                })
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetUsersByIdsAsync(List<string> userIds)
        {
            return await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };

            return userDto;
        }

        public async Task<Response<string>> CreateUserAsync(RegisterRequest request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                throw new ApiException("Email already registered");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new ApiException("Failed to create user");

            if (request.Roles != null && request.Roles.Any())
            {
                var validRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                var invalidRoles = request.Roles.Except(validRoles).ToList();
                if (invalidRoles.Any())
                    throw new ApiException($"Invalid roles: {string.Join(", ", invalidRoles)}");

                var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
                if (!roleResult.Succeeded)
                    throw new ApiException("Failed to assign roles to user");
            }

            return new Response<string>(user.Id, "User created successfully");
        }

        public async Task<Response<string>> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("User not found");

            // Update username and email
            user.UserName = request.UserName ?? user.UserName;
            user.Email = request.Email ?? user.Email;

            // Update user in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new ApiException("Failed to update user details");

            // Update roles if provided
            if (request.Roles != null && request.Roles.Any())
            {
                var existingRoles = await _userManager.GetRolesAsync(user);
                var validRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                // Validate if all roles in request exist in the system
                var invalidRoles = request.Roles.Except(validRoles).ToList();
                if (invalidRoles.Any())
                    throw new ApiException($"Invalid roles: {string.Join(", ", invalidRoles)}");

                // Remove old roles and add new ones
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
                await _userManager.AddToRolesAsync(user, request.Roles);
            }

            return new Response<string>(user.Id, "User updated successfully");
        }

        public async Task<Response<string>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ApiException("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new ApiException("Failed to delete user");

            return new Response<string>(userId, "User deleted successfully");
        }
    }
}
