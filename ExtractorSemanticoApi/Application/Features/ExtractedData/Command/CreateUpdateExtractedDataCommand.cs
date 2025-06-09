using ExtractorSemanticoApi.Application.Dto.ExtractedData;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.ExtractedData.Command;


public record CreateUpdateExtractedDataCommand(
    int? DataId,
    int ReviewId,
    string Type,
    string Value,
    string? Subtype,
    Dictionary<string, object>? Metadata
) : IRequest<ExtractedDataResponseDto>;

public class CreateUpdateExtractedDataCommandHandler
    : IRequestHandler<CreateUpdateExtractedDataCommand, ExtractedDataResponseDto>
{
    private readonly IExtractedDataRepository _extractedDataRepository;

    public CreateUpdateExtractedDataCommandHandler(IExtractedDataRepository extractedDataRepository)
    {
        _extractedDataRepository = extractedDataRepository;
    }

    public async Task<ExtractedDataResponseDto> Handle(
        CreateUpdateExtractedDataCommand request,
        CancellationToken cancellationToken)
    {
        var requestDto = new ExtractedDataRequestDto(
            request.DataId,
            request.ReviewId,
            request.Type,
            request.Value,
            request.Subtype,
            request.Metadata
        );

        return await _extractedDataRepository.CreateUpdateExtractedData(requestDto);
    }
}