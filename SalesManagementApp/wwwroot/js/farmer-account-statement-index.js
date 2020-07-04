$(document).ready(function () {
    let farmerId = parseInt($("#farmerId").val());
    $("#farmerName").text(getFarmerById(farmerId).Name);
    loadData(farmerId);
});



var safeList = [];
let headerId;
var total = 0;
//>>>CRUD Operations Methods
//Loading Purechase Header Data
function loadData(farmerId) {
    var url = "/FarmerAccountStatement/List?farmerId=" + farmerId;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            safeList = JSON.parse(result);
            setFarmerAccountStatement();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//>>>END CRUD Operations Methods

//>>>Helper Methods 
//Binding Purechase Header --LoadData() call it
function setFarmerAccountStatement() {
    var html = '';
    var i = 1;
    $.each(safeList, function (key, item) {
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + getLocalDate(item.Date) + '</td>';
        html += '<td>' + item.Outcoming + '</td>';
        html += '<td>' + item.Incoming + '</td>';
        html += '<td>' + item.Notes + '</td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    var value = $("#search").val();
    if (value !== "") {
        filter();
    }
}

//Prepare Purechase Header to bind it in modal
function preparePurechaseHeader(selectedPurechaseHeader) {
    $('#Number').text(selectedPurechaseHeader.Id);
    $('#FarmerName').text(getFarmerById(selectedPurechaseHeader.FarmerId).Name);
    $('#Date').text(getLocalDate(selectedPurechaseHeader.PurechasesDate));
}
//Filtering
function filter() {
    var value = $("#search").val();
    if (value !== "") {
        $("#farmer-account-statement-header-table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }
    else {
        loadData();
    }
}


//print report
function printReport() {

    var reportHeader = prepareReportHeader();
    var reportContent = prepareReportContent();
    var reportFooter = prepareReportFooter();

    var newWin = window.open('', 'Print-Window');

    newWin.document.open();

    newWin.document.write(`<html>
<head>
<title>فاتورة</title>
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
<body onload="window.print()">` +
        reportHeader + reportContent + reportFooter +
        `</body></html>`);

    newWin.document.close();

    //setTimeout(function () {
    //    debugger;
    //    newWin.close();
    //    let isToday = parseInt($("#today").val());
    //    loadData(isToday);
    //}, 300);

}

function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header" style="margin-bottom: 10px;>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;">
                                        <img src="/images/a.jpg" style="width:100%;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:40%;border:none;">
                                        التاريخ:`+ convertToIndiaNumbers($('#Date').text()) + `
                                    </td>
                                    <td style="width:60%;border:none;">
                                        رقم الفاتورة:`+ convertToIndiaNumbers($('#Number').text()) + `
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;border:none;font-size:24px;font-weight:bold;" colspan="2">
                                        اسم العميل:`+ $('#FarmerName').text() + `
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>`;
    return reportHeader;
}

function prepareReportContent() {
    return `<div class="row" id="report-content">
                        <div class="col-lg-12">
                            <table id="purechase-details-table" class="table table-bordered table-hover" style="margin: 50px 0px;">
                                <thead>
                                    <tr>
                                        <th>العدد</th>
                                        <th> الوزن</th>
                                        <th>السعر</th>
                                        <th>الاجمالي  </th>
                                    </tr>
                                </thead>
                                <tbody class="purechase-details">`+ getReportContent(headerId) + `</tbody>
                            </table>
                        </div>
                    </div>`;
}

function getReportContent(headerId) {
    var selectedPurechaseHeader = purechaseHeaders.find(x => x.Id == headerId);
    var html = '';
    var totalQuantity = 0;
    var total = 0;
    for (var i = 0; i < selectedPurechaseHeader.PurechasesDetialsList.length; i++) {
        let rowNumber = i + 1;

        let quantity = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Quantity);
        let weight = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Weight);
        let price = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Price);

        html += '<tr>';
        html += '<td>' + quantity + '</td>';
        html += '<td>' + weight + '</td>';
        html += '<td>' + price + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Weight * selectedPurechaseHeader.PurechasesDetialsList[i].Price) + '</td>';
        html += '</tr>';

        totalQuantity += selectedPurechaseHeader.PurechasesDetialsList[i].Quantity;
        total += (selectedPurechaseHeader.PurechasesDetialsList[i].Price * selectedPurechaseHeader.PurechasesDetialsList[i].Weight);

    }
    return html;
}

function prepareReportFooter() {
    let reportFooter = `<div class="row" id="report-footer">
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:70%;border:none;">
                                        <table>
                                            <tr>
                                                <td>الاجمالي</td>
                                                <td>`+ convertToIndiaNumbers($("#Total").text()) + `</td>
                                            </tr>
                                            <tr>
                                                <td> اجمالي الخصومات</td>
                                                <td>`+ convertToIndiaNumbers($("#TotalDiscounts").text()) + `</td>
                                            </tr>
                                            <tr style="font-size:24px;font-weight:bold;">
                                                <td> الصافي</td>
                                                <td>`+ convertToIndiaNumbers($("#TotalAfterDiscount").text()) + `</td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td style="width:30%;border:none;">
                                        <table>
                                            <tr>
                                                <td>العمولة</td>
                                                <td>`+ convertToIndiaNumbers($("#Commission").val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td> النزول</td>
                                                <td>`+ convertToIndiaNumbers($("#Descent").val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td>النولون</td>
                                                <td>`+ convertToIndiaNumbers($("#Nawlon").val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td>الوهبة </td>
                                                <td>`+ convertToIndiaNumbers($('#Gift').val()) + `</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>`;
    let author = ` <div class="col-lg-12">
                            <table style="width:100%;border:none;font-weight:bold">
                                <tr>
                                    <td style="width:30%;border:none;">01093162036</td>
                                    <td style="width:70%;border:none;">Developed By Mahmoud A.Salman</td>
                                </tr>
                             </table>
                    </div>`;
    reportFooter += author;
    return reportFooter;
}

//>>>END Helper Methods 