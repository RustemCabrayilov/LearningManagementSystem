using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Document;

namespace LearningManagementSystem.BLL.Services.Document;

public class DocumentValidator:AbstractValidator<DocumentRequest>
{
    public DocumentValidator()
    {
        RuleFor(x=>x.Path).NotEmpty();
        RuleFor(x=>x.OwnerId).NotNull();
        RuleFor(x=>x.FileName).NotEmpty();
        RuleFor(x=>x.OriginName).NotEmpty();
    }   
}