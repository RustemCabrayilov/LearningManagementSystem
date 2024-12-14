using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public class DocumentMappingProfile:Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<DocumentRequest, Domain.Entities.Document>();
        CreateMap<Domain.Entities.Document, DocumentResponse>();
    }
}