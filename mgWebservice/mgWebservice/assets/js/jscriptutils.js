function getCookie(c_name) {
    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start == -1) {
        c_start = c_value.indexOf(c_name + "=");
    }
    if (c_start == -1) {
        c_value = null;
    }
    else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end == -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
    }
    return c_value;
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function areCookiesEnabled() {
    var r = false;
    createCookie("testing", "Hello", 1);
    if (readCookie("testing") != null) {
        r = true;
        eraseCookie("testing");
    }
    return r;
}

function SetScreenWidth(SetWidth) {

    if ((getCookie('screenwidth') < 1) && (areCookiesEnabled == true)) {

        setCookie('screenwidth', screen.availWidth, '28');

        if (screen.availWidth != SetWidth) {
            location.reload();
        }

    }
}

//generic text change
function changetext(box, text) {

    if (document.getElementById(box) == null || document.getElementById(box) == "null" || document.getElementById(box) == undefined || document.getElementById(box) == "undefined") {

    }
    else {

        document.getElementById(box).innerHTML = text;
    }

}

function changeopacity(div, onoroff) {

        if (onoroff == 1) {

            document.getElementById(div).style.boxShadow = "0px 0px 15px #CCC";

        }
        else {

            document.getElementById(div).style.boxShadow = "0 1px 10px rgba(0, 0, 0, 0.065)";

        }
    }



function drawlocationmap(lat,lon,title,zoom)
{

    if (!zoom || zoom == "") {

        zoom = 16;

    }

     var myLatlng = new google.maps.LatLng(lat, lon);
var mapOptions = {
    zoom: zoom,
  center: myLatlng,
  mapTypeId: google.maps.MapTypeId.ROADMAP
}
var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

// To add the marker to the map, use the 'map' property
var marker = new google.maps.Marker({
    position: myLatlng,
    map: map,
    title:title
});

}





