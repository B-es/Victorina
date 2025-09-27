using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Victorina.Data;
using Victorina.Extensions;
using Victorina.Models;
using Victorina.Services;

namespace Victorina.Controllers
{
    public class QuizController : Controller
    {
        private QuizManager _quizManager;
        private QuizHolder _quizHolder;

        private const string QuizStateKey = "QuizState";

        public QuizController(QuizManager quizManager, QuizHolder quizHolder)
        {
            _quizManager = quizManager;
            _quizHolder = quizHolder;
        }

        private QuizStateModel _getQuizState()
        {
            var state = HttpContext.Session.GetObject<QuizStateModel>(QuizStateKey);
            return state;
        }

        private void _setQuizState(QuizStateModel quizState)
        {
            HttpContext.Session.SetObject(QuizStateKey, quizState);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var state = _getQuizState();
            TempData.Keep("VictorinaTitle");
            return View(_quizManager.GetCurrentQuestion(state, _quizHolder));
        }

        [HttpGet]
        public IActionResult SetQuestions(string id, string title)
        {
            TempData["VictorinaTitle"] = title;
            var state = new QuizStateModel() { Id = id, QuestionsCount = _quizHolder.GetQuestionCount(id) };
            _setQuizState(state);
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
        public IActionResult Next(int userChoiceIndex)
        {
            var state = _getQuizState();
            bool isStop = _quizManager.SubmitResult(state, _quizHolder, userChoiceIndex);
            _setQuizState(state);

            if (isStop)
            {
                return RedirectToAction(nameof(Result), _quizManager.GetQuizResult(state));
            }
			return RedirectToAction(nameof(Index));
		}

        public IActionResult StartNewVictorina()
        {
            var prevState = _getQuizState();
            prevState.RightCount = 0;
            prevState.CurrentIndex = 0;
            _setQuizState(prevState);
            return RedirectToAction(nameof(Index));
		}

        public IActionResult Result(QuizResultModel result)
        {
            TempData.Keep("VictorinaTitle");
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}