using Microsoft.AspNetCore.Mvc;
using Victorina.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Victorina.Controllers
{
	public class MenuController : Controller
	{

		public IActionResult Index()
		{
			return View(VictorinaManager.GetInstance().Victorins);
		}

		
		public IActionResult StartVictorina(string id)
		{
			if (id == null)
			{
				return View();
			}
			QuizManager.GetInstance().SetQuestions(id);

			return RedirectToAction("Index", "Home");
		}
	}
}
