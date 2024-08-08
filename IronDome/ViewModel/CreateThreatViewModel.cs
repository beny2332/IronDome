using IronDome.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace IronDome.ViewModel
{
    public class CreateThreatViewModel
    {
        public List<SelectListItem> TerrorOrgs {  get; set; }
        public int OrgId { get; set; }
        public List<SelectListItem> Types { get; set; }
        public int ThreatTypeId {  get; set; }

    }
}
