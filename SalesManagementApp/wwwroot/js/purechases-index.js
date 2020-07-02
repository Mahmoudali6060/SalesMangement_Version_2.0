$(document).ready(function () {
    let isToday = parseInt($("#today").val());
    loadData(isToday);
});
var purechaseHeaders = [];
let headerId;
var total = 0;
//>>>CRUD Operations Methods
//Loading Purechase Header Data
function loadData(isToday) {
    var url = isToday == 1 ? "/Purechases/GetAllDaily" : "/Purechases/List";
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            purechaseHeaders = JSON.parse(result);
            setPurechaseHeader();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Going to Details Page to Edit Purechase
function getById(id) {
    window.location.href = '/Purechases/Details/' + id + '';
}
//Deletig Purechase (Header and Details)
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
                url: "/Purechases/Delete/" + id,
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
//Binding Purechase Header --LoadData() call it
function setPurechaseHeader() {
    var html = '';
    var i = 1;
    $.each(purechaseHeaders, function (key, item) {
        html += '<tr style="cursor:pointer;" onclick="getPurechaseDetails(' + item.Id + ')">';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.PurechasesDate) + '</td>';
        html += '<td>' + getFarmerById(item.FarmerId).Name + '</td>';
        html += '<td>' + '' + '</td>';
        html += '<td><i style="color:blue;cursor:pointer" class="icon-search-plus"  onclick="getPurechaseDetails(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    var value = $("#search").val();
    if (value !== "") {
        filter();
    }
}
//Getting Related PurechaseDetails to show them in modal 
function getPurechaseDetails(id) {
    debugger;
    headerId = id;
    var selectedPurechaseHeader = purechaseHeaders.find(x => x.Id == id);
    var html = '';
    var totalQuantity = 0;
  
    for (var i = 0; i < selectedPurechaseHeader.PurechasesDetialsList.length; i++) {
        let rowNumber = i + 1;
        html += '<tr>';
        html += '<td >' + selectedPurechaseHeader.PurechasesDetialsList[i].Quantity + '</td>';
        html += '<td>' + selectedPurechaseHeader.PurechasesDetialsList[i].Weight + '</td>';
        html += '<td>' + selectedPurechaseHeader.PurechasesDetialsList[i].Price + '</td>';
        html += '<td>' + parseFloat((selectedPurechaseHeader.PurechasesDetialsList[i].Price * selectedPurechaseHeader.PurechasesDetialsList[i].Weight).toFixed(2)) + '</td>';
        html += '</tr>';

        totalQuantity += selectedPurechaseHeader.PurechasesDetialsList[i].Quantity;
        total += parseFloat((selectedPurechaseHeader.PurechasesDetialsList[i].Price * selectedPurechaseHeader.PurechasesDetialsList[i].Weight).toFixed(2));

    }

    $('tbody.purechase-details').html(html);

    preparePurechaseHeader(selectedPurechaseHeader);

    preparePurechaseFooter(totalQuantity, total, selectedPurechaseHeader);
    $('#listModal').modal('show');
    return html;
}
//Prepare Purechase Header to bind it in modal
function preparePurechaseHeader(selectedPurechaseHeader) {
    $('#Number').text(selectedPurechaseHeader.Id);
    $('#FarmerName').text(getFarmerById(selectedPurechaseHeader.FarmerId).Name);
    $('#Date').text(getLocalDate(selectedPurechaseHeader.PurechasesDate));
}
//Prepare Purechase footer to bind it in modal
function preparePurechaseFooter(totalQuantity, total, selectedPurechaseHeader) {
    if (selectedPurechaseHeader.Commission == 0)
        selectedPurechaseHeader.Commission = Math.ceil(total * .09);

    $('#Commission').val(selectedPurechaseHeader.Commission);

    $('#Nawlon').val(selectedPurechaseHeader.Nawlon);

    $('#Descent').val(totalQuantity);
    let gift = Math.ceil(totalQuantity * .5);
    $('#Gift').val(gift);

    $('#Total').text(Math.ceil(total));
    let totalDiscounts = Math.ceil(totalQuantity + selectedPurechaseHeader.Commission + selectedPurechaseHeader.Nawlon +gift);
    $('#TotalDiscounts').text(totalDiscounts);
    $('#TotalAfterDiscount').text(Math.ceil(total) - totalDiscounts);
}
//Filtering
function filter() {
    var value = $("#search").val();
    if (value !== "") {
        $("#purechase-header-table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }
    else {
        loadData();
    }
}
//gettting nawlon value is changed
function updateTotal() {
    updateCommission();
    updateTotalDiscounts();
    updateTotalAfterDiscount();
}

//Update Commission when choose new Commission Percentage
function updateCommission() {
    let commissionPercentage = $("#CommissionPercentage").val();
    $("#Commission").val((commissionPercentage/100 ) * total);
}
//Updateing Total Discount after changing the nawlon 
function updateTotalDiscounts() {
    let commission = $("#Commission").val();
    let descent = $("#Descent").val();
    let nawlon = $("#Nawlon").val();
    let gift = $('#Gift').val();
    let totalDiscounts = 0;
    if (nawlon == "" || nawlon <= 0) {
        totalDiscounts = Math.ceil(parseFloat(commission)) + parseFloat(descent) + parseFloat(gift);
    }
    else if (nawlon > 0) {
        totalDiscounts = parseFloat(commission) + parseFloat(descent) + parseFloat(gift) + parseFloat(nawlon);
    }
    $("#TotalDiscounts").text(Math.ceil(totalDiscounts));
}
//Updating Total After Discount
function updateTotalAfterDiscount() {
    let totalDiscounts = $("#TotalDiscounts").text();
    let total = $("#Total").text();

    let totalAfterDiscount = parseFloat(total) - parseFloat(totalDiscounts);

    $("#TotalAfterDiscount").text(Math.ceil(totalAfterDiscount));
}

function updateInPrinting() {
    let purechasesHeader = preparePurechasesEntity();
    $.ajax({
        url: "/Purechases/UpdateInPrinting",
        data: purechasesHeader,
        type: "POST",

        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function preparePurechasesEntity() {
    var entity = {
        Id: headerId,
        Commission: $('#Commission').val(),
        Nawlon: $('#Nawlon').val(),
    };
    return entity;

}
//print report
function printReport() {
    debugger;
    updateInPrinting();

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