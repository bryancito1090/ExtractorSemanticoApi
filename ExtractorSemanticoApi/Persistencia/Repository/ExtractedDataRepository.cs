using ExtractorSemanticoApi.Application.Dto.ExtractedData;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace ExtractorSemanticoApi.Persistencia.Repository;


public class ExtractedDataRepository : IExtractedDataRepository
{
    private readonly ExtractorsemanticoContext _context;

    public ExtractedDataRepository(ExtractorsemanticoContext context)
    {
        _context = context;
    }

    public async Task<ExtractedDataResponseDto> CreateUpdateExtractedData(ExtractedDataRequestDto request)
    {
        ExtractedDatum entity;

        if (request.DataId.HasValue && request.DataId > 0)
        {
            // Actualización
            entity = await _context.ExtractedData
                .FirstOrDefaultAsync(ed => ed.DataId == request.DataId)
                ?? throw new KeyNotFoundException($"ExtractedData with ID {request.DataId} not found");

            entity.Type = request.Type;
            entity.Value = request.Value;
            entity.Subtype = request.Subtype;
            entity.Metadata = request.Metadata != null
                ? JsonConvert.SerializeObject(request.Metadata)
                : null;

            _context.ExtractedData.Update(entity);
        }
        else
        {
            // Creación
            entity = new ExtractedDatum
            {
                ReviewId = request.ReviewId,
                Type = request.Type,
                Value = request.Value,
                Subtype = request.Subtype,
                Metadata = request.Metadata != null
                    ? JsonConvert.SerializeObject(request.Metadata)
                    : null
            };

            await _context.ExtractedData.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        return new ExtractedDataResponseDto(
            entity.DataId,
            entity.ReviewId,
            entity.Type,
            entity.Value,
            entity.Subtype,
            entity.Metadata != null
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(entity.Metadata)
                : null
        );
    }

    public async Task<List<ExtractedDataResponseDto>> GetExtractedDataByReviewId(int reviewId)
    {
        return await _context.ExtractedData
            .Where(ed => ed.ReviewId == reviewId)
            .Select(ed => new ExtractedDataResponseDto(
                ed.DataId,
                ed.ReviewId,
                ed.Type,
                ed.Value,
                ed.Subtype,
                ed.Metadata != null
                    ? JsonConvert.DeserializeObject<Dictionary<string, object>>(ed.Metadata)
                    : null
            ))
            .ToListAsync();
    }
}