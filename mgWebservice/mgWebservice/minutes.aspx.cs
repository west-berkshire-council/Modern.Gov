using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace mgWebservice
{
    public partial class minutes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            int intPage = 1;

            wbccommgrapp.mgWebService modernGov = new wbccommgrapp.mgWebService();

            XmlNode xnMeeting = modernGov.GetMeeting(2356);

            XmlNodeList xnAgendaItems = xnMeeting.SelectNodes("/agendaitems/agendaitem");


            string strAgendaTitle = "";
            string strAgendaText = "";


            try
            {

                intPage = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);

            }
            catch { }




                XmlNode xnAgendaItem = xnAgendaItems[intPage];


                HttpContext.Current.Response.Write("<h1>" + xnAgendaItem["agendaitemtitle"].InnerText + "</h1>");
                HttpContext.Current.Response.Write("<h2>" + xnAgendaItem["agendatext"].InnerText + "</h2>");
                HttpContext.Current.Response.Write("<div class=\"minuteshtmlbody\">" + xnAgendaItem["minutesnonemptyhtmlbody"].InnerText + "</div>");


                if (intPage > 0)
                {
                    HttpContext.Current.Response.Write("<a href=\"minutes.aspx?page=" + (intPage - 1).ToString() + "\">Previous Item</a>");
                }


                if (xnAgendaItems.Count > (intPage + 1))
                {
                    HttpContext.Current.Response.Write("<a href=\"minutes.aspx?page=" + (intPage + 1).ToString() + "\">Next Item</a>");
                }


  



            int intIndex = 0;

            HttpContext.Current.Response.Write("<h3>Jump to an item</h3>");

            foreach (XmlNode xnAgendaListItem in xnAgendaItems)
            {

                HttpContext.Current.Response.Write("<a href=\"minutes.aspx?page=" + intIndex + "\">" + xnAgendaListItem["agendaitemtitle"].InnerText + "</a><br/>");

                intIndex++;

            }



        }
    }

}
