using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Identity.API.Infrastructure.Entities;
using Identity.API.Infrastructure.Errors;
using Identity.API.Infrastructure.Helpers;
using Identity.API.Infrastructure.Models;
using Identity.API.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Email = "test@email.com", HashedPassword = BCrypt.Net.BCrypt.HashPassword("test") }
    };

    private readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IOptions<AppSettings> appSettings, IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepository.GetUserByUsername(model.Username);

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
        // validate
        if (_users.Any(x => x.Username == model.Username))
            throw AppExceptionEnum.UserNotFound;

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        user = await _userRepository.CreateUser(user);

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
        return _users.FirstOrDefault(x => x.Id == id);
    }
}