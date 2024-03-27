using Gym.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gym.VM
{
    public class ClientVM
    {
        public Client Client { get; set; }
        public List<SelectListItem> genderList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" },
            new SelectListItem { Value = "NonBinary", Text = "Non-Binary" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };
    }
}
