﻿using Codely.Core.Data;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Users;

public sealed class BanUserCommand : IRequestHandler<BanUserRequest, BanUserResponse>
{
    private readonly CodelyContext _context;

    public BanUserCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<BanUserResponse> Handle(BanUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Username == request.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new CodelyException("User not found");
        }

        user.UserStatus = UserStatus.Banned;
        await _context.SaveChangesAsync(cancellationToken);
        
        return new BanUserResponse();
    }
}

public sealed class BanUserRequest : IRequest<BanUserResponse>
{
    public required string Username { get; init;}
}

public sealed class BanUserResponse
{
}