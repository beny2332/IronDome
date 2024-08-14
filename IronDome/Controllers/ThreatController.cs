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
        public static Dictionary<string, CancellationTokenSource> ThreatMap = new();
        
        public IActionResult Index()
        {
            List<Threat> threats = Data.Get.Threats
                .Include(t => t.Org)
                .Include(t => t.type)
                .ToList();
            return View(threats);
        }
        public IActionResult Create()
        {
            List<Missle>? msl = Data.Get.Missles.ToList();
            List<TerrorOrg>? orgList = Data.Get.TerrorOrgs.ToList();

            CreateThreatViewModel model = new CreateThreatViewModel
            {
                Types = msl.Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList(),
                TerrorOrgs = orgList.Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList(),
            };
            return View(model);
        
        }
        [HttpPost, ValidateAntiForgeryToken]
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
                type = missleType
            };
            Data.Get.Threats.Add(newThreat);
            Data.Get.SaveChanges();
                //Task.Run(() => StartAttack(newThreat

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

            // create cancelation token.
            CancellationTokenSource cts = new ();

            // create & run task.
            Task task = Task.Run(async () => 
            {
                // print status every 2 seconds .
                int timer = t.response_time;
                while (!cts.IsCancellationRequested && timer > 0) 
                {
                    Console.WriteLine($"{t.Id} threat is {timer} seconds away");
                    await Task.Delay (2000);
                    timer -= 2;
                }
                if (cts.IsCancellationRequested)
                {
                    t.status = Utils.ThreatStatus.Failed;
                }
                else
                {
                    t.status = Utils.ThreatStatus.Succeeded;
                }
                cts.Cancel ();
                ThreatMap.Remove(t.Id.ToString());
                Data.Get.SaveChanges();
            }, cts.Token);


            // save the threat in the dictionary.
            ThreatMap[t.Id.ToString()] = cts;
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeffenceArea()
        { 
            return View(Data.Get.Threats
                .Include(t=>t.type)
                .Include(t=>t.Org)
                .ToList()
                .Where(t=>t.status != Utils.ThreatStatus.Inactive)); 
        }

        public IActionResult Intercept(int tid, int did)
        {
            // find threat.
            Threat? t = Data.Get.Threats.Find(tid);

            // find deffence.
            DefenceAmmunition? da = Data.Get.DefenceAmmunitions.Find(did);

            // make sure not null.
            if (t == null || da == null)
            {
                return NotFound();
            }
            if (da.amount < 1)
            {
                return BadRequest($"{da.name} is out of ammunition! Please refill!");
            }

            // cancel task and delete from dictionary.
            ThreatMap[tid.ToString()].Cancel();
            ThreatMap.Remove(tid.ToString());

            // reduce the {da} amount and update the status of the threat.
            --da.amount;
            t.status = Utils.ThreatStatus.Failed;

            Data.Get.SaveChanges();
            Thread.Sleep(500);

            return RedirectToAction(nameof (DeffenceArea));
        }
    }
}
