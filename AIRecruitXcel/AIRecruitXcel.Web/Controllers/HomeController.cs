using AIRecruitXcel.Core;
using AIRecruitXcel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace AIRecruitXcel.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly IInterviewSupporter _sKernel;

    public HomeController(ILogger<HomeController> logger, IInterviewSupporter sKernel)
    {
      _sKernel = sKernel;
      _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
      var model = new InterviewViewModel();
      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(InterviewViewModel model)
    {
      if (model.Action == "Upload" && model.ResumeFile != null && model.ResumeFile.Length > 0)
      {
        model.Resume = await ParseResumeAsync(model);
        model.Questions = new List<QuestionViewModel>();
      }

      if (model.Action == "Start")
      {
        model = await GenerateQuestions(model);
      }

      if (model.Action == "Finish")
      {
        model.IsFinishedReview = true;
        model = EvaluateAnswers(model);
      }

      return View(model);
    }

    private async Task<InterviewViewModel> GenerateQuestions(InterviewViewModel model)
    {
      // TODO: Cuong & Hung
      var questions = await _sKernel.GetSampleQuestions(model.JobDescription, model.Resume, "Senior");
      model.Questions = new List<QuestionViewModel>();
      foreach (var question in questions)
      {
        if (!string.IsNullOrEmpty(question))
        {
          var questionAndHint = question.Split("||", StringSplitOptions.RemoveEmptyEntries);
          var q = new QuestionViewModel
          {
            Question = questionAndHint[0]
          };
          if (questionAndHint.Length > 1)
          {
            q.Hint = questionAndHint[1];
          }

          model.Questions.Add(q);
        }

      }

      return model;
    }

    private InterviewViewModel EvaluateAnswers(InterviewViewModel model)
    {
      foreach (var question in model.Questions.Where(x => !string.IsNullOrEmpty(x.Answer)))
      {
        question.AIFeedback = _sKernel.EvaluateAnswers(model.JobDescription, model.Resume, question.Question, question.Answer).Result;
      }
      return model;
    }

    private async Task<string> ParseResumeAsync(InterviewViewModel model)
    {
      string tempFilePath = Path.GetTempFileName();
      using (var fileStream = System.IO.File.Create(tempFilePath))
      {
        model.ResumeFile.CopyTo(fileStream);
      }
      var parser = new ResumeParser();
      var data = await parser.ParseResumeAsync(tempFilePath);
      System.IO.File.Delete(tempFilePath);
      return string.Join(Environment.NewLine, data);
    }

  }
}
