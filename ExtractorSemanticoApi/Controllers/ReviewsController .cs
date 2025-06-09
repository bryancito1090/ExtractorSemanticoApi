using ExtractorSemanticoApi.Application.Features.Reviews.Command;
using ExtractorSemanticoApi.Application.Features.Reviews.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExtractorSemanticoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateReview([FromBody] CreateUpdateReviewCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetReviews()
    {
        var result = await _mediator.Send(new GetReviewsQuery());
        return Ok(result);
    }

    [HttpGet("by-product/{productId}")]
    public async Task<IActionResult> GetReviewsByProductId(int productId)
    {
        var result = await _mediator.Send(new GetReviewsByProductIdQuery(productId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var result = await _mediator.Send(new GetReviewByIdQuery(id));
        return Ok(result);
    }

    [HttpPost("process/{reviewId}")]
    public async Task<IActionResult> ProcessReviewText(int reviewId)
    {
        await _mediator.Send(new ProcessReviewTextCommand(reviewId));
        return Ok(new { Message = "Review text processing started" });
    }
}