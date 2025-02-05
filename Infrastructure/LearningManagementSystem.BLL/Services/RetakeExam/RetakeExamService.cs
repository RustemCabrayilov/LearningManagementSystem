using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.RetakeExam;

public class RetakeExamService(  
    IGenericRepository<Domain.Entities.RetakeExam> _RetakeExamRepository,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) :IRetakeExamService
{
        public async Task<RetakeExamResponse> CreateAsync(RetakeExamRequest dto)
        {
            var entity = _mapper.Map<Domain.Entities.RetakeExam>(dto);
            await _RetakeExamRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            /*foreach (var file in dto.Files)
            {
                string fileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                var localPath = _environment.ContentRootPath;
                var directoryPath = Path.Combine("Documents","RetakeExams",fileName);
                var fullPath = Path.Combine(localPath,directoryPath);
                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                var document = new Domain.Entities.Document()
                {
                    OwnerId = entity.Id,
                    FileName = fileName,
                    OriginName = file.FileName,
                    Path = directoryPath,
                    DocumentType = DocumentType.RetakeExam,
                };
                await _documentRepository.AddAsync(document);
                await _unitOfWork.SaveChangesAsync();
            }*/
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<RetakeExamResponse> UpdateAsync(Guid id, RetakeExamRequest dto)
        {
            var entity = await _RetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("RetakeExam not found");
            _mapper.Map(dto, entity);
             _RetakeExamRepository.Update(entity);
             _unitOfWork.SaveChanges();
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<RetakeExamResponse> RemoveAsync(Guid id)
        {
            var entity = await _RetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("RetakeExam not found");
            _RetakeExamRepository.Remove(entity);
            _unitOfWork.SaveChanges();
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<RetakeExamResponse> GetAsync(Guid id)
        {
            string key = $"member-{id}";
            var data = _redisCachingService.GetData<RetakeExamResponse>(key);
            if (data is not null)
                return data;
            var entity = await _RetakeExamRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if(entity is null) throw new NotFoundException("RetakeExam not found");
            entity.Exam=await _examRepository.GetAsync(x => x.Id == entity.ExamId&&!x.IsDeleted);
            var outDto = _mapper.Map<RetakeExamResponse>(entity);
            _redisCachingService.SetData(key, outDto);
            return outDto;
        }

        public async Task<IList<RetakeExamResponse>> GetAllAsync(RequestFilter? filter)
        {
            var entities =await _RetakeExamRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
            return _mapper.Map<IList<RetakeExamResponse>>(entities);
        }
        
}