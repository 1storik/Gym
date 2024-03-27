using Gym.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gym.VM
{
    public class MembershipVM
    {
        public Membership Membership { get; set; }
        public List<SelectListItem> Coach { get; set; } = new List<SelectListItem>();
    }
}
