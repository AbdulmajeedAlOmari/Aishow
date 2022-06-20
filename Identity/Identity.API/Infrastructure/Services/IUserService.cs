﻿using AutoMapper;
using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Entities;
using Identity.API.Infrastructure.Enums;
using Identity.API.Infrastructure.Errors;
using Identity.API.Infrastructure.Helpers;
using Identity.API.Infrastructure.Models;
using Identity.API.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace Identity.API.Infrastructure.Services;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<AuthenticateResponse> Register(RegisterRequest model);
    Task<List<User>> GetAll();
    User GetById(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new List<User>
    {
        new User { UserId = 1, FirstName = "Test", LastName = "User", Username = "test", Email = "test@email.com", HashedPassword = BCrypt.Net.BCrypt.HashPassword("test") }
    };

    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IdentityDataContext _context;

    public UserService(IOptions<AppSettings> appSettings, IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, IdentityDataContext context)
    {
        _appSettings = appSettings.Value;
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _context = context;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepository.GetUserWithRolesByUsername(model.Username);

        // Throw exception in case user not found
        if (user == null)
        {
            throw AppExceptionEnum.UserOrPasswordIncorrect;
        }

        bool verified = BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword);

        // Throw exception in case password is incorrect
        if (!verified)
        {
            throw AppExceptionEnum.UserOrPasswordIncorrect;
        }

        // Authentication successful, so generate jwt token
        var token = JwtTokenUtils.GenerateJwtToken(user, _appSettings.Secret);

        // Return token
        return new AuthenticateResponse(user, token);
    }

    public async Task<AuthenticateResponse> Register(RegisterRequest model)
    {
        // Validate user does not exist
        if (_users.Any(x => x.Username == model.Username))
            throw AppExceptionEnum.UserNotFound;

        // Map model to new user object
        var user = _mapper.Map<User>(model);

        // Hash password
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // Create user
        user = await _userRepository.CreateUser(user);

        // Add default role to user
        Role role = (await _roleRepository.GetRole(Roles.Member.RoleId))!;
        user.Roles.Add(role);

        // Save all changes to database in one transaction
        await _context.SaveChangesAsync();

        // Generate token for user
        var token = JwtTokenUtils.GenerateJwtToken(user, _appSettings.Secret);

        // Return successful response including token
        return new AuthenticateResponse(user, token);
    }

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetUsers();
    }

    public User GetById(int id)
    {
        return _users.FirstOrDefault(x => x.UserId == id);
    }
}