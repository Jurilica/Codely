using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Gateways;
using Codely.Core.Gateways.Contracts;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Submission;

public sealed class ExecuteTestCasesCommand : IRequestHandler<ExecuteTestCasesRequest>
{
    private readonly CodelyContext _context;
    private readonly ICodeTranslationClient _codeTranslationClient;

    public ExecuteTestCasesCommand(CodelyContext context, ICodeTranslationClient codeTranslationClient)
    {
        _context = context;
        _codeTranslationClient = codeTranslationClient;
    }
    
    public async Task Handle(ExecuteTestCasesRequest request, CancellationToken cancellationToken)
    {
        var submissionData = await _context.Submissions
            .Where(x => x.Id == request.SubmissionId)
            .Select(x =>
                new
                {
                    Submission = x,
                    x.Problem.TestCases,
                    x.ProgrammingLanguage
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (submissionData is null)
        {
            throw new CodelyException("Submission not found");
        }

        var allTestCasesPassed = true;

        foreach (var testCase in submissionData.TestCases)
        {
            var file = new TranslateCodeFile
            {
                Content = submissionData.Submission.Answer 
            };
            
            var codeTranslationRequest = new TranslateCodeRequest
            {
                Language = submissionData.ProgrammingLanguage.ToString(),
                Version = submissionData.ProgrammingLanguage.ToString(),
                StandardInput = testCase.Input,
                Files = new List<TranslateCodeFile>
                {
                    file
                }
            };

            try
            {
                var codeTranslationResponse = await _codeTranslationClient.TranslateCode(codeTranslationRequest);

                var isSuccess = string.IsNullOrEmpty(codeTranslationResponse.Result.Error);
                var isCorrect = codeTranslationResponse.Result.Output == testCase.Output;

                var submissionTestCaseStatus = isCorrect
                    ? SubmissionTestCaseStatus.CorrectAnswer
                    : isSuccess
                        ? SubmissionTestCaseStatus.WrongAnswer
                        : SubmissionTestCaseStatus.Error;

                var submissionTestCase = new SubmissionTestCase
                {
                    Output = isSuccess ? codeTranslationResponse.Result.Output! : codeTranslationResponse.Result.Error!,
                    SubmissionTestCaseStatus = submissionTestCaseStatus,
                    TestCaseId = testCase.Id,
                    SubmissionId = submissionData.Submission.Id
                };
                
                if (!isSuccess)
                {
                    allTestCasesPassed = false;
                }
                
                _context.SubmissionTestCases.Add(submissionTestCase);
            }
            catch
            {
                submissionData.Submission.SubmissionStatus = SubmissionStatus.InternalError;
                await _context.SaveChangesAsync(cancellationToken);
                
                return;
            }
        }

        submissionData.Submission.SubmissionStatus = allTestCasesPassed 
            ? SubmissionStatus.Succeeded 
            : SubmissionStatus.Failed;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ExecuteTestCasesRequest : IRequest
{
    public required int SubmissionId { get; init; }
}