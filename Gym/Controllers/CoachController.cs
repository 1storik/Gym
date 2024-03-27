using Gym.Models;
using Gym.Repository.IRepository;
using Gym.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class CoachController : Controller
    {
        private readonly ICoachRepository _coachRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CoachController(ICoachRepository coachRepository, IWebHostEnvironment webHostEnvironment)
        {
            _coachRepository = coachRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var coach= await _coachRepository.GetAllAsync();

            return View(coach);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
                return NotFound();

            var coach = await _coachRepository.GetByIdAsync(id);

            if (coach == null)
                return NotFound();

            return View(coach);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Coach coach)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePathCoach;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            coach.Image = fileName + extension;

            await _coachRepository.CreateAsync(coach);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
                return NotFound();

            var coach = await _coachRepository.GetByIdAsync(id);

            if (coach == null)
                return NotFound();

            return View(coach);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Coach сoach)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            var objFromDb = await _coachRepository.GetAsync(c => c.Id == сoach.Id, isTracking: false);

            if (files.Count > 0)
            {
                string upload = webRootPath + WC.ImagePathCoach;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                var oldFile = Path.Combine(upload, objFromDb.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                сoach.Image = fileName + extension;
            }
            else
            {
                сoach.Image = objFromDb.Image;
            }

            await _coachRepository.UpdateAsync(сoach);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
                return NotFound();

            var coach = await _coachRepository.GetByIdAsync(id);

            if (coach == null)
                return NotFound();

            return View(coach);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var coach = await _coachRepository.GetByIdAsync(id);
            if (coach == null)
            {
                NotFound();
            }

            await _coachRepository.RemoveAsync(coach);

            return RedirectToAction("Index");
        }
    }
}
