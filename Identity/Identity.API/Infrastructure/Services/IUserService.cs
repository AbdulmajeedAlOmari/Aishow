using AutoMapper;
using Common.API.Models.Entities;
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
    Task<AuthenticateResponse> Login(AuthenticateRequest model);
    Task<AuthenticateResponse> Register(RegisterRequest model);
    Task<List<User>> GetAll();
    Task<CommonUserDto> GetById(int id);
    Task<CommonUserDto> GetUserFromToken();
}

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IdentityDataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IOptions<AppSettings> appSettings, IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository, IdentityDataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthenticateResponse> Login(AuthenticateRequest model)
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
        // if (_users.Any(x => x.Username == model.Username))
        //     throw AppExceptionEnum.UserNotFound;

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

    public async Task<CommonUserDto> GetById(int id)
    {
        // Fetch user object from HTTP context
        User user = await _userRepository.GetUser(id);

        // Map user to common user dto object
        var userDto = _mapper.Map<CommonUserDto>(user);

        return userDto;
    }

    public async Task<CommonUserDto> GetUserFromToken()
    {
        // Fetch user object from HTTP context
        CommonUserDto user = (CommonUserDto) _httpContextAccessor.HttpContext?.Items["User"]!;

        return user;
    }
}