using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace mgWebservice
{
    public partial class infobar : System.Web.UI.Page
    {
        public string strContent = "";
        public string strType = "";
        public int intElection = 2;
        
        protected void Page_Load(object sender, EventArgs e)
        {
                      
            intElection = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("election").ToString());

            try
            {
                intElection = Convert.ToInt32(Request.QueryString["election"].ToString());
                strType = Request.QueryString["type"].ToString();

            }
            catch { }

            wbccommgrapp.mgWebService modernGov = new wbccommgrapp.mgWebService();
            XmlNode xnMeeting = modernGov.GetElectionResults(intElection);

            XmlNodeList xnAgendaItems = xnMeeting.SelectNodes("/candidates/candidate");

            CandidatesList CandidatesList = new CandidatesList();

            foreach (XmlNode Node in xnAgendaItems)
            {

                Candidate candidate = new Candidate();
                candidate.party = Node["politicalpartytitle"].InnerText;
                candidate.partycolour = Node["politicalpartycolour"].InnerText;
                candidate.votes = Convert.ToInt32(Node["numvotes"].InnerText);

                bool blIsElected = false;

                if (Node["iselected"].InnerText == "True")
                {
                    blIsElected = true;
                }

                candidate.iselected = blIsElected;

                CandidatesList.Add(candidate);


            }


            List<Candidate> SortedList = CandidatesList.OrderBy(o => o.party).ToList();

            PartyList partylist = new PartyList();
            double intTotalVotes = 0;
            double intTotalSeats = 0;

            string strPartyName = "";
            string strPartyColour = "";
            double intVotes = 0;
            double intElected = 0;

            Party party = new Party();

            foreach (Candidate Candidate in SortedList)
            {

                Party thisparty = new Party();
                party = thisparty;

                intTotalVotes += Candidate.votes;

                if (strPartyName == Candidate.party)
                {
                    strPartyName = Candidate.party;
                    strPartyColour = Candidate.partycolour;
                    intVotes += Candidate.votes;

                    if (Candidate.iselected == true)
                    {
                        intElected = intElected + 1;
                        intTotalSeats = intTotalSeats + 1;
                    }

                }
                else
                {


                    party.party = strPartyName;
                    party.partycolour = strPartyColour;
                    party.elected = intElected;
                    party.votes = intVotes;

                    partylist.Add(party);

                    strPartyName = Candidate.party;
                    strPartyColour = Candidate.partycolour;
                    intVotes = Candidate.votes;
                    intElected = 0;

                    if (Candidate.iselected == true)
                    {
                        intElected = intElected + 1;
                        intTotalSeats = intTotalSeats + 1;
                    }

                }


            }


            party.party = strPartyName;
            party.partycolour = strPartyColour;
            party.elected = intElected;
            party.votes = intVotes;

            partylist.Add(party);

            List<Party> SortedPartyList = new List<Party>();


            strContent += "<div class=\"row bar\">";


            if (intElection == 2)
            {
                strContent += "<div class=\"col-xs-12 infobarhead\">2011 Result</div>";
                SortedPartyList = partylist.OrderByDescending(o => o.elected)
                     .ThenBy(o => o.party).ToList();
            }
            else if (strType == "seats")
            {
                strContent += "<div class=\"col-xs-12 infobarhead\">2015 Seats</div>";
                SortedPartyList = partylist.OrderByDescending(o => o.elected)
                     .ThenBy(o => o.party).ToList();
            }
            else if (strType == "percent")
            {
                strContent += "<div class=\"col-xs-12 infobarhead\">2015 Votes</div>";
                                SortedPartyList = partylist.OrderByDescending(o => o.votes)
                     .ThenBy(o => o.party).ToList();
            }
    

            foreach (Party Party in SortedPartyList)
            {

                if (Party.party != "")
                {
                    double dblTotalVotes = Math.Round((Party.votes / intTotalVotes) * 100, 2);

                    string strTotalVotesPercentage = dblTotalVotes.ToString() + "%";
                    if (strTotalVotesPercentage == "NaN%")
                    {
                        strTotalVotesPercentage = "0";
                    }

                    string strForegroundColour = "#FFFFFF";
                    if (Party.partycolour == "Orange")
                    {
                        strForegroundColour = "#000000";
                    }

                    if (strType == "seats")
                    {
                        strContent += "<div class=\"col-lg-12 col-md-12 col-sm-4 col-xs-6 col-vs-12 infobarbox\" style=\"background-color:" + Party.partycolour + "; color:" + strForegroundColour + "\"><span><span class=\"partyname\">" + Party.party.Remove(3) + "</span> <span class=\"partyfigure\">" + Party.elected + "</span></span></div>";
                    }
                    else if (strType == "percent")
                    {
                        strContent += "<div class=\"col-lg-12 col-md-12 col-sm-4 col-xs-6 col-vs-12 infobarbox\" style=\"background-color:" + Party.partycolour + "; color:" + strForegroundColour + "\"><span class=\"partyname\">" + Party.party.Remove(3) + "</span> <span class=\"partyfigure\">" + strTotalVotesPercentage + "</span></span></div>";
                    }                   
                    else
                    {
                        strContent += "<div class=\"col-lg-12 col-md-12 col-sm-4 col-xs-6 col-vs-12 infobarbox\" style=\"background-color:" + Party.partycolour + "; color:" + strForegroundColour + "\"><span class=\"partyname\">" + Party.party.Remove(3) + "</span> <span class=\"partyfigure\">" + Party.elected + "</span></span></div>";
                    }
 
                }

            }

            //strContent += "<div class=\"row partyrow\">";
            //strContent += "<div class=\"col-sm-6 totalseats\">" + intTotalSeats + " of 52 seats declared</div>";
            //strContent += "<div class=\"col-sm-6 totalvotes\">Turnout: " + GetTurnout() + "%</div>";
            strContent += "</div>";


        }

        string GetTurnout()
        {

            string strTurnout = "0";

            wbccommgrapp.mgWebService modernGov = new wbccommgrapp.mgWebService();

            XmlNode xnMeeting = modernGov.GetElectionResults(intElection);

            XmlNodeList xnAgendaItems = xnMeeting.SelectNodes("/electionareas/electionarea");

            double intElectorate = 0;
            double intNumballotpapersissued = 0;

            foreach (XmlNode Node in xnAgendaItems)
            {

                Ward ward = new Ward();
                intElectorate += Convert.ToInt32(Node["electorate"].InnerText);
                intNumballotpapersissued += Convert.ToInt32(Node["numballotpapersissued"].InnerText);
            }

            strTurnout = (Math.Round(((intNumballotpapersissued / intElectorate * 100)), 2)).ToString();

            return strTurnout;
        }



        class Candidate
        {
            public string ward { get; set; }
            public string party { get; set; }
            public string partycolour { get; set; }
            public double votes { get; set; }
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


        class Party
        {

            public string party { get; set; }
            public string partycolour { get; set; }
            public double votes { get; set; }
            public double elected { get; set; }


        }


        class PartyList : List<Party>
        {

            public Party party
            {
                get;
                set;
            }


        }


        class Ward
        {
            public string wardid { get; set; }
            public int seats { get; set; }
            public int electorate { get; set; }
            public int numballotpapersissued { get; set; }

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