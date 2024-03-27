using Gym.Models;
using Gym.Repository.IRepository;
using Gym.Utility;
using Gym.VM;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Gym.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMembershipRepository _membershipRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public ClientController(IClientRepository clientRepository, IMembershipRepository membershipRepository, 
            ISubscriptionRepository subscriptionRepository, IWebHostEnvironment webHostEnvironment)
        {
            _clientRepository = clientRepository;
            _webHostEnvironment = webHostEnvironment;
            _membershipRepository = membershipRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var client = await _clientRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                client = client.Where((p => p.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            p.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            p.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            return View(client);
        }

        public IActionResult Create()
        {
            ClientVM clientVm = new ClientVM();

            return View(clientVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Client client)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePathClient;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            client.Image = fileName + extension;
            client.RegistrationDate = DateTime.Today;

            await _clientRepository.CreateAsync(client);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid Id)
        {
            var client = await _clientRepository.GetByIdAsync(Id);
            var clientMembership = await _membershipRepository.GetAllAsync(m => m.Subscription.Any(c => c.ClientId == Id), includeProperties: "Subscription");

            var allMemberships = await _membershipRepository.GetAllAsync();

            var otherMemberships = allMemberships.Except(clientMembership).ToList();

            ClientMembershipVM clientMembershipVm = new ClientMembershipVM
            {
                Client = client,
                ClientMemberships = clientMembership,
                OtherMemberships = otherMemberships
            };

            return View(clientMembershipVm);
        }


        public async Task<IActionResult> Edit(Guid Id)
        {
            ClientVM clientVM = new ClientVM();

            clientVM.Client = await _clientRepository.GetByIdAsync(Id);
            if (clientVM == null)
                return NotFound();

            return View(clientVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientVM clientVM)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            var objFromDb = await _clientRepository.GetAsync(c => c.Id == clientVM.Client.Id, isTracking: false);

            if (files.Count > 0)
            {
                string upload = webRootPath + WC.ImagePathClient;
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

                clientVM.Client.Image = fileName + extension;
            }
            else
            {
                clientVM.Client.Image = objFromDb.Image;
            }

            clientVM.Client.Id = objFromDb.Id;
            clientVM.Client.RegistrationDate = objFromDb.RegistrationDate;

            await _clientRepository.UpdateAsync(clientVM.Client);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            if (Id == null)
                return NotFound();

            var post = await _clientRepository.GetByIdAsync(Id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            var post = await _clientRepository.GetByIdAsync(Id);
            if (post == null)
            {
                NotFound();
            }

            await _clientRepository.RemoveAsync(post!);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToMembership(Guid clientId, Guid membershipId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            var membership = await _membershipRepository.GetByIdAsync(membershipId);

            if (client != null && membership != null)
            {
                var subscription = new Subscription()
                {
                    ClientId = clientId,
                    MembershipId = membershipId,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Now.AddMonths(1),
                    Freeze = false
                };
                await _subscriptionRepository.CreateAsync(subscription);

                return RedirectToAction("Details", new { id = clientId });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveToMembership(Guid clientId, Guid membershipId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId);
            var membership = await _membershipRepository.GetByIdAsync(membershipId);

            if (client != null && membership != null)
            {
                var subscription =
                   await _subscriptionRepository.GetAsync(s => s.ClientId == clientId && s.MembershipId == membershipId);

                await _subscriptionRepository.RemoveAsync(subscription);

                return RedirectToAction("Details", new { id = clientId });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> FreezeMembership(Guid clientId, Guid membershipId)
        {
            var subscription =
                await _subscriptionRepository.GetAsync(s => s.ClientId == clientId && s.MembershipId == membershipId);

            subscription.EndDate = subscription.EndDate.AddDays(7);
            subscription.Freeze = true;
            subscription.FreezeDate = DateTime.Today;

            await _subscriptionRepository.UpdateAsync(subscription);

            return RedirectToAction("Details", new { id = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> UnfreezeMembership(Guid clientId, Guid membershipId)
        {
            var subscription =
                await _subscriptionRepository.GetAsync(s => s.ClientId == clientId && s.MembershipId == membershipId);


            int daysFrozen = (int)(DateTime.Today - subscription.FreezeDate).TotalDays;

            subscription.EndDate = subscription.EndDate.AddDays(daysFrozen - 7);
            subscription.Freeze = false;

            await _subscriptionRepository.UpdateAsync(subscription);

            return RedirectToAction("Details", new { id = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> TogglePresence(Guid Id)
        {
            var client = await _clientRepository.GetByIdAsync(Id);

            if (client.Presence)
            {
                client.Presence = false;
                client.NumberOfShelf = null;
            }
            else
            {
                client.Presence = true;
                client.NumberOfShelf = await GenerateLockerNumber(); 
            }

            await _clientRepository.UpdateAsync(client);

            return RedirectToAction("Details", new { id = Id });
        }

        [HttpGet]
        public async Task<IActionResult> Present()
        {
            var clients = await _clientRepository.GetAllAsync(c => c.Presence,
                orderBy: q => q.OrderBy(c => c.NumberOfShelf));

            return View(clients);
        }

        private async Task<int> GenerateLockerNumber()
        {
            Random _random = new Random();

            var lockerNumbers = (await _clientRepository.GetAllAsync(c => c.Presence == true)).
                Select(c => c.NumberOfShelf).ToList();

            HashSet<int?> assignedLockerNumbers = new HashSet<int?>(lockerNumbers); 

            while (true)
            {
                int lockerNumber = _random.Next(1, 51); 

                if (!assignedLockerNumbers.Contains(lockerNumber))
                {
                    return lockerNumber;
                }
            }
        }
    }
}
