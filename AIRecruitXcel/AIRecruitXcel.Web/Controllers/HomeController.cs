using AIRecruitXcel.Core;
using AIRecruitXcel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AIRecruitXcel.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
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
        model = GenerateQuestions(model);
      }

      if (model.Action == "Finish")
      {
        model.IsFinishedReview = true;
        model = EvaluateAnswers(model);
      }

      return View(model);
    }

    private InterviewViewModel GenerateQuestions(InterviewViewModel model)
    {
      // TODO: Cuong & Hung
      model.Questions = new List<QuestionViewModel>
      {
        new QuestionViewModel
        {
          Question = "What is your name?"
        },
        new QuestionViewModel
        {
          Question = "Tell me about yourself."
        }
      };
      return model;
    }

    private InterviewViewModel EvaluateAnswers(InterviewViewModel model)
    {
      // TODO: Khang & Leo
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
