using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Victorina.Models;

namespace Victorina.Controllers
{
    public class HomeController : Controller
    {
        private QuizManager quizManager = QuizManager.GetInstance();

        [HttpGet]
        public IActionResult Index()
        {
			return View(quizManager.CurrentQuestion);
        }

		[HttpPost]
        public IActionResult Next(int UserChoiceIndex)
        {
            var current = quizManager.CurrentQuestion;

			if (current.RightAnswer == UserChoiceIndex)
            {
				quizManager.RightCount++;
				ViewBag.IsWrong = false;
			}
            else
            {
                ViewBag.IsWrong = true;
				ViewBag.RightAnswerText = current.Answers[current.RightAnswer-1];
			}

            quizManager.currentIndex++;

            if (quizManager.isStop)
            {
                return RedirectToAction(nameof(Result), quizManager.VictorinaResult);
            }
			return RedirectToAction(nameof(Index));
			//return View(nameof(Index), quizManager.CurrentQuestion);
		}

        public IActionResult StartNewVictorina()
        {
            return RedirectToAction(nameof(Index));
		}

        public IActionResult Result(VictorinaResult result)
        {
            quizManager.Clear();
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}