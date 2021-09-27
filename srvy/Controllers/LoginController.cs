using Newtonsoft.Json;
using srvy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace srvy.Controllers
{
    public class LoginController : Controller
    {
        AnketDBEntities db = new AnketDBEntities();
        TBL_USERS user = new TBL_USERS();
        public static string Email = "";
        public static string Password = "";
        // GET: Login
        //
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GirisButonu(TBL_USERS us)
        {
            Email = us.Email;
            Password = us.Password;

            var Userinfo = db.TBL_USERS.Where(x => x.Email == Email && x.Password == Password).FirstOrDefault();
            if (Userinfo != null)
            {
                Session.Add("UserEmail", us.Email);
                Session.Add("UserPassword", us.Password);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                us.LoginError = "Hatalı e-mail veya şifre";
                return View("Index", us);
            }
        }
        public ActionResult UyeOl()
        {
            return View("KullaniciKayit");
        }

        public ActionResult KullaniciKayit(TBL_USERS us)
        {
            user.FirstName = us.FirstName;
            user.LastName = us.LastName;
            user.Email = us.Email;
            user.Password = us.Password;
            db.TBL_USERS.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Profil()
        {
            if (Session["UserEmail"] != null && Session["UserPassword"] != null)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        public ActionResult LogOut()
        {
            if (Session["UserEmail"] != null && Session["UserPassword"] != null)
            {
                Session.Abandon();
                return View("Index");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Bilgilerim()
        {
            string sifre = Password;
            var user = db.TBL_USERS.Where(u => u.Password == sifre && u.Email == Email).FirstOrDefault();
            ViewBag.eMail = user.Email;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            return View();
        }
        public ActionResult Anketlerim()
        {
            var userID = db.TBL_USERS.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
            int ID = userID.ID;

            var userSurvey = db.TBL_SURVEY.Where(x => x.UsersID == ID).ToList();
            if (userSurvey != null)
            {
                return View(userSurvey);
            }
            else
            {

                return View();
            }

        }

        public ActionResult PasswordUpdate(TBL_USERS pass)
        {
            if (pass.Password == Password)
            {
                var user = db.TBL_USERS.Where(u => u.Password == pass.Password).FirstOrDefault();
                user.Password = pass.newPassword;
                db.SaveChanges();
                pass.PasswordError = "Şifreniz güncellenmiştir.";
                return View("Bilgilerim", pass);
            }
            else
            {
                pass.PasswordError = "Hatalı şifre";
                return View("Bilgilerim", pass);
            }
        }

        public ActionResult anketOlusturButton()
        {
            return View();
        }
        public ActionResult txtEkle(string[] DynamicTextBox, string[] cevapTextbox, TBL_SURVEY name)
        {
            TBL_SURVEY survey = new TBL_SURVEY();
            TBL_SURVEYQUESTIONS surveyQues = new TBL_SURVEYQUESTIONS();
            TBL_QUESTIONSANSWERS quesAnswers = new TBL_QUESTIONSANSWERS();

            var userID = db.TBL_USERS.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
            int ID = userID.ID;
            survey.SurveyName = name.SurveyName;
            survey.UsersID = ID;
            db.TBL_SURVEY.Add(survey);
            db.SaveChanges();

            var srvy = db.TBL_SURVEY.Where(s => s.SurveyName == name.SurveyName).FirstOrDefault();
            int surveyID = srvy.ID;

            foreach (var i in DynamicTextBox)
            {
                surveyQues.SurveyID = surveyID;
                surveyQues.Questions = i;
            }
            db.TBL_SURVEYQUESTIONS.Add(surveyQues);
            db.SaveChanges();

            var quesID = db.TBL_SURVEYQUESTIONS.Where(s => s.SurveyID == surveyID).FirstOrDefault();
            for (int i = 0; i < cevapTextbox.Length; i++)
            {
                quesAnswers.SurveyID = surveyID;
                quesAnswers.QuestionsID = quesID.ID;
                quesAnswers.Answers = cevapTextbox[i];
                db.TBL_QUESTIONSANSWERS.Add(quesAnswers);
                db.SaveChanges();
            }
            return View("anketOlusturButton");
        }

     
    }
}