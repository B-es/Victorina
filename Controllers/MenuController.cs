using Microsoft.AspNetCore.Mvc;
using Victorina.Models;
using Victorina.Services.Interfaces;

namespace Victorina.Controllers
{
	public class MenuController : Controller
	{
		private IVictorinaHolder _victorinaHolder;
		public MenuController(IVictorinaHolder victorinaHolder) 
		{
            _victorinaHolder = victorinaHolder;
        }


		public IActionResult Index()
		{
            TempData["End"] = null;
            return View(_victorinaHolder.Models);
		}

		
		public IActionResult StartVictorina(string id)
		{
			if (id == null)
			{
				return View();
			}

			return RedirectToAction("SetQuestions", "Quiz", new { id = id, title = _victorinaHolder.Models.First(m => m.Id == id).Title});
		}
	}
}
