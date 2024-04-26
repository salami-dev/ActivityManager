using ActivityManager.Application.Common.Interfaces;
using ActivityManager.Domain.Entities;

namespace ActivityManager.Application.Tags.Commands.BulkCreateTags;

public record BulkCreateTagsCommand : IRequest<List<int>>
{
    public ICollection<CreateTagRequestDto> Tags { get; init; } = null!;
}

public class BulkCreateTagsCommandValidator : AbstractValidator<BulkCreateTagsCommand>
{
    public BulkCreateTagsCommandValidator()
    {
        RuleFor(x => x.Tags)
            .NotEmpty();
    }
}

public class BulkCreateTagsCommandHandler : IRequestHandler<BulkCreateTagsCommand, List<int>>
{
    private readonly IApplicationDbContext _context;

    public BulkCreateTagsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> Handle(BulkCreateTagsCommand request, CancellationToken cancellationToken)
    {
        var tagsToCreateRequest = request.Tags;
        // var requestTagNames = tagsToCreateRequest.Select(t => t.Name).ToList();
        var requestTagNames = tagsToCreateRequest.Select(t => $"%{t.Name}%").ToList();

        // var existingTags =
        //     await _context.Tags.AsNoTracking()
        //         .Where(t => requestTagNames.Any(name => EF.Functions.ILike(t.Name, $"%{name}%")))
        //         .ToListAsync(cancellationToken);

        var existingTags =
            await _context.Tags.AsNoTracking()
                .Where(t => requestTagNames.Any(rtn => EF.Functions.ILike(t.Name, rtn)))
                .ToListAsync(cancellationToken);

        var newTagsToCreate = tagsToCreateRequest
            .Where(nt => !existingTags.Any(et => et.Name.ToLower() == nt.Name.ToLower()))
            .Select(nt => new Tag()
            {
                Name = nt.Name, Theme = nt.Theme, Created = DateTimeOffset.Now, LastModified = DateTimeOffset.Now
            })
            .ToList();

        if (newTagsToCreate.Any())
        {
            await _context.BulkInsertAsync(newTagsToCreate, cancellationToken: cancellationToken);
        }

        return await _context.Tags.Where(t => requestTagNames.Any(rtn => EF.Functions.ILike(t.Name, rtn)))
            .Select(nt => nt.Id)
            .ToListAsync(cancellationToken);
    }
}
