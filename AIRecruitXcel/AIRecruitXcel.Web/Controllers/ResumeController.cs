using Microsoft.AspNetCore.Mvc;

namespace AIRecruitXcel.Web.Controllers
{
  public class ResumeController : Controller
  {
    private readonly ILogger<ResumeController> _logger;

    public ResumeController(ILogger<ResumeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Upload()
    {
      return View();
    }

    public IActionResult Generate()
    {
      return View();
    }
  }
}
