using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Auth;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using LearningManagementSystem.Application.Abstractions.Services.Token;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly AdminSettings _AdminSettings;

    public AuthService(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ITokenHandler tokenHandler,
        IEmailService emailService,
        IMapper mapper,
        IOptions<AdminSettings> settings, RoleManager<AppRole> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _emailService = emailService;
        _mapper = mapper;
        _roleManager = roleManager;
        _AdminSettings = settings.Value;
    }

    public async Task<Token> SignInAsync(SignInRequest requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user == null && (requestDto.UserName != _AdminSettings.UserName &&
                             requestDto.Password != _AdminSettings.Password))
            throw new BadRequestException("Username or password is incorrect");
       
        if ((requestDto.UserName == _AdminSettings.UserName &&
             requestDto.Password == _AdminSettings.Password))
        {
            var adminUserResult =
                await _userManager.CreateAsync(new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = _AdminSettings.UserName
                }, requestDto.Password);
            user = await _userManager.FindByNameAsync(requestDto.UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Contains("Admin"))
            {
                await _roleManager.CreateAsync(new AppRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin"
                });
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        var result =
            await _signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
        if (!result.Succeeded) throw new BadRequestException("Username or password is incorrect");
        Token token = await _tokenHandler.CreateAccessToken(30, user);
        return token;
    }

    public async Task<Token> SignUpAsync(SignUpRequest requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is not null) throw new BadRequestException("This user already exits");
        var userToCreate = _mapper.Map<AppUser>(requestDto);
        userToCreate.Id = Guid.NewGuid().ToString();
        var result = await _userManager.CreateAsync(userToCreate, requestDto.Password);
        if (!result.Succeeded)
            throw new BadRequestException($"User couldn't  be created:{result.Errors.FirstOrDefault().Description}");
        Token token = await _tokenHandler.CreateAccessToken(30, userToCreate);
        var activationLink =
            $"http://localhost:5208/api/Auth/confirm-email?email={userToCreate.Email}&token={token}";
        await _emailService.SendEmailAsync(activationLink, "Confirm email", userToCreate.Email);
        return token;
    }

    public async Task ConfirmEmailAsync(string email, string token)
    {
        token = token.Replace(" ", "+");

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null) throw new BadRequestException("Invalid email address");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded) throw new BadRequestException("Email couldn't be confirmed");
    }
}