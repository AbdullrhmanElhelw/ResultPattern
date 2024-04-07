using FluentValidation;

namespace ResultPattern.DTOs.Comments;

public sealed record CreateCommentDTO
    (Guid PostId, string Content);

public sealed class CreateCommentDTOValidator : AbstractValidator<CreateCommentDTO>
{
    public CreateCommentDTOValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Content)
          .NotEmpty()
          .MaximumLength(500);
    }
}