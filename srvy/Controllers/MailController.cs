using srvy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace srvy.Controllers
{
    public class MailController : Controller
    {
        AnketDBEntities db = new AnketDBEntities();
        // GET: Mail
        public ActionResult Index(string btnKatilim)
        {
            var btnValue = btnKatilim.Split();
            int anketID = Convert.ToInt32(btnValue[1]);
            var anketeKatilan = db.TBL_USERSRESPONSE.Where(r => r.SurveyID == anketID).ToList();
            var anketadi = db.TBL_SURVEY.Where(a => a.ID == anketID).FirstOrDefault();
            ViewBag.Anket = anketadi.SurveyName;
            return View(anketeKatilan);
        }
    }
}