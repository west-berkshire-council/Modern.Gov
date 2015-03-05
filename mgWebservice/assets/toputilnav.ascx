<%@ Control Language="C#" AutoEventWireup="true" %>
        <div id="header" class="container">           
        <div class="navbar navbar-default">                            
                <div class="navbar-header"><a href="<!--#TheSubsite.SubsiteSettings[WebsiteURL]-->"><span style="width:200px; height:60px; display:table; float:left;"><span class="hidefromview">Home</span></span></a>
     
                </div>
            <div>
<div class="headlinktext">
                <%if(Request.QueryString["display"] != null){%>View this online at <span class="headlinkURL">www.westberks.gov.uk/results</span><%}%></div>
            </div>

        </div>
        </div>



