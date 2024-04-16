using Microsoft.SemanticKernel;
namespace AIRecruitXcel.Core;

public interface ISemanticKernel
{
    Task<List<string>> GetSampleQuestions(string jd, string resume, string level);
}

