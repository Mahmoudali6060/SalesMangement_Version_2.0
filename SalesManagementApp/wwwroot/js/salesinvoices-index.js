$(document).ready(function () {
    let isToday = parseInt($("#today").val());
    loadData(isToday);
});
var salesinvoiceHeaders = [];
var headerId;
//>>>CRUD Operations Methods
//Loading Salesinvoice Header Data
function loadData(isToday) {
    var url = isToday == 1 ? "/Salesinvoices/GetAllDaily" : "/Salesinvoices/List";
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            salesinvoiceHeaders = JSON.parse(result);
            setSalesinvoiceHeader();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Going to Details Page to Edit Salesinvoice
function getById(id) {
    window.location.href = '/Salesinvoices/Details/' + id + '';
}
//Deletig Salesinvoice (Header and Details)
function delele(id) {
    swal({
        title: "هل انت متأكد ؟",
        text: "هل انت متأكد من حذف هذا السجل ؟",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonText: "نعم ! و ,احذف",
        cancelButtonText: "إالغاء",
        confirmButtonColor: "#ec6c62"
    },
        function () {
            $.ajax({
                url: "/Salesinvoices/Delete/" + id,
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                dataType: "json"
            })
                .done(function (data) {
                    sweetAlert
                        ({
                            title: "تم الحذف!",
                            text: "تم حذف السجل بنجاح !",
                            type: "success"
                        },
                        function () {
                            loadData();
                        });
                })
                .error(function (data) {
                    swal("لم يتم الحذف", "حدث خطأ في الحذف", "خطأ");
                });
        });
}
//>>>END CRUD Operations Methods

//>>>Helper Methods 
//Binding Salesinvoice Header --LoadData() call it
function setSalesinvoiceHeader() {
    var html = '';
    var i = 1;
    $.each(salesinvoiceHeaders, function (key, item) {
        html += '<tr style="cursor:pointer;" onclick="getSalesinvoiceDetails(' + item.Id + ')">';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.SalesinvoicesDate) + '</td>';
        html += '<td>' + getSellerById(item.SellerId).Name + '</td>';
        html += '<td>' + '' + '</td>';
        html += '<td><i style="color:blue;cursor:pointer" class="icon-search-plus"  onclick="getSalesinvoiceDetails(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    var value = $("#search").val();
    if (value !== "") {
        filter();
    }
}
//Getting Related SalesinvoiceDetails to show them in modal 
function getSalesinvoiceDetails(id) {
    
    headerId = id;
    var selectedSalesinvoiceHeader = salesinvoiceHeaders.find(x => x.Id == id);
    var html = '';
    var totalQuantity = 0;
    var totalWight = 0;
    var total = 0;
    for (var i = 0; i < selectedSalesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let rowNumber = i + 1;

        html += '<tr>';
        let subTotal = (selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + (6 * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity);
        html += '<td>' + subTotal + '</td>';
        html += '<td style="width: 30%;">' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price + '</td>';
        html += '</tr>';

        total += subTotal;
        totalQuantity += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
        totalWight += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight;
    }
    html = prepareSalesinvoiceTotal(html, total, totalWight, totalQuantity);
    $('tbody.salesinvoice-details').html(html);
    prepareSalesinvoiceHeader(selectedSalesinvoiceHeader);
    $('#listModal').modal('show');
}
//Prepare Salesinvoice Header to bind it in modal
function prepareSalesinvoiceHeader(selectedSalesinvoiceHeader) {
    
    //$('#Number').text(selectedSalesinvoiceHeader.Id);
    $('#SellerName').text(getSellerById(selectedSalesinvoiceHeader.SellerId).Name);
    //$('#SellerName').val(getSellerById(selectedSalesinvoiceHeader.SellerId).Name);
    $('#Date').text(getLocalDate(selectedSalesinvoiceHeader.SalesinvoicesDate));
}
//Prepare Salesinvoice footer to bind it in modal
function prepareSalesinvoiceTotal(html, total, totalWight, totalQuantity) {
    html += '<tr style="background-color: #f7edbd;font-size: 20px;font-weight: bold;">';
    html += '<td>' +  Math.ceil(total) + '</td>';
    html += '<td>' + totalQuantity + '</td>';
    html += '<td>' +  totalWight + '</td>';
    html += '<td>' + 'اجمالي الكشف' + '</td>';
    html += '</tr>';
    return html;
}
//Filtering
function filter() {
    var value = $("#search").val();
    if (value !== "") {
        $("#salesinvoice-header-table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }
    else {
        loadData();
    }
}

function printReport() {
    debugger;
    var reportHeader = prepareReportHeader();
    var reportContent = prepareReportContent();

    var newWin = window.open('', 'Print-Window');

    newWin.document.open();

    newWin.document.write(`<html>
<head>
<title>كشـــف</title>
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
        reportHeader+   reportContent +
        `</body></html>`);

    newWin.document.close();

    //setTimeout(function () { newWin.close(); }, 300);

}

function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header">
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;" align="center">
                                        <img src="/images/salesinvoice-Header.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:none;">
                                        التاريخ/ `+ convertToIndiaNumbers($("#Date").text()) + `
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:none;">
                                        المطلوب من السيد/ `+ $("#SellerName").text() + `
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>`;
    return reportHeader;
}

function prepareReportContent() {
    
    let reportcontent= `<div class="row" id="report-content">
                        <div class="col-lg-12">
                                <table id="salesinvoice-details-table" class="table table-bordered table-hover" style="margin: 10px 0px;">
                                <thead>
                                    <tr>
                                        <th>الجملة  </th>
                                        <th>العدد</th>
                                        <th> الكيلو</th>
                                        <th>السعر</th>
                                    </tr>
                                </thead>
                                <tbody class="salesinvoice-details" >`+ getReportContent(headerId) +`</tbody>
                            </table>
                        </div>
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
    reportcontent += author;
    return reportcontent;
}

function getReportContent(headerId) {
    
    var selectedSalesinvoiceHeader = salesinvoiceHeaders.find(x => x.Id == headerId);
    var html = '';
    var totalQuantity = 0;
    var totalQuantity = 0;
    var totalWight = 0;
    var total = 0;
    for (var i = 0; i < selectedSalesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let rowNumber = i + 1;

        html += '<tr>';
        let subTotal = (selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + (6 * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity);
        html += '<td>' + convertToIndiaNumbers(subTotal) + '</td>';
        html += '<td>' + convertToIndiaNumbers( selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity) + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + '</td>';
        html += '<td>' + convertToIndiaNumbers( selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price) + '</td>';
        html += '</tr>';

        total += subTotal;
        totalQuantity += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
        totalWight += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight;
    }
    html = prepareSalesinvoiceTotalReport(html, total, totalWight, totalQuantity);
    return html;
}

function prepareSalesinvoiceTotalReport(html, total, totalWight, totalQuantity) {
    html += '<tr style="background-color: #f7edbd;font-size: 20px;font-weight: bold;">';
    html += '<td>' +convertToIndiaNumbers( Math.ceil(total)) + '</td>';
    html += '<td>' +convertToIndiaNumbers( totalQuantity )+ '</td>';
    html += '<td>' + convertToIndiaNumbers(totalWight) + '</td>';
    html += '<td>' + 'اجمالي الكشف' + '</td>';
    html += '</tr>';
    return html;

}
//>>>END Helper Methods 