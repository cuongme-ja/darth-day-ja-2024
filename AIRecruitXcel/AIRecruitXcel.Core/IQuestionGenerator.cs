using Microsoft.SemanticKernel;
namespace AIRecruitXcel.Core;

public interface IQuestionGenerator
{
    Task<List<string>> GetSampleQuestions(string jd, string resume, string level);
}

