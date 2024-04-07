using ResultPattern.DTOs.Posts;
using ResultPattern.Shared;

namespace ResultPattern.Services.Abstracts;

public interface IPostService
{
    Task<Result> CreateAsync(CreatePostDTO postDto);

    Task<Result<IEnumerable<GetPostsDTO>>> GetPostsAsync();

    Task<Result<GetPostsDTO>> GetPostAsync(Guid id);

    Task<Result> UpdatePostAsync(Guid id, UpdatePostDTO postDto);

    Task<Result> DeletePostAsync(Guid id);
}