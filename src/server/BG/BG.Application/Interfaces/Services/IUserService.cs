﻿using BG.Application.DTOs.User;

namespace BG.Application.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse> AddAsync(CreateUserDto createUserDto);
    Task<ApiResponse> DeleteAsync(int userId, int id);
    Task<ApiResponse> GetAsync(UserFilterDto userFilterDto);
    Task<ApiResponse> GetByIdAsync(int id);
    Task<ApiResponse> UpdateAsync(int id, UpdateUserDto updateUserDto);
}