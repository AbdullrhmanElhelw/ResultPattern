using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ResultPattern.Data;
using ResultPattern.DTOs.Comments;
using ResultPattern.DTOs.Posts;
using ResultPattern.Services.Abstracts;
using ResultPattern.Services.Implementations;
using ResultPattern.Shared;
using ResultPattern.Shared.Repositories.Abstractions;
using ResultPattern.Shared.Repositories.Implementations;
using ResultPattern.Shared.UOW;

namespace ResultPattern.Dependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(op =>
        {
            op.UseSqlServer(connectionString);
        });

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentService, CommentService>();

        // add fluent validation services

        services.AddScoped<IValidator<CreatePostDTO>, CreatePostDTOValidator>();
        services.AddScoped<IValidator<UpdatePostDTO>, UpdatePostDTOValidator>();
        services.AddScoped<IValidator<CreateCommentDTO>, CreateCommentDTOValidator>();
        services.AddScoped<IValidator<UpdateCommentDTO>, UpdateCommentDTOValidator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}