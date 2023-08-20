using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Leaderboard;

public sealed class GetLeaderboardQuery : IRequestHandler<GetLeaderboardRequest, GetLeaderboardResponse>
{
    private readonly CodelyContext _context;

    public GetLeaderboardQuery(CodelyContext context)
    {
        _context = context;
    }

    public async Task<GetLeaderboardResponse> Handle(GetLeaderboardRequest request, CancellationToken cancellationToken)
    {
        var lastSubmissionsPerUser = await _context.Submissions
            .AsNoTracking()
            .Where(x => x.Problem.Status == ProblemStatus.Published)
            .Select(x =>
                new
                {
                    x.User.Username,
                    x.Problem.Difficulty,
                    x.SubmissionStatus,
                    x.ProblemId,
                    x.Created
                })
            .GroupBy(x => new {x.Username, x.ProblemId})
            .Select(x =>
                new
                {
                    SubmissionData = x
                        .OrderByDescending(y => y.Created)
                        .First()
                })
            .ToListAsync(cancellationToken);

        var leaderboardData = lastSubmissionsPerUser
            .Where(x => x.SubmissionData.SubmissionStatus == SubmissionStatus.Succeeded)
            .GroupBy(x => x.SubmissionData.Username)
            .Select(x =>
                new
                {
                    Username = x.Key,
                    EasyProblemsSolved = x
                        .Where(y => y.SubmissionData.Difficulty == ProblemDifficulty.Easy)
                        .Count(),
                    MediumProblemsSolved = x
                        .Where(y => y.SubmissionData.Difficulty == ProblemDifficulty.Medium)
                        .Count(),
                    HardProblemsSolved = x
                        .Where(y => y.SubmissionData.Difficulty == ProblemDifficulty.Hard)
                        .Count()
                })
            .Select(x => 
                new LeaderboardData
                {
                    Username = x.Username,
                    EasyProblemsSolved = x.EasyProblemsSolved,
                    MediumProblemsSolved = x.MediumProblemsSolved,
                    HardProblemsSolved = x.HardProblemsSolved,
                    Points = x.EasyProblemsSolved + x.MediumProblemsSolved * 2 + x.HardProblemsSolved * 3
                })
            .ToList();

        var fetchedUsernames = leaderboardData
            .Select(x => x.Username)
            .ToList();

        var missingUsersLeaderboardData = await _context.Users
            .Where(x => x.Role == Role.User) 
            .Where(x => !fetchedUsernames.Contains(x.Username))
            .Select(x =>
                new LeaderboardData
                {
                    Username = x.Username,
                    EasyProblemsSolved = 0,
                    HardProblemsSolved = 0,
                    MediumProblemsSolved = 0,
                    Points = 0
                })
            .ToListAsync(cancellationToken);
        
        leaderboardData.AddRange(missingUsersLeaderboardData);

        var position = 1;
        var getLeaderboardData = leaderboardData
            .OrderByDescending(x => x.Points)
            .Select(x =>
                new GetLeaderboardData
                {
                    Position = position++,
                    Username = x.Username,
                    EasyProblemsSolved = x.EasyProblemsSolved,
                    MediumProblemsSolved = x.MediumProblemsSolved,
                    HardProblemsSolved = x.HardProblemsSolved,
                    Points = x.Points
                })
            .ToList();

        return new GetLeaderboardResponse
        {
            Leaderboard = getLeaderboardData
        };
    }
}

public sealed class GetLeaderboardRequest : IRequest<GetLeaderboardResponse>
{

}

public sealed class GetLeaderboardResponse
{
    public required List<GetLeaderboardData> Leaderboard { get; init; }
}

public sealed class GetLeaderboardData : LeaderboardData
{
    public required int Position { get; init; }
}

public class LeaderboardData
{
    public required string Username { get; init; }
    
    public required int Points { get; init; }
    
    public required int EasyProblemsSolved { get; init; }
    
    public required int MediumProblemsSolved { get; init; }
    
    public required int HardProblemsSolved { get; init; }
}