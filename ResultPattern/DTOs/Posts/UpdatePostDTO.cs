using FluentValidation;

namespace ResultPattern.DTOs.Posts;

public sealed record UpdatePostDTO
    (
    string Title,
    string Content);

public class UpdatePostDTOValidator : AbstractValidator<UpdatePostDTO>
{
    public UpdatePostDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is Required");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is Required");
    }
}