using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Theme;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Theme;

public class ThemeService(
    IGenericRepository<Domain.Entities.Theme> _themeRepository,
    IDocumentService _documentService,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
        IUnitOfWork _unitOfWork):IThemeService
    {
        public async Task<ThemeResponse> CreateAsync(ThemeRequest dto)
        {
            var entity = _mapper.Map<Domain.Entities.Theme>(dto);
            await _themeRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            await _documentService.CreateByOwnerAsync(new DocumentByOwner(new() { dto.File }, entity.Id,
                DocumentType.Theme));
            return _mapper.Map<ThemeResponse>(entity);
        }

        public async Task<ThemeResponse> UpdateAsync(Guid id, ThemeRequest dto)
        {
            var entity = await _themeRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("Theme not found");
            _mapper.Map(dto, entity);
            _themeRepository.Update(entity);
            _unitOfWork.SaveChanges();
            var document = await _documentService.GetByOwnerId(id);
           /*await _documentService.UpdateAsync(document.Id, new DocumentRequest(id,
               DocumentType.Theme,dto.File.,new List<IFormFile>(){dto.File}));*/
            return _mapper.Map<ThemeResponse>(entity);
        }

        public async Task<ThemeResponse> RemoveAsync(Guid id)
        {
            var entity = await _themeRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("Theme not found");
            _themeRepository.Remove(entity);
            _unitOfWork.SaveChanges();
            return _mapper.Map<ThemeResponse>(entity);
        }

        public async Task<ThemeResponse> GetAsync(Guid id)
        {
            string key = $"member-{id}";
            var data = _redisCachingService.GetData<ThemeResponse>(key);
            if (data is not null)
                return data;
            var entity = await _themeRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if(entity is null) throw new NotFoundException("Theme not found");
            var outDto=_mapper.Map<ThemeResponse>(entity);
            _redisCachingService.SetData(key, outDto);
            return outDto;
        }

        public async Task<IList<ThemeResponse>> GetAllAsync(RequestFilter? filter)
        {
            var entities =await _themeRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
            return _mapper.Map<IList<ThemeResponse>>(entities);
        }

        public async Task<ThemeResponse> ActivateAsync(Guid id)
        {
            var entity = await _themeRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            entity.IsActive=!entity.IsActive;
            _themeRepository.Update(entity);
            _unitOfWork.SaveChanges();
            return _mapper.Map<ThemeResponse>(entity);
        }
    }