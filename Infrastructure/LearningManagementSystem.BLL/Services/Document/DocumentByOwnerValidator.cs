using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Document;

namespace LearningManagementSystem.BLL.Services.Document;

public class DocumentByOwnerValidator:AbstractValidator<DocumentByOwner>
{
    public DocumentByOwnerValidator()
    {
        RuleFor(x=>x.DocumentType).NotEmpty();
        RuleFor(x=>x.OwnerId).NotNull();
        RuleFor(x=>x.Files).NotEmpty();
    }   
}