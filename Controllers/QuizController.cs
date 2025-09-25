using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Victorina.Extensions;
using Victorina.Models;

namespace Victorina.Controllers
{
    public class QuizController : Controller
    {
        private QuizManager _quizManager;

        private const string QuizStateKey = "QuizState";

        public QuizController(QuizManager quizManager)
        {
            _quizManager = quizManager;
        }

        private void _initQuizState()
        {
            var state = HttpContext.Session.GetObject<QuizState>(QuizStateKey);
            _quizManager.initState(state);
        }

        private void _setQuizState()
        {
            HttpContext.Session.SetObject(QuizStateKey, _quizManager.QuizState);
        }

        [HttpGet]
        public IActionResult Index()
        {
            _initQuizState();
            return View(_quizManager.CurrentQuestion);
        }

        [HttpGet]
        public IActionResult SetQuestions(string id)
        {
            _quizManager.SetQuestions(id);
            Console.WriteLine(id);
            _setQuizState();
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
        public IActionResult Next(int userChoiceIndex)
        {
            _initQuizState();
            bool isStop = _quizManager.SubmitResult(userChoiceIndex);
            _setQuizState();
            if (isStop)
            {
                return RedirectToAction(nameof(Result), _quizManager.QuizResult);
            }
			return RedirectToAction(nameof(Index));
		}

        public IActionResult StartNewVictorina()
        {
            return RedirectToAction(nameof(Index));
		}

        public IActionResult Result(QuizResult result)
        {
        
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}