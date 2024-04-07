using FluentValidation;

namespace ResultPattern.DTOs.Comments;

public sealed record UpdateCommentDTO
    (Guid PostId,
     Guid CommentId,
     string Content);

public sealed class UpdateCommentDTOValidator : AbstractValidator<UpdateCommentDTO>
{
    public UpdateCommentDTOValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.CommentId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(500);
    }
}