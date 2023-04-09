﻿using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Gateways;
using Codely.Core.Gateways.Contracts;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Refit;

namespace Codely.Core.Handlers.User.Submission;

public sealed class SubmitAnswerCommand : IRequestHandler<SubmitAnswerRequest, SubmitAnswerResponse>
{
    private readonly CodelyContext _context;
    private readonly ICodeTranslationClient _codeTranslationClient;
    private readonly ICurrentUserService _currentUserService;

    public SubmitAnswerCommand(CodelyContext context, ICodeTranslationClient codeTranslationClient, ICurrentUserService currentUserService)
    {
        _context = context;
        _codeTranslationClient = codeTranslationClient;
        _currentUserService = currentUserService;
    }
    
    public async Task<SubmitAnswerResponse> Handle(SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Answer, "Answer is empty");

        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .Select(x =>
                new
                {
                    TestCase = x.TestCases
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (problem is null)
        {
            throw new CodelyException("Coding problem not found");
        }

        var programmingLanguageVersion = await _context.ProgrammingLanguageVersions
            .Where(x => x.ProgrammingLanguage == request.ProgrammingLanguage)
            .Select(x => 
                new
                {
                    x.Id,
                    x.Version,
                    x.Name
                })
            .SingleOrDefaultAsync(cancellationToken);

        if (programmingLanguageVersion is null)
        {
            throw new CodelyException("Invalid programming language");
        }

        var submission = new Data.Entities.Submission
        {
            UserId = _currentUserService.Id,
            Answer = request.Answer,
            SubmissionStatus = SubmissionStatus.Created,
            ProgrammingLanguageVersionId = programmingLanguageVersion.Id,
            ProblemId = request.ProblemId
        };

        await _context.Submissions.AddAsync(submission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        submission.SubmissionStatus = SubmissionStatus.CorrectAnswer;
        foreach (var testCase in problem.TestCase)
        {
            var file = new TranslateCodeFile
            {
                Content = request.Answer
            };
            
            var translateCodeRequest = new TranslateCodeRequest
            {
                Language = programmingLanguageVersion.Name,
                Version = programmingLanguageVersion.Version,
                Files = new List<TranslateCodeFile>
                {
                    file
                },
                StandardInput = testCase.Input
            };

            try
            {
                var response = await _codeTranslationClient.TranslateCode(translateCodeRequest);

                var testCaseStatus = response.Result.Output == testCase.Output
                    ? SubmissionTestCaseStatus.Passed
                    : SubmissionTestCaseStatus.Failed;
                
                var submissionTestCase = new SubmissionTestCase
                {
                    TestCaseId = testCase.Id,
                    SubmissionId = submission.Id,
                    SubmissionTestCaseStatus = testCaseStatus,
                    Output = testCaseStatus == SubmissionTestCaseStatus.Passed 
                        ? response.Result.Output!
                        : response.Result.Error!
                };
                
                await _context.SubmissionTestCases.AddAsync(submissionTestCase, cancellationToken);

                if (!response.Result.Error.IsNullOrEmpty() && testCase.Id == problem.TestCase.First().Id)
                {
                    submission.SubmissionStatus = SubmissionStatus.CompilationError;
                    break;
                }
                
                if (!response.Result.Error.IsNullOrEmpty())
                {
                    submission.SubmissionStatus = SubmissionStatus.ExecutionError;
                    break;
                }

                if (testCaseStatus == SubmissionTestCaseStatus.Failed)
                {
                    submission.SubmissionStatus = SubmissionStatus.WrongAnswer;
                    break;
                }
            }
            catch (ApiException)
            {
                submission.SubmissionStatus = SubmissionStatus.InternalError;
            }
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitAnswerResponse();
    }
}

public sealed class SubmitAnswerRequest : IRequest<SubmitAnswerResponse>
{
    public int ProblemId { get; init; }

    public string Answer { get; init; } = string.Empty;
    
    public ProgrammingLanguage ProgrammingLanguage { get; init; }
}

public sealed class SubmitAnswerResponse
{
}