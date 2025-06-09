using ExtractorSemanticoApi.Application.Features.TextProcessing.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtractorSemanticoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TextProcessingController : ControllerBase
{
    private readonly IMediator _mediator;

    public TextProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process/{reviewId}")]
    public async Task<IActionResult> ProcessReviewText(int reviewId)
    {
        await _mediator.Send(new ProcessReviewTextCommand(reviewId));
        return Ok(new { Message = "Review text processing started successfully" });
    }

    [HttpPost("analyze-sentiments/{reviewId}")]
    public async Task<IActionResult> AnalyzeReviewSentiments(int reviewId)
    {
        await _mediator.Send(new AnalyzeReviewSentimentsCommand(reviewId));
        return Ok(new { Message = "Sentiment analysis started successfully" });
    }

    [HttpPost("generate-rdf/{reviewId}")]
    public async Task<IActionResult> GenerateRdfTriples(int reviewId)
    {
        await _mediator.Send(new GenerateRdfTriplesCommand(reviewId));
        return Ok(new { Message = "RDF triples generation started successfully" });
    }
}