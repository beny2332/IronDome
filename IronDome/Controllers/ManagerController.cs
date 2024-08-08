using IronDome.DAL;
using IronDome.Models;
using IronDome.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace IronDome.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            List<DefenceAmmunition> defences = Data.Get.DefenceAmmunitions.ToList();
            return View(defences);
        }

        public IActionResult AttackArea()
        {
            List<Threat> threats = Data.Get.Threats.ToList();
            return View(threats);
        }

        public IActionResult updateDefenceAmmiunition(int dfid, int amount)
        {
            DefenceAmmunition? da = Data.Get.DefenceAmmunitions.Find(dfid);
            da.amount = amount;
            Data.Get.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
