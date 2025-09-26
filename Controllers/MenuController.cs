using Microsoft.AspNetCore.Mvc;
using Victorina.Data;
using Victorina.Models;

namespace Victorina.Controllers
{
	public class MenuController : Controller
	{
		private VictorinaHolder _victorinaHolder;
		public MenuController(VictorinaHolder victorinaHolder) 
		{
            _victorinaHolder = victorinaHolder;
        }


		public IActionResult Index()
		{
			return View(_victorinaHolder.Models);
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
