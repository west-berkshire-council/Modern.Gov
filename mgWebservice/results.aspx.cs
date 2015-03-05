using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace mgWebservice
{
    public partial class results : System.Web.UI.Page
    {
        public string strContent = "";
        public int intElection = 2;
        
        protected void Page_Load(object sender, EventArgs e)
        {


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



            List<Party> SortedPartyList = partylist.OrderByDescending(o => o.elected)
                     .ThenByDescending(o => o.votes).ToList();



            strContent += "<div class=\"row votesheaderrow hidden-xs\">";
            strContent += "<div class=\"col-sm-6\"></div>";
            strContent += "<div class=\"col-sm-2\">Seats</div>";
            strContent += "<div class=\"col-sm-2\">Votes</div>";
            strContent += "<div class=\"col-sm-2\">%</div>";
            strContent += "</div>";

            foreach (Party Party in SortedPartyList)
            {

                if (Party.party != "")
                {
                    double dblTotalVotes = Math.Round((Party.votes / intTotalVotes) * 100, 2);

                    string strForegroundColour = "#FFFFFF";
                    if (Party.partycolour == "Orange")
                    {
                        strForegroundColour = "#000000";
                    }

                    strContent += "<div class=\"row partyrow\" style=\"background-color:" + Party.partycolour + "; color:" + strForegroundColour + "\">";
                    strContent += "<div class=\"col-sm-6 party\"><span class=\"rowtext\">" + Party.party + "</span></div>";
                    strContent += "<div class=\"col-sm-2 elected\"><span class=\"rowtext\">" + Party.elected + "</span><span class=\"visible-xs\">Seats:&nbsp;</span></div>";
                    strContent += "<div class=\"col-sm-2 votes\"><span class=\"rowtext\">" + Party.votes + "</span></span><span class=\"visible-xs\">Votes:&nbsp;</span></div>";
                    strContent += "<div class=\"col-sm-2 votes\"><span class=\"rowtext\"><span class=\"visible-xs\">Percentage of Votes:&nbsp;</span>" + dblTotalVotes + "%</span></div>";
                    strContent += "</div>";
                }

            }

            strContent += "<div class=\"row partyrow\">";
            strContent += "<div class=\"col-sm-6 totalseats\">" + intTotalSeats + " of 52 seats declared</div>";
            strContent += "<div class=\"col-sm-6 totalvotes\">Turnout: " + GetTurnout() + "%</div>";
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

            strTurnout = (Math.Round(((intNumballotpapersissued / intElectorate * 100)),2)).ToString();

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