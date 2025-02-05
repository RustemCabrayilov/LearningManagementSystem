using Amazon.S3;
using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Storage.Aws;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Document;

public class DocumentService(
    IGenericRepository<Domain.Entities.Document> _documentRepository,
    IMapper _mapper,
    IRedisCachingService _redisCachingService,
    IAwsStorage awsStorage,
    IUnitOfWork _unitOfWork) : IDocumentService
{
    public async Task<DocumentResponse> CreateAsync(DocumentRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Document>(dto);
        await _documentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var outDto = _mapper.Map<DocumentResponse>(entity);
        return outDto;
    }

    public async Task<List<DocumentResponse>> CreateByOwnerAsync(DocumentByOwner documentByOwner)
    {
        foreach (var file in documentByOwner.Files)
        {
            /*string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
            var localPath = _environment.ContentRootPath;
            var directoryPath = Path.Combine("Documents",$"{documentByOwner.documentType.ToString()}s",fileName);
            var fullPath = Path.Combine(localPath,directoryPath);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }*/
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var key = await awsStorage.UploadFileAsync(new FileRequest(file, fileName, $"{documentByOwner.DocumentType}"));
            var document = new Domain.Entities.Document()
            {
                OwnerId = documentByOwner.OwnerId,
                FileName = fileName,
                OriginName = file.FileName,
                Key = key,
                DocumentType = documentByOwner.DocumentType,
            };
            await _documentRepository.AddAsync(document);
            await _unitOfWork.SaveChangesAsync();
        }

        var entities = await _documentRepository.GetAll(x => x.OwnerId == documentByOwner.OwnerId && !x.IsDeleted, null)
            .ToListAsync();
        return _mapper.Map<List<DocumentResponse>>(entities);
    }

    public async Task<DocumentResponse> UpdateAsync(Guid id, DocumentRequest dto)
    {
        var entity = await _documentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Document not found");
        var entityToUpdate = _mapper.Map<Domain.Entities.Document>(dto);
        entityToUpdate.Id = entity.Id;
        _documentRepository.Update(entityToUpdate);
        _unitOfWork.SaveChanges();
        foreach (var file in dto.Files)
        {
            await awsStorage.UpdateFileAsync(
                new FileRequest(file, entityToUpdate.FileName, $"{entityToUpdate.DocumentType}"), entity.Key);
        }

        var outDto = _mapper.Map<DocumentResponse>(entity);
        return outDto;
    }

    public async Task<DocumentResponse> RemoveAsync(Guid id)
    {
        var entity = await _documentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Document not found");
        _documentRepository.Remove(entity);
        _unitOfWork.SaveChanges();
       
        bool result = await awsStorage.DeleteFileAsync(entity.Key);
        if (result is false) throw new Exception("Document cannot be deleted");
        var outDto = _mapper.Map<DocumentResponse>(entity);
        return outDto;
    }

    public async Task<DocumentResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<DocumentResponse>(key);
        if (data is not null)
            return data;
        var entity = await _documentRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Document not found");
        string path = await awsStorage.GetFileUrlAsync($"{entity.Key}", $"{entity.DocumentType}");
        var outDto = _mapper.Map<DocumentResponse>(entity) with
        {
            Path = path
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<DocumentResponse> GetByOwnerId(Guid ownerId)
    {
        string key = $"member-{ownerId}";
        var data = _redisCachingService.GetData<DocumentResponse>(key);
        if (data is not null)
            return data;
        var entity = await _documentRepository.GetAsync(x => x.OwnerId == ownerId && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Document  not found");
        string path = await awsStorage.GetFileUrlAsync($"{entity.Key}", $"{entity.DocumentType}");
        var outDto = _mapper.Map<DocumentResponse>(entity) with
        {
            Path = path
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<DocumentResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _documentRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        var outDtos = _mapper.Map<IList<DocumentResponse>>(entities);
        return outDtos;
    }
}