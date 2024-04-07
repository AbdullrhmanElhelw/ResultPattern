using FluentValidation;
using FluentValidation.Results;
using ResultPattern.DTOs.Comments;
using ResultPattern.Entites;
using ResultPattern.Services.Abstracts;
using ResultPattern.Shared;
using ResultPattern.Shared.Repositories.Abstractions;
using ResultPattern.Shared.UOW;
using System.Collections.Immutable;

namespace ResultPattern.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCommentDTO> _validator;
    private readonly IValidator<UpdateCommentDTO> _updateValidator;

    public CommentService(ICommentRepository commentRepository,
                          IUnitOfWork unitOfWork,
                          IValidator<CreateCommentDTO> validator,
                          IPostRepository postRepository,
                          IValidator<UpdateCommentDTO> updateValidator)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _postRepository = postRepository;
        _updateValidator = updateValidator;
    }

    public async Task<Result> CreateComment(CreateCommentDTO commentDto, CancellationToken cancellationToken)
    {
        var findPost = _postRepository.GetById(commentDto.PostId);
        if (findPost is null)
            return Result.Failure("Post not found");
        var validationResult = _validator.Validate(commentDto);
        if (!validationResult.IsValid)
            return Result.Failure(GetErrorMessage(validationResult.Errors));

        var comment = Comment.CreateComment(commentDto.Content, findPost);

        _commentRepository.Add(comment);
        if (await _unitOfWork.CommitAsync() <= 0)
            return Result.Failure("Failed to create comment");

        return Result.Success();
    }

    private static string GetErrorMessage(IEnumerable<ValidationFailure> errors)
    {
        var errorMessages = errors.Select(e => e.ErrorMessage);
        return string.Join(Environment.NewLine, errorMessages);
    }

    public async Task<Result> UpdateComment(UpdateCommentDTO commentDto, CancellationToken cancellationToken)
    {
        var findPost = _postRepository.GetById(commentDto.PostId);
        if (findPost is null)
            return Result.Failure("Post not found");

        var findComment = _commentRepository.GetById(commentDto.CommentId);
        if (findComment is null)
            return Result.Failure("Comment not found");

        var validationResult = _updateValidator.Validate(commentDto);
        if (!validationResult.IsValid)
            return Result.Failure(GetErrorMessage(validationResult.Errors));

        Comment.UpdateComment(findPost, findComment, commentDto.Content);

        _commentRepository.Update(findComment);

        if (await _unitOfWork.CommitAsync() <= 0)
            return Result.Failure("Failed to update comment");

        return Result.Success();
    }

    public async Task<Result> DeleteComment(Guid postId, Guid commentId, CancellationToken cancellationToken)
    {
        var findPost = _postRepository.GetById(postId);
        if (findPost is null)
            return Result.Failure("Post not found");

        var findComment = _commentRepository.GetById(commentId);
        if (findComment is null)
            return Result.Failure("Comment not found");

        Comment.DeleteComment(findPost, findComment);
        _commentRepository.Update(findComment);
        if (await _unitOfWork.CommitAsync() <= 0)
            return Result.Failure("Failed to delete comment");
        return Result.Success();
    }

    public Task<Result<IReadOnlyCollection<ReadCommentDTO>>> GetCommentsAsync(Guid PostId)
    {
        var findPost = _postRepository.GetById(PostId);
        if (findPost is null)
            return Task.FromResult(Result.Failure<IReadOnlyCollection<ReadCommentDTO>>("Post not found"));

        var comments = _commentRepository.GetAll()
            .Where(c => c.PostId == PostId)
            .Where(c => c.IsDeleted == false)
            .Select(c => new ReadCommentDTO(c.PostId, c.Id, c.Content))
            .ToImmutableList();

        return Task.FromResult(Result.Success<IReadOnlyCollection<ReadCommentDTO>>(comments));
    }
}