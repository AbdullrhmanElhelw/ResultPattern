using Microsoft.AspNetCore.Mvc;
using ResultPattern.DTOs.Comments;
using ResultPattern.Helper.Extensions;
using ResultPattern.Services.Abstracts;

namespace ResultPattern.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCommentDTO>> CreateComment([FromBody] CreateCommentDTO commentDTO, CancellationToken cancellationToken)
    {
        var result = await _commentService.CreateComment(commentDTO, cancellationToken);
        return result.Match<ActionResult<CreateCommentDTO>>(
            onSuccess: () => Ok(),
            onFailure: error => BadRequest(error)
            );
    }

    [HttpPut]
    public async Task<ActionResult<UpdateCommentDTO>> UpdateComment([FromBody] UpdateCommentDTO commentDTO, CancellationToken cancellationToken)
    {
        var result = await _commentService.UpdateComment(commentDTO, cancellationToken);
        return result.Match<ActionResult<UpdateCommentDTO>>(
            onSuccess: () => Ok(),
            onFailure: error => BadRequest(error)
            );
    }

    [HttpDelete("{postId}/{commentId}")]
    public async Task<ActionResult> DeleteComment(Guid postId, Guid commentId, CancellationToken cancellationToken)
    {
        var result = await _commentService.DeleteComment(postId, commentId, cancellationToken);
        return result.Match<ActionResult>(
           onSuccess: Ok,
           onFailure: BadRequest);
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<IReadOnlyCollection<ReadCommentDTO>>> GetCommentsAsync(Guid postId)
    {
        var result = await _commentService.GetCommentsAsync(postId);
        return result.Match<ActionResult<IReadOnlyCollection<ReadCommentDTO>>>(
            onSuccess: () => Ok(result),
            onFailure: error => BadRequest(error)
            );
    }
}