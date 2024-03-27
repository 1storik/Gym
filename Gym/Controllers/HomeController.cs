using Gym.Repository.IRepository;
using Gym.VM;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly ICoachRepository _coachRepository;

        public HomeController(IMembershipRepository membershipRepository, ICoachRepository coachRepository)
        {
            _membershipRepository = membershipRepository;
            _coachRepository = coachRepository;
        }

        public async Task<IActionResult> Index()
        {
            var membership = await _membershipRepository.GetAllAsync();
            var coach = await _coachRepository.GetAllAsync();

            HomeVM vm = new HomeVM()
            {
                Memberships = membership,
                Coachs = coach
            };


            return View(membership);
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            if (Id == null)
                return NotFound();

            var membership = await _membershipRepository.GetAsync(m => m.Id == Id, includeProperties: "Coach");

            if (membership == null)
                return NotFound();

            return View(membership);
        }
    }
}