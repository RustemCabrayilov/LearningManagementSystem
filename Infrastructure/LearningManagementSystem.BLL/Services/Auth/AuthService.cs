using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Auth;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace LearningManagementSystem.BLL.Services.Auth;

public class AuthService(
    /*SignInManager<AppUser> _signInManager,*/
    UserManager<AppUser> _userManager,
    IEmailService _emailService,
    IMapper _mapper) : IAuthService
{
    public async Task SignInAsync(SignInRequest requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user == null) throw new BadRequestException("Username or password is incorrect");
        /*var result =
            await _signInManager.PasswordSignInAsync(user, requestDto.Password, requestDto.IsPersistent, false);
        if (!result.Succeeded) throw new BadRequestException("Username or password is incorrect");*/
    }

    public async Task SignUpAsync(SignUpRequest requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is not null) throw new BadRequestException("This user already exits");
        var userToCreate = _mapper.Map<AppUser>(requestDto);
        userToCreate.Id = Guid.NewGuid().ToString();
        var result = await _userManager.CreateAsync(userToCreate, requestDto.Password);
        if (!result.Succeeded)
            throw new BadRequestException($"User couldn't  be created:{result.Errors.FirstOrDefault().Description}");
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userToCreate);
        var activationLink =
            $"http://localhost:5208/api/Auth/ConfirmEmail?email={userToCreate.Email}&token={emailConfirmationToken}";
        await _emailService.SendEmailAsync(userToCreate.Email, "Confirm email", activationLink);
        /*return _mapper.Map<UserResponseDto>(userToCreate);*/
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