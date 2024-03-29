﻿
$(document).ready(function () {
    setCompanyFolderInLocalStore();

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "3000",
        "hideDuration": "3000",
        "timeOut": "5000",
        "extendedTimeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

});

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
    var today = now.getFullYear() + "/" + month + "/" + day
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


function setCompanyFolderInLocalStore() {
    //TO-Do>>You must refactor LATER ! >> Babe :) 
    clearCompanyFolderFromLocalStore();
    var companyFolder = $("#imagesUrl").val();//Company Folder 
    localStorage.setItem('companyFolder', companyFolder);
}

function getCompanyFolderFromLocalStore() {
    return localStorage.getItem('companyFolder');
}

function clearCompanyFolderFromLocalStore() {
    return localStorage.removeItem('companyFolder');
}

function getApiUrl() {
    //return 'http://192.168.1.1:50010/';
    return 'http://localhost:54600/';

}

function getImageUrl() {
    return "/images/" + getCompanyFolderFromLocalStore();
}

function getImageFullPath(imageName) {
    return getImageUrl() + "/" + imageName;
}

function setImage(elementId) {
    let element = document.getElementById(elementId);
    $(element).attr("src", getImageFullPath(elementId + ".jpg"));
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
    //return text;
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



}

function getfolder(e) {

    var files = e.target.files;
    var path = files[0].webkitRelativePath;
    var Folder = path.split("/");
    alert(Folder[0]);
}

function backupDatabase() {

    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/BackupDatabase",
        data: databaseEntity,
        type: "POST",
        success: function () {

            //alert("Done");
            $('#database-Form').modal('hide');
            //swal("نسخ البيانات", "تم انشاء قاعدة بيانات احتياكية بنجاح !", "عملية ناجحة");
            alert("تم انشاء قاعدة بيانات احتياطية بنجاح");

        },
        error: function () {

            alert("حدث خطأ في نسخ قاعدة البيانات ");
        }
    });
}

function fixDataIntegrity() {

    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/FixSalesinvoiceTotal",
        data: databaseEntity,
        type: "POST",
        success: function () {
            $('#database-Form').modal('hide');
            toastr.success("تم تصليح البيانات بنجاح")
        },
        error: function () {
            toastr.error("حدث خطأ")
        }
    });
}

function fixSalesinvoiceTotal() {

    var databaseEntity = fillDatabaseEntity_Static();
    $.ajax({
        url: "/Database/FixSalesinvoiceTotal",
        data: databaseEntity,
        type: "POST",
        success: function () {
            //$('#database-Form').modal('hide');
            //toastr.success("تم تصليح البيانات بنجاح")
        },
        error: function () {
            //toastr.error("حدث خطأ")
        }
    });
}


function updateFarmersBalance() {

    var databaseEntity = fillDatabaseEntity_Static();
    $.ajax({
        url: "/Database/UpdateFarmersBalance",
        data: databaseEntity,
        type: "POST",
        success: function () {
            //$('#database-Form').modal('hide');
            //toastr.success("تم تصليح البيانات بنجاح")
        },
        error: function () {
            //toastr.error("حدث خطأ")
        }
    });
}

function updateSellersBalance() {

    var databaseEntity = fillDatabaseEntity_Static();
    $.ajax({
        url: "/Database/UpdateSellersBalance",
        data: databaseEntity,
        type: "POST",
        success: function () {
            //$('#database-Form').modal('hide');
            //toastr.success("تم تصليح البيانات بنجاح")
        },
        error: function () {
            //toastr.error("حدث خطأ")
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

function fillDatabaseEntity_Static() {
    var databaseEntity = {
        ConnectionString:"Server=.;Database=SalesManagement;Trusted_Connection=True;",
    };
    return databaseEntity;
}

function loadDatabaseEntity() {
    $('#ConnectionString').val("Server =.\\SQLEXPRESS; Database = SalesManagement; Trusted_Connection = True;");
    $('#DatabaseName').val("SalesManagement");
    $("#FilePath").val("D://Backup_Dabase");
}


function getReportHead(title) {
    var html = `<html dir="rtl">
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
<body style="background-color: #ffffff;" onload="window.print()">`;
    return html;
}

function isEmpty(text) {
    if (text == undefined || text == "" || text == null) {
        return true;
    }
    return false;
}

function getReportFooterText() {

    return `<div class="col-lg-12">
                            <table style="width:100%;border:none;font-weight:bold">
                                <tr>
                                    <td style="width:70%;border:none;text-align:center">خـــــــــالص مع الشكــــــــــــر</td>
                                </tr>
                             </table>
                    </div>`;
}

function getReportAuthor() {
    //var html = getReportFooterText();
    return `<div class="col-lg-12 author">
                            <table style="width:100%;border:none;font-weight:bold">
                                <tr>
                                    <td style="width:30%;border:none;">01093162036</td>
                                    <td style="width:70%;border:none;">تم عمل هذا البرنامج بواسطة مهندس/محمود علي</td>
                                </tr>
                             </table>
                    </div>`;
}


var elements = document.getElementsByClassName("arrow-togglable");
//var length = elements.length;
//var currentIndex = 0;

///Prevent Arrows to move
document.onkeydown = function (e) {

    //let targetIndex = e.target.tabIndex;

    //let rowNumber = getRowNumber(e);
    //var index = targetIndex + (parseInt(rowNumber - 1) * 5);
    //switch (e.keyCode) {

    //    case 9:
    //        currentIndex = ((index + 1) == elements.length) ? 0 : ++index;
    //        elements[4].focus();
    //        break;

    ////Right Arrow
    //case 39:
    //    currentIndex = (index == 0) ? elements.length - 1 : --index;
    //    elements[currentIndex].focus();
    //    break;

    ////Left Arrow
    //case 37:
    //    currentIndex = ((index + 1) == elements.length) ? 0 : ++index;
    //    elements[currentIndex].focus();
    //    break;
    // }
};

function getBalanceByAccountId(accountId, accountTypeId) {
    var balance;
    $.ajax({
        url: "/Safes/GetBalanceByAccountId?accountId=" + accountId + "&&accountTypesEnum=" + accountTypeId,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            balance = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return balance;
}

function getRowNumber(e) {
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

function selectRow(tableId, event, index) {
    debugger;
    var table = document.getElementById(tableId);
    var rows = table.getElementsByTagName("tr");
    rows[index].style.backgroundColor = event.target.checked ? "#cedbf3" : "white";
}


function disableButton(id) {
    $('#' + id).prop('disabled', true);
}

function enableButton(id) {
    $('#' + id).prop('disabled', false);
}
