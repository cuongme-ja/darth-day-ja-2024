using AIRecruitXcel.Core;

namespace AIRecruitXcel.App
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      var p = new ResumeParser();

      await p.PrintResumeAsync("Resumes/MahafujAnsari.pdf");
    }
  }
}
