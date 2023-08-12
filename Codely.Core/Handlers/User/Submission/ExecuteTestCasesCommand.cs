using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Services;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Submission;

public sealed class ExecuteTestCasesCommand : IRequestHandler<ExecuteTestCasesRequest>
{
    private const string Delimiter = "TestCase:";
    
    private readonly CodelyContext _context;
    private readonly ICodeExecutionService _codeExecutionService;

    public ExecuteTestCasesCommand(CodelyContext context, ICodeExecutionService codeExecutionService)
    {
        _context = context;
        _codeExecutionService = codeExecutionService;
    }
    
    public async Task Handle(ExecuteTestCasesRequest request, CancellationToken cancellationToken)
    {
        var submissionData = await _context.Submissions
            .Where(x => x.Id == request.SubmissionId)
            .Select(x =>
                new
                {
                    Submission = x,
                    x.Problem.TestCases
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (submissionData is null)
        {
            throw new CodelyException("Submission not found");
        }
        
        var testCasesInput = submissionData.TestCases
            .Select(x => x.Input)
            .ToList();

        var testCaseInputsWithDelimiter = string.Join(Delimiter, testCasesInput);
        var resultWithDelimiter = await _codeExecutionService.ExecuteCode(submissionData.Submission.Answer,
            submissionData.Submission.ProgrammingLanguage, testCaseInputsWithDelimiter, cancellationToken);

        //remove first delimiter at the beginning of the string
        resultWithDelimiter = resultWithDelimiter.Remove(0, Delimiter.Length);

        var testCaseOutputs = resultWithDelimiter.Split(Delimiter);

        var submissionTestCases = new List<SubmissionTestCase>();
        for (var i = 0; i < submissionData.TestCases.Count; i++)
        {
            var testCaseOutput = testCaseOutputs[i];
            var testCase = submissionData.TestCases[i];
            
            var isCorrect = testCaseOutput.Trim() == testCase.Output;

            var submissionTestCaseStatus = isCorrect
                ? SubmissionTestCaseStatus.CorrectAnswer
                : SubmissionTestCaseStatus.WrongAnswer;
            
            var submissionTestCase = new SubmissionTestCase
            {
                Output = testCaseOutput,
                SubmissionTestCaseStatus = submissionTestCaseStatus,
                TestCaseId = testCase.Id,
                SubmissionId = submissionData.Submission.Id
            };
            
            submissionTestCases.Add(submissionTestCase);
        }

        var allTestCasesPassed = submissionTestCases
            .All(x => x.SubmissionTestCaseStatus == SubmissionTestCaseStatus.CorrectAnswer);
        
        submissionData.Submission.SubmissionStatus = allTestCasesPassed 
            ? SubmissionStatus.Succeeded 
            : SubmissionStatus.Failed;

        _context.SubmissionTestCases.AddRange(submissionTestCases);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ExecuteTestCasesRequest : IRequest
{
    public required int SubmissionId { get; init; }
}