using IronDome.DAL;
using IronDome.Models;
using IronDome.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IronDome.Controllers
{
    public class ThreatController : Controller
    {
        public IActionResult Index()
        {
            List<Threat> threats = Data.Get.Threats
                .Include(t => t.Org)
                .Include(t => t.missle_type)
                .ToList();
            return View(threats);
        }
        public IActionResult Create()
        {
            List<Missle>? msl = Data.Get.Missles.ToList();
            List<TerrorOrg>? orgList = Data.Get.TerrorOrgs.ToList();

            CreateThreatViewModel model = new CreateThreatViewModel
            {
                Types = msl.Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.missle_type }).ToList(),
                TerrorOrgs = orgList.Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList(),
            };
            return View(model);
        
        }
        [HttpPost]
        public IActionResult Create(CreateThreatViewModel model)
        {
            TerrorOrg? org = Data.Get.TerrorOrgs.Find(model.OrgId);
            Missle? missleType = Data.Get.Missles.Find(model.ThreatTypeId);
            if (org == null || missleType == null) 
            {
                return NotFound();              
            }
            Threat newThreat = new Threat
            {
                Org = org,
                missle_type = missleType
            };
                //Task.Run(() => StartAttack(newThreat
            Data.Get.Threats.Add(newThreat);
            Data.Get.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Launch(int id)
        { 
            Threat? t = Data.Get.Threats.Find(id);
            if (t == null)
            {
                return NotFound();
            }
            t.status = Utils.ThreatStatus.Active;
            t.fire_at = DateTime.Now;
            Data.Get.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
