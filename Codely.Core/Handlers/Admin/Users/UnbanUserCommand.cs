using Codely.Core.Data;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Users;

public sealed class UnbanUserCommand : IRequestHandler<UnbanUserRequest, UnbanUserResponse>
{
    private readonly CodelyContext _context;

    public UnbanUserCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<UnbanUserResponse> Handle(UnbanUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new CodelyException("User not found");
        }

        user.UserStatus = UserStatus.Active;
        await _context.SaveChangesAsync(cancellationToken);

        return new UnbanUserResponse();
    }
}

public sealed class UnbanUserRequest : IRequest<UnbanUserResponse>
{
    public required string Username { get; init;}
}

public sealed class UnbanUserResponse
{
}