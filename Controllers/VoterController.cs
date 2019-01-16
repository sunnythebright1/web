using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectoralSystem.Models;
using System.Data;
using System.Data.SqlClient;
namespace ElectoralSystem.Controllers
{
    public class VoterController : Controller
    {
        GeneralPublic gp = new GeneralPublic();
        GPContext context = new GPContext();
        Candidate can = new Candidate();
        Voter voter = new Voter();


        // GET: Voter
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Authenticate(decimal NIC, int OTP)
        {

            //  var candidate = context.Candidates.SqlQuery("Select AC.NIC_No from Applied_Candidates AC,General_Public GP where AC.NIC_No=GP.NIC_No AND AC.NIC_No=4250136948236");

            //            SqlConnection con = new SqlConnection(@"Data Source = .; Initial Catalog = Fyp_electoral_populated; Integrated Security = True;");
            //          SqlCommand cmd = new SqlCommand("spt_constituencyResults");
            //      con.Open();
            //        cmd.ExecuteReader();

            if (voter.authenticate(NIC, OTP) == false)
            {
                TempData["Error"] = 2;
                return View("Index");
                
            }
            else
            {



                gp = context.Publics.SingleOrDefault(n => n.NIC_No == NIC);



                if (gp == null)
                {
                    TempData["Error"] = 1;
                    return Redirect("/Voter/Index");

                }
                else if (NIC == gp.NIC_No)
                {

                    TempData["Name"] = gp.Name;
                    TempData["VoterNIC"] = NIC;
                    TempData["Type"] = "Voter";
                    Session["NIC"] = TempData["VoterNIC"];

                    return RedirectToAction("Verification");
                }

                return Redirect("/Voter/Index");

            }
        }
        public ActionResult Verification()
        {
            decimal NIC = (decimal)TempData["VoterNIC"];
            
            TempData["Name2"] = TempData["Name"];
          var s=  can = context.Candidates.SqlQuery("Select AC.NIC_No as NIC_No, Name as Name from Applied_Candidates AC,General_Public GP where AC.NIC_No=GP.NIC_No AND AC.NIC_No=" + NIC).FirstOrDefault();

            
           if (s != null)
            { 
                decimal canNIC = can.NIC_No;
                if (NIC == canNIC)
                {
                    TempData["canNIC"] = canNIC;
                    
                    TempData["Type"] = "Candidate";
                    return View();
                    //candidate code here
                }
                return View("/Views/Voter/Index.cshtml"); 
            }
            else
            {
                return View("Verification");//(voter code here;
            }

        }
        public ActionResult abc()
        {

            TempData["Name3"] = TempData["Name2"];
            string type = TempData["Type"].ToString();
            if (type == "Voter")
            {
                return RedirectToAction("VotingMain");
            }
            else if (type == "Candidate")
            {
                return Redirect("/Candidate/Index");
            }
            else
                return Redirect("https://www.bing.com");
        }
        public ActionResult VotingMain(Voter vote)
        {
            List<Voter> voters = new List<Voter>();
            vote.NIC = (decimal)Session["NIC"];
            vote.getVoter();
            voters = vote.voteConstituency();
            
            if (vote.Name == null || vote.City == null || vote.naNo < 1)
            {
                vote = null;
            }
            return View(voters.ToList());
        }
        public ActionResult VotingNA(int choice,Voter vote)
        {
            
            
            List<Voter> voters = new List<Voter>();
            vote.NIC = (decimal)Session["NIC"];
            vote.getVoter();
            voters = vote.voteNA();

            if (vote.Name == null || vote.City == null || vote.naNo < 1)
            {
                vote = null;
            }
            return View(voters.ToList());
        }
    }
}