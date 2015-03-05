using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace mgWebservice
{
    public partial class wardresults : System.Web.UI.Page
    {
        public string strContent = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            int intStartNumber = 0;
            
            try
            {
                intStartNumber = Convert.ToInt32(Request.QueryString["start"].ToString());
            }
            catch { }
            
            wbccommgrapp.mgWebService modernGov = new wbccommgrapp.mgWebService();

            XmlNode xnMeeting = modernGov.GetElectionResults(2);

            XmlNodeList xnAgendaItems = xnMeeting.SelectNodes("/candidates/candidate");

            CandidatesList CandidatesList = new CandidatesList();

            foreach(XmlNode Node in xnAgendaItems)
            {

                Candidate candidate = new Candidate();

                candidate.name = Node["candidatename"].InnerText;
                candidate.ward = Node["areatitle"].InnerText;
                candidate.wardid = Convert.ToInt32(Node["electionareaid"].InnerText);
                candidate.party = Node["politicalpartytitle"].InnerText;
                candidate.partycolour = Node["politicalpartycolour"].InnerText;

                bool blIsElected = false;

                if (Node["iselected"].InnerText == "True")
                {
                    blIsElected = true;
                }

                candidate.iselected = blIsElected;

                CandidatesList.Add(candidate);
            }


            XmlNode xnWards = modernGov.GetElectionResults(2);

            XmlNodeList xnWardItems = xnWards.SelectNodes("/electionareas/electionarea");

            WardList WardList = new WardList();

            foreach (XmlNode Node in xnWardItems)
            {

                Ward ward = new Ward();

                ward.seats = Convert.ToInt32(Node["numseats"].InnerText);
                ward.wardid = Convert.ToInt32(Node["electionareaid"].InnerText);
                ward.electorate = Convert.ToInt32(Node["electorate"].InnerText);
                ward.numballotpapersissued = Convert.ToInt32(Node["numballotpapersissued"].InnerText);
                ward.status = Node["status"].InnerText;

                WardList.Add(ward);
            }


            WardList.RemoveRange(0, intStartNumber);

            int i = 0;

            foreach (Ward Ward in WardList)
            {
                string strWaitingText = "Awaiting Result";

                strContent += "<div class=\"row ward\">";

                List<Candidate> WardIndList = CandidatesList.Where(x => x.wardid == Ward.wardid).ToList();
                strContent += "<div class=\"col-sm-6\"><h2>" + WardIndList[0].ward + "</h2></div>";

                if (Ward.status == "Published")
                {
                    strContent += "<div class=\"col-sm-6\"><h2 class=\"headturnout hidden-xs\">Turnout: " + Math.Round((((double)Ward.numballotpapersissued / (double)Ward.electorate) * 100), 2).ToString() + "%</h2></div>";
                }


                strContent += "</div><div class=\"row\"><div class=\"col-sm-12\">";

                if (Ward.status == "Published")
                {

                    List<Candidate> ElectedList = WardIndList.Where(x => x.iselected == true).ToList();

                    foreach (Candidate Candidate in ElectedList)
                    {

                        string strForegroundColour = "#FFFFFF";
                        if (Candidate.partycolour == "Orange")
                        {
                            strForegroundColour = "#000000";
                        }

                        strContent += "<div class=\"row result\" style=\"background-color:" + Candidate.partycolour + "; color:" + strForegroundColour + "\">";
                        strContent += "<div class=\"col-sm-8 name\"><span class=\"rowtext\">" + Candidate.name + "</span></div>";
                        strContent += "<div class=\"col-sm-4 party\"><span class=\"rowtext\">" + Candidate.party + "</span></div>";
                        strContent += "</div>";

                    }


                }
                else
                {
                    for (int iward = 1; iward <= Ward.seats; iward++)
                    {
                        strContent += "<div class=\"row result\">";
                        strContent += "<div class=\"col-sm-8 name\"><span class=\"rowtext\">" + strWaitingText + "</span></div>";
                        strContent += "<div class=\"col-sm-4 party\"></div>";
                        strContent += "</div>";
                    }

                }

                strContent += "</div></div>";

                if (i > 3) break;
                i++;
            }
            
        }


        class Candidate
        {

            public string name { get; set; }
            public string ward { get; set; }
            public int wardid { get; set; }
            public string party { get; set; }
            public string partycolour { get; set; }
            public bool iselected { get; set; }

        }


        class CandidatesList : List<Candidate>
        {

            public Candidate candidate
            {
                get;
                set;
            }


        }



        class Ward
        {
            public int wardid { get; set; }
            public int seats { get; set; }
            public int electorate { get; set; }
            public int numballotpapersissued { get; set; }
            public string status { get; set; }

        }


        class WardList : List<Ward>
        {

            public Ward ward
            {
                get;
                set;
            }

        }
    }

 
}