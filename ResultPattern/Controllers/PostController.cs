using Microsoft.AspNetCore.Mvc;
using ResultPattern.DTOs.Posts;
using ResultPattern.Helper.Extensions;
using ResultPattern.Services.Abstracts;

namespace ResultPattern.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatePostDTO>> CreatePost([FromBody] CreatePostDTO postDTO)
    {
        var result = await _postService.CreateAsync(postDTO);
        return result.Match<ActionResult<CreatePostDTO>>
            (
            onSuccess: () => Ok(postDTO),
            onFailure: error => BadRequest(error.Message)
            );
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPostsDTO>>> GetPosts()
    {
        var result = await _postService.GetPostsAsync();
        return result.Match<ActionResult<IEnumerable<GetPostsDTO>>>(
            onSuccess: () => Ok(result),
            onFailure: error => BadRequest(error.Message)
            );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetPostsDTO>> GetPost(Guid id)
    {
        var result = await _postService.GetPostAsync(id);
        return result.Match<ActionResult<GetPostsDTO>>(
            onSuccess: () => Ok(result),
            onFailure: error => BadRequest(error.Message)
            );
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdatePostDTO>> UpdatePost(Guid id, [FromBody] UpdatePostDTO postDTO)
    {
        var result = await _postService.UpdatePostAsync(id, postDTO);
        return result.Match<ActionResult<UpdatePostDTO>>(
            onSuccess: () => Ok(postDTO),
            onFailure: error => BadRequest(error.Message)
             );
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePost(Guid id)
    {
        var result = await _postService.DeletePostAsync(id);
        return result.Match<ActionResult>
            (
            onSuccess: NoContent,
            onFailure: BadRequest
            );
    }
}