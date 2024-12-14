using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Document;

    public class DocumentService(IGenericRepository<Domain.Entities.Document> _documentRepository,
        IMapper _mapper,
        IUnitOfWork _unitOfWork):IDocumentService
    {
        public async Task<DocumentResponse> CreateAsync(DocumentRequest dto)
        {
            var entity = _mapper.Map<Domain.Entities.Document>(dto);
            await _documentRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            var outDto = _mapper.Map<DocumentResponse>(entity);
            return outDto;
        }

        public async Task<DocumentResponse> UpdateAsync(Guid id, DocumentRequest dto)
        {
            var entity = await _documentRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if (entity is null) throw new NotFoundException("Document not found");
            var entityToUpdate = _mapper.Map<Domain.Entities.Document>(dto);
            entityToUpdate.Id = entity.Id;
            _documentRepository.Update(entityToUpdate);
            _unitOfWork.SaveChanges();
            var outDto = _mapper.Map<DocumentResponse>(entity);
            return outDto;
        }

        public async Task<DocumentResponse> RemoveAsync(Guid id)
        {
            var entity = await _documentRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if (entity is null) throw new NotFoundException("Document not found");
            _documentRepository.Remove(entity);
            _unitOfWork.SaveChanges();
            var outDto = _mapper.Map<DocumentResponse>(entity);
            return outDto;
        }

        public async Task<DocumentResponse> GetAsync(Guid id)
        {
            var entity = await _documentRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if (entity is null) throw new NotFoundException("Document not found");
            var outDto = _mapper.Map<DocumentResponse>(entity);
            return outDto;
        }

        public async Task<IList<DocumentResponse>> GetAllAsync(RequestFilter? filter)
        {
            var entities = await _documentRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
            var outDtos = _mapper.Map<IList<DocumentResponse>>(entities);
            return outDtos;
        }
}