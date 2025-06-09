using ExtractorSemanticoApi.Application.Features.Sentiments.Command;
using ExtractorSemanticoApi.Application.Features.Sentiments.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtractorSemanticoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SentimentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SentimentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateSentiment([FromBody] CreateUpdateSentimentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("by-review/{reviewId}")]
    public async Task<IActionResult> GetSentimentsByReviewId(int reviewId)
    {
        var result = await _mediator.Send(new GetSentimentsByReviewIdQuery(reviewId));
        return Ok(result);
    }

    [HttpPost("analyze/{reviewId}")]
    public async Task<IActionResult> AnalyzeSentiments(int reviewId)
    {
        await _mediator.Send(new AnalyzeSentimentsCommand(reviewId));
        return Ok(new { Message = "Sentiment analysis completed" });
    }

    [HttpDelete("by-review/{reviewId}")]
    public async Task<IActionResult> DeleteSentiments(int reviewId)
    {
        await _mediator.Send(new DeleteSentimentsCommand(reviewId));
        return Ok(new { Message = "Sentiments deleted successfully" });
    }
}