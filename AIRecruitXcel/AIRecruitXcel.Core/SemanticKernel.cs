using Microsoft.SemanticKernel;
using System.Reflection;

namespace AIRecruitXcel.Core;

public class SemanticKernel: ISemanticKernel
{

    private readonly Kernel _kernel;
    public SemanticKernel(string model, string aIKey)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(model, aIKey);

        builder.Plugins.AddFromPromptDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins", "InterviewPlugin"));

        _kernel = builder.Build();
    }

    public async Task<List<string>> GetSampleQuestions(string jd, string resume, string level)
    {
        try
        {
            const string questionsSeparater = "*question_end*";
            var arguments = new KernelArguments() { 
                ["jd"] = Mock.JD, ["number_of_questions"] = 8, 
                ["level"] = level,
                ["resume"] = resume,
                ["question_separator"] = questionsSeparater };

            var plugin = _kernel.Plugins["InterviewPlugin"];
            var genQuestionsfunction = plugin["GenQuestions"];

            var result = await _kernel.InvokeAsync(genQuestionsfunction, arguments);

            var questions = result.GetValue<string>().Split(questionsSeparater);

            return questions.ToList();
            
        }catch (Exception ex)
        {
            throw ex;
        }
    }
}

