using FluentValidation;
using FluentValidation.Results;
using ResultPattern.DTOs.Posts;
using ResultPattern.Entites;
using ResultPattern.Services.Abstracts;
using ResultPattern.Shared;
using ResultPattern.Shared.Repositories.Abstractions;
using ResultPattern.Shared.UOW;

namespace ResultPattern.Services.Implementations;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePostDTO> _createPostValidator;
    private readonly IValidator<UpdatePostDTO> _updatePostValidator;

    public PostService(
        IPostRepository postRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreatePostDTO> createPostValidator,
        IValidator<UpdatePostDTO> updatePostValidator)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _createPostValidator = createPostValidator;
        _updatePostValidator = updatePostValidator;
    }

    public async Task<Result> CreateAsync(CreatePostDTO postDto)
    {
        var validationResult = await _createPostValidator.ValidateAsync(postDto);

        if (!validationResult.IsValid)
        {
            return Result.Failure(GetErrorMessage(validationResult.Errors));
        }

        var post = Post.Create(postDto.Title, postDto.Content);
        _postRepository.Add(post);
        if (await _unitOfWork.CommitAsync() > 0)
        {
            return Result.Success();
        }
        return Result.Failure("Failed to create post");
    }

    public Task<Result<GetPostsDTO>> GetPostAsync(Guid id)
    {
        var post = _postRepository.GetById(id);
        if (post is null)
            return Task.FromResult(Result.Failure<GetPostsDTO>("Post not found"));

        var postDto = new GetPostsDTO(post.Id, post.Title, post.Content, post.CreatedOnUtc);
        return Task.FromResult(Result.Success(postDto));
    }

    public Task<Result<IEnumerable<GetPostsDTO>>> GetPostsAsync()
    {
        var posts = _postRepository.GetAll().Where(p => p.IsDeleted == false);
        if (posts is null)
            return Task.FromResult(Result.Failure<IEnumerable<GetPostsDTO>>("No posts found"));

        var postDtos = posts.Select(p => new GetPostsDTO(p.Id, p.Title, p.Content, p.CreatedOnUtc));
        return Task.FromResult(Result.Success(postDtos));
    }

    public async Task<Result> UpdatePostAsync(Guid id, UpdatePostDTO postDto)
    {
        try
        {
            var post = _postRepository.GetById(id);
            if (post == null)
                return Result.Failure("Post not found");

            var validationResult = await _updatePostValidator.ValidateAsync(postDto);
            if (!validationResult.IsValid)
                return Result.Failure(GetErrorMessage(validationResult.Errors));

            Post.Update(post, postDto.Title, postDto.Content);
            _postRepository.Update(post);

            if (await _unitOfWork.CommitAsync() > 0)
                return Result.Success();

            return Result.Failure("No changes were made to the post.");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result.Failure("An error occurred while updating the post. Please try again later.");
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    public async Task<Result> DeletePostAsync(Guid id)
    {
        var post = _postRepository.GetById(id);
        if (post is null)
            return Result.Failure("Post not found");

        // use soft Delete method
        Post.Delete(post);
        _postRepository.Update(post);
        if (await _unitOfWork.CommitAsync() <= 0)
            return Result.Failure("Failed to delete post");

        return Result.Success();
    }

    private static string GetErrorMessage(IEnumerable<ValidationFailure> errors)
    {
        var errorMessages = errors.Select(e => e.ErrorMessage);
        return string.Join(Environment.NewLine, errorMessages);
    }
}