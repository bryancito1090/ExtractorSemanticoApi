using ExtractorSemanticoApi.Application.Features.ExtractedData.Command;
using ExtractorSemanticoApi.Application.Features.ExtractedData.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtractorSemanticoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExtractedDataController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExtractedDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateExtractedData(
        [FromBody] CreateUpdateExtractedDataCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("by-review/{reviewId}")]
    public async Task<IActionResult> GetExtractedDataByReviewId(int reviewId)
    {
        var result = await _mediator.Send(new GetExtractedDataByReviewIdQuery(reviewId));
        return Ok(result);
    }
}
