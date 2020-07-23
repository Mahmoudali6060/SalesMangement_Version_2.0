
$(".validate").keydown(
    function () {

        var element = $(this).val();
        if (element) {
            $(this).closest("div").find("p").hide();
            $(this).css('border-color', '');
        }
        else {
            $(this).closest("div").find("p").show();
            $(this).css('border-color', 'Red');
        }
    });

function getLocalDate(dateUTC) {
    var now = new Date(dateUTC);
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = day + "-" + month + "-" + now.getFullYear();
    return today;
}

function getLocalDateForInput(dateUTC) {
    var now = new Date(dateUTC);
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + month + "-" + day;
    return today;
}

function turnOnTab(elementId) {
    $('#' + elementId + '').on('shown.bs.modal', function () {
        $(document).off('focusin.modal');
    });
}

function getAllFarmers(currentPage) {
    var url;
    if (currentPage == undefined)
        url = "/Farmers/List";
    else
        url = "/Farmers/GetPagedList?currentPage=" + currentPage;

    var farmers = [];
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            farmers = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return farmers;
}

function getFarmerById(farmerId) {

    var farmer;
    $.ajax({
        url: "/Farmers/GetById/" + farmerId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            farmer = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return farmer;
}


function getImagesUrl() {
    var imagesUrl = $("#imagesUrl").val();
    return imagesUrl;
}

function getAllSellers() {
    var sellers = [];
    $.ajax({
        url: "/Sellers/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            sellers = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

    return sellers;
}

function getSellerById(sellerId) {

    var seller;
    $.ajax({
        url: "/Sellers/GetById/" + sellerId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            seller = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return seller;
}

function getAllRoles() {
    var roles = [];
    $.ajax({
        url: "/Roles/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            roles = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return roles;
}

function getRoleById(roleId) {
    var role;
    $.ajax({
        url: "/Roles/GetById/" + roleId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            role = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return role;
}

function convertToIndiaNumbers(text) {

    //return text.replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
    //    .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');

    //var id = ['٠', '١', ' ٢', '۳', '٤', '٥', '٦', '۷', '۸', '۹'];
    //return text.replace(/[0-9]/g, function (w) {
    //    return id[+w]
    //});
    //text = 3.5;
    text += "";
    let splitted = text.split(".");
    let part1;
    let part2;
    let result = "";
    part1 = splitted[0].replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
        .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');
    result = "" + part1;


    if (splitted[1] != null) {
        part2 = splitted[1].replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
            .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');
        result = "" + part2 + ', ' + "" + part1;
    }
    return result;
    //if (text == undefined) return '';
    //var str = $.trim(text.toString());
    //if (str == "") return "";
    //str = str.replace('0','۰');
    //str = str.replace('1','۱');
    //str = str.replace('2','۲');
    //str = str.replace('3','۳');
    //str = str.replace('4','٤');
    //str = str.replace('5','٥');
    //str = str.replace('6','٦');
    //str = str.replace('7','۷');
    //str = str.replace('8','۸');
    //str = str.replace('9','۹');
    //return str;

    //String.prototype.toEnDigit = function () {
    //    return this.replace(/[\u06F0-\u06F9]+/g, function (digit) {
    //        var ret = '';
    //        for (var i = 0, len = digit.length; i < len; i++) {
    //            ret += String.fromCharCode(digit.charCodeAt(i) - 1728);
    //        }

    //        return ret;
    //    });
    //};


}

function getfolder(e) {
    debugger;
    var files = e.target.files;
    var path = files[0].webkitRelativePath;
    var Folder = path.split("/");
    alert(Folder[0]);
}

function backupDatabase() {
    debugger;
    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/BackupDatabase",
        data: databaseEntity,
        type: "POST",
        success: function () {
            debugger;
            //alert("Done");
            $('#database-Form').modal('hide');
            //swal("نسخ البيانات", "تم انشاء قاعدة بيانات احتياكية بنجاح !", "عملية ناجحة");
            alert("تم انشاء قاعدة بيانات احتياطية بنجاح");

        },
        error: function () {
            debugger;
            alert("حدث خطأ في نسخ قاعدة البيانات ");
        }
    });
}

function restoreDatabase() {
    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/RestoreDatabase",
        data: databaseEntity,
        type: "POST",
        success: function (result) {
            $('#database-Form').modal('hide');
            alert("تم استرجاع قاعدة بيانات الاحتياطية بنجاح");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function fillDatabaseEntity() {
    var databaseEntity = {
        DatabaseName: $('#DatabaseName').val(),
        FilePath: $('#FilePath').val(),
        ConnectionString: $('#ConnectionString').val(),
    };
    return databaseEntity;
}

function loadDatabaseEntity() {
    $('#ConnectionString').val("Server =.\\SQLEXPRESS; Database = SalesManagement; Trusted_Connection = True;");
    $('#DatabaseName').val("SalesManagement");
    $("#FilePath").val("E://Backup_Dabase");
}


function getReportHead(title) {
    var html = `<html>
<head>
<title>`+ title + `</title>
 <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui">
    <meta name="description" content="Robust admin is super flexible, powerful, clean &amp; modern responsive bootstrap 4 admin template with unlimited possibilities.">
    <meta name="keywords" content="admin template, robust admin template, dashboard template, flat admin template, responsive admin template, web app">
    <meta name="author" content="PIXINVENT">
    <title>Sales Management</title>
    <link rel="apple-touch-icon" sizes="60x60" href="/Content/app-assets/images/ico/apple-icon-60.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/Content/app-assets/images/ico/apple-icon-76.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/Content/app-assets/images/ico/apple-icon-120.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/Content/app-assets/images/ico/apple-icon-152.png">
    <link rel="shortcut icon" type="image/x-icon" href="/Content/app-assets/images/ico/favicon.ico">
    <link rel="shortcut icon" type="image/png" href="/Content/app-assets/images/ico/favicon-32.png">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-touch-fullscreen" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="default">

    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/fonts/icomoon.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/fonts/flag-icon-css/css/flag-icon.min.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/vendors/css/extensions/pace.css">
  
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/bootstrap-extended.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/app.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/colors.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/custom-rtl.css">
   
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/core/menu/menu-types/vertical-menu.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/core/menu/menu-types/vertical-overlay-menu.css">
    <link rel="stylesheet" type="text/css" href="/Content/app-assets/css-rtl/core/colors/palette-gradient.css">
    
    <link rel="stylesheet" type="text/css" href="/css/site.css">
    <link rel='stylesheet' href='/report/css/style.css'>
    <link rel='stylesheet' href='/report/css/print.css' media="print">

</head>
<body onload="window.print()">`;
    return html;
}

function isEmpty(text) {
    if (text == undefined || text == "" || text == null) {
        return true;
    }
    return false;
}

function getReportAuthor() {
    return `<div class="col-lg-12 author">
                            <table style="width:100%;border:none;font-weight:bold">
                                <tr>
                                    <td style="width:30%;border:none;">01093162036</td>
                                    <td style="width:70%;border:none;">  الدعم الفني والبرمجيات م/محمود علي</td>
                                </tr>
                             </table>
                    </div>`;
}


var elements = document.getElementsByClassName("arrow-togglable");
//var length = elements.length;
//var currentIndex = 0;

document.onkeydown = function (e) {
    let targetIndex = e.target.tabIndex;
    
    let rowNumber= getRowNumber(e);
    var index = targetIndex + (parseInt( rowNumber-1 )* 5);
    switch (e.keyCode) {
        case 39:
            currentIndex = (index == 0) ? elements.length - 1 : --index;
            elements[currentIndex].focus();
            break;
        case 37:
            currentIndex = ((index + 1) == elements.length) ? 0 : ++index;
            elements[currentIndex].focus();
            break;
    }
};

function getRowNumber(e){
    if (e.target.id.startsWith("Quantity")) {
        return e.target.id.split('Quantity')[1];
    }
    if (e.target.id.startsWith("Weight")) {
        return e.target.id.split('Weight')[1];
    }
    if (e.target.id.startsWith("Price")) {
        return e.target.id.split('Price')[1];
    }
    if (e.target.id.startsWith("SellingPrice")) {
        return e.target.id.split('SellingPrice')[1];
    }
    if (e.target.id.startsWith("Sellers")) {
        return e.target.id.split('Sellers')[1];
    }
}