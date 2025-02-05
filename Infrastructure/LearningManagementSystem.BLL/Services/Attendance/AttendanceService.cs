using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Attendance;

public class AttendanceService(
    IGenericRepository<Domain.Entities.Attendance> _AttendanceRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IAttendanceService
{
    public async Task<AttendanceResponse> CreateAsync(AttendanceRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Attendance>(dto);
        await _AttendanceRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse[]> UpdateRangeAsync(AttendanceRequest[] dtos)
    {
        var entities = new List<Domain.Entities.Attendance>();
        foreach (var dto in dtos)
        {
            var entity = await _AttendanceRepository.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
            _mapper.Map(dto, entity);
            _AttendanceRepository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        return _mapper.Map<AttendanceResponse[]>(entities);
    }

    public async Task<AttendanceResponse> UpdateAsync(Guid id, AttendanceRequest dto)
    {
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        _mapper.Map(dto, entity);
        _AttendanceRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse> RemoveAsync(Guid id)
    {
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        _AttendanceRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<AttendanceResponse>(key);
        if (data is not null)
            return data;
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        var outDto=_mapper.Map<AttendanceResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<AttendanceResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _AttendanceRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<AttendanceResponse>>(entities);
    }
}