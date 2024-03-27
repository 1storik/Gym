using Gym.Models;
using Gym.Repository.IRepository;
using Gym.Utility;
using Gym.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gym.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public MembershipController(IMembershipRepository membershipRepository, ICoachRepository coachRepository, 
            ISubscriptionRepository subscriptionRepository, IWebHostEnvironment webHostEnvironment)
        {
            _membershipRepository = membershipRepository;
            _coachRepository = coachRepository;
            _subscriptionRepository = subscriptionRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Membership> membership = await _membershipRepository.GetAllAsync(includeProperties: "Coach");
            return View(membership);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
                return NotFound();

            var membership = await _membershipRepository.GetAsync(m => m.Id == id, includeProperties: "Coach");
            var subscriptions =
                await _subscriptionRepository.GetAllAsync(s => s.MembershipId == id, includeProperties: "Client");

            var membersipDetailVM = new MembershipDetailVM()
            {
                Membership = membership,
                Subscriptions = subscriptions
            };

            return View(membersipDetailVM);
        }

        public async Task<IActionResult> Create()
        {
            var coaches = await _coachRepository.GetAllAsync();

            MembershipVM vm = new MembershipVM()
            {
                Coach = coaches.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(), 
                    Text = $"{c.FirstName} {c.LastName}"
                }).ToList()
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Membership membership)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePathMember;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            membership.Image = fileName + extension;
            membership.Coach = await _coachRepository.GetByIdAsync(membership.Coach.Id);

            await _membershipRepository.CreateAsync(membership);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var coaches = await _coachRepository.GetAllAsync();

            MembershipVM membershipVm = new MembershipVM()
            {
                Coach = coaches.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"
                }).ToList(),
                Membership = await _membershipRepository.GetByIdAsync(id)
            };

            if (membershipVm == null)
                return NotFound();

            return View(membershipVm);
        }
    

        [HttpPost]
        public async Task<IActionResult> Edit(Membership membership)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            var objFromDb = await _membershipRepository.GetAsync(m => m.Id == membership.Id, isTracking: false);

            if (files.Count > 0)
            {
                string upload = webRootPath + WC.ImagePathMember;
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

                membership.Image = fileName + extension;
            }
            else
            {
                membership.Image = objFromDb.Image;
            }

            membership.Coach = await _coachRepository.GetByIdAsync(membership.Coach.Id);

            await _membershipRepository.UpdateAsync(membership);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
                return NotFound();

            var membership = await _membershipRepository.GetByIdAsync(id);

            if (membership == null)
                return NotFound();

            return View(membership);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(Guid MembershipId)
        {
            var membership = await _membershipRepository.GetByIdAsync(MembershipId);
            if (membership == null)
            {
                NotFound();
            }

            await _membershipRepository.RemoveAsync(membership);

            return RedirectToAction("Index");
        }
    }
}
