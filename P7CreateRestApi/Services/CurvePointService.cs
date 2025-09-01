using Microsoft.EntityFrameworkCore.Metadata;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Dto;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public interface ICurvePointService
    {
        Task<List<CurvePointDto>> GetAllAsync();
        Task<CurvePointDto?> GetByIdAsync(int id);
        Task<CurvePointDto> CreateAsync(CurvePointDto curvePointDto);
        Task<CurvePointDto?> UpdateAsync(int id, CurvePointDto curvePointDto);
        Task<bool> DeleteAsync(int id);
    }


    public class CurvePointService : ICurvePointService
    {
        private readonly IRepository<CurvePoint> _repository;

        public CurvePointService(IRepository<CurvePoint> repository)
        {
            _repository = repository;
        }

        public async Task<List<CurvePointDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync()
                .ContinueWith(task => task.Result
                    .Select(curvePoint => new CurvePointDto
                    {
                        Id = curvePoint.Id,
                        AsOfDate = curvePoint.AsOfDate,
                        Term = curvePoint.Term,
                        CurvePointValue = curvePoint.CurvePointValue,
                        CreationDate = curvePoint.CreationDate
                    })
                    .ToList());
        }

        public async Task<CurvePointDto?> GetByIdAsync(int id)
        {
            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                return null;
            }
            return new CurvePointDto
            {
                Id = curvePoint.Id,
                AsOfDate = curvePoint.AsOfDate,
                Term = curvePoint.Term,
                CurvePointValue = curvePoint.CurvePointValue,
                CreationDate = curvePoint.CreationDate
            };
        }

        public async Task<CurvePointDto> CreateAsync(CurvePointDto curvePointDto)
        {
            var curvePoint = new CurvePoint
            {
                AsOfDate = curvePointDto.AsOfDate,
                Term = curvePointDto.Term,
                CurvePointValue = curvePointDto.CurvePointValue,
                CreationDate = curvePointDto.CreationDate
            };

            var created = await _repository.AddAsync(curvePoint);

            return new CurvePointDto
            {
                Id = created.Id,
                AsOfDate = created.AsOfDate,
                Term = created.Term,
                CurvePointValue = created.CurvePointValue,
                CreationDate = created.CreationDate
            };
        }

        public async Task<CurvePointDto?> UpdateAsync(int id, CurvePointDto curvePointDto)
        {
            var existingCurvePoint = await _repository.GetByIdAsync(id);
            if (existingCurvePoint == null)
            {
                return null;
            }

            existingCurvePoint.Id = id;
            existingCurvePoint.CreationDate = curvePointDto.CreationDate;
            existingCurvePoint.AsOfDate = curvePointDto.AsOfDate;
            existingCurvePoint.CurvePointValue = curvePointDto.CurvePointValue;
            existingCurvePoint.Term = curvePointDto.Term;

            var updated = await _repository.UpdateAsync(existingCurvePoint);

            return new CurvePointDto
            {
                Id = updated.Id,
                AsOfDate = updated.AsOfDate,
                Term = updated.Term,
                CurvePointValue = updated.CurvePointValue,
                CreationDate = updated.CreationDate
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingCurvePoint = await _repository.GetByIdAsync(id);
            if (existingCurvePoint != null)
            {
                _repository.Remove(existingCurvePoint);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
