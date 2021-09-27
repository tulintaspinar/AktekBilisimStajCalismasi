using Microsoft.Ajax.Utilities;
using srvy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace srvy.Controllers
{
    public class HomeController : Controller
    {
        AnketDBEntities db = new AnketDBEntities();

        public class QuestionsANDAnswers
        {
            public List<TBL_SURVEYQUESTIONS> Questions { get; set; }
            public List<TBL_QUESTIONSANSWERS> Answers { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Anketler()
        {
            var survey = db.TBL_SURVEY.ToList();
            return View(survey);
        }

        public ActionResult anketOylamaSayfasi(int SurveyID)
        {
            if (Session["UserEmail"] != null && Session["UserPassword"] != null)
            {
                var surveyName = db.TBL_SURVEY.Where(s => s.ID == SurveyID).FirstOrDefault();
                ViewBag.SurveyName = surveyName.SurveyName;

                var SurveyQuestion = db.TBL_SURVEYQUESTIONS.ToList();

                List<TBL_SURVEYQUESTIONS> QuestionsList = new List<TBL_SURVEYQUESTIONS>();
                foreach (var q in SurveyQuestion)
                {
                    if (q.SurveyID == SurveyID)
                    {
                        QuestionsList.Add(new TBL_SURVEYQUESTIONS()
                        {
                            ID = q.ID,
                            SurveyID = q.SurveyID,
                            Questions = q.Questions,
                        });
                    }

                }


                var QuestionsAnswers = db.TBL_QUESTIONSANSWERS.ToList();
                List<TBL_QUESTIONSANSWERS> AnswersList = new List<TBL_QUESTIONSANSWERS>();
                foreach (var a in QuestionsAnswers)
                {
                    if (a.SurveyID == SurveyID)
                    {
                        AnswersList.Add(new TBL_QUESTIONSANSWERS()
                        {
                            ID = a.ID,
                            SurveyID = a.SurveyID,
                            QuestionsID = a.QuestionsID,
                            Answers = a.Answers,
                        });
                    }

                }

                var SurveyQuestionsAnswers = new QuestionsANDAnswers()
                {
                    Questions = QuestionsList,
                    Answers = AnswersList,
                };

                return View(SurveyQuestionsAnswers);
            }
            else
            {
                TempData["Message"] = "Lütfen Giriş Yapın";
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult anketOyla(HomeController.QuestionsANDAnswers obj)
        {
            var e_mail = Session["UserEmail"].ToString();
            var password = Session["UserPassword"].ToString();
            var user = db.TBL_USERS.Where(u => u.Email == e_mail && u.Password == password).FirstOrDefault();
            var IPAdress = GetClientIp();
            bool sonuc = false;
            TBL_USERSRESPONSE response = new TBL_USERSRESPONSE();

            try
            {
                for (int i = 0; i < obj.Questions.Count; i++)
                {
                    var id = obj.Questions[i].ID;
                    var answers = db.TBL_QUESTIONSANSWERS.Where(s => s.ID == id).FirstOrDefault();

                    response.UserID = user.ID;
                    response.SurveyID = answers.SurveyID;
                    response.QuestionsID = answers.QuestionsID;
                    response.Response = answers.Answers;
                    db.TBL_USERSRESPONSE.Add(response);
                    db.SaveChanges();
                    
                }

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.UserName = "sitesianket@gmail.com";
                WebMail.Password = "anketsitesi2020";
                WebMail.EnableSsl = true;
                try
                {
                    WebMail.Send(
                        to: e_mail, subject: "ANKETE KATILDIĞINIZ İÇİN TEŞEKKÜRLER",
                        body: "Bu bir web mail denemesidir.",
                        replyTo: e_mail, isBodyHtml: true);

                    sonuc = true;
                }
                catch (Exception ex)
                {
                    ViewBag.Hata = ex.Message;
                }
                ViewBag.Sonuc = sonuc;

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
            }
            
        }
        public static string GetClientIp()
        {
            var ipAddress = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null && System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            return ipAddress;
        }
    }
}