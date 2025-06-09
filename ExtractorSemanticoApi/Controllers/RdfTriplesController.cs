using ExtractorSemanticoApi.Application.Features.RdfTriples.Command;
using ExtractorSemanticoApi.Application.Features.RdfTriples.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExtractorSemanticoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RdfTriplesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RdfTriplesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateRdfTriple([FromBody] CreateUpdateRdfTripleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetRdfTriples()
    {
        var result = await _mediator.Send(new GetRdfTriplesQuery());
        return Ok(result);
    }

    [HttpGet("by-review/{reviewId}")]
    public async Task<IActionResult> GetRdfTriplesByReviewId(int reviewId)
    {
        var result = await _mediator.Send(new GetRdfTriplesByReviewIdQuery(reviewId));
        return Ok(result);
    }

    [HttpPost("generate/{reviewId}")]
    public async Task<IActionResult> GenerateRdfTriples(int reviewId)
    {
        await _mediator.Send(new GenerateRdfTriplesCommand(reviewId));
        return Ok(new { Message = "RDF triples generated successfully" });
    }

    [HttpDelete("by-review/{reviewId}")]
    public async Task<IActionResult> DeleteRdfTriples(int reviewId)
    {
        await _mediator.Send(new DeleteRdfTriplesCommand(reviewId));
        return Ok(new { Message = "RDF triples deleted successfully" });
    }
}