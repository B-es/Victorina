using Microsoft.AspNetCore.Mvc;
using Victorina.Models;

namespace Victorina.Controllers
{
	public class MenuController : Controller
	{
		private VictorinaManager _victorinaManager;
		public MenuController(VictorinaManager victorinaManager) 
		{ 
			_victorinaManager = victorinaManager;
        }


		public IActionResult Index()
		{
			return View(_victorinaManager.Models);
		}

		
		public IActionResult StartVictorina(string id)
		{
			if (id == null)
			{
				return View();
			}

			return RedirectToAction("SetQuestions", "Quiz", new { id = id});
		}
	}
}
