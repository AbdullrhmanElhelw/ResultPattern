using FluentValidation;

namespace ResultPattern.DTOs.Posts;

public sealed record CreatePostDTO
    (string Title, string Content);

public sealed class CreatePostDTOValidator : AbstractValidator<CreatePostDTO>
{
    public CreatePostDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}