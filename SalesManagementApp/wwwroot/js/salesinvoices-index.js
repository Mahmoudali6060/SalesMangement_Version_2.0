﻿$(document).ready(function () {
    let isToday = parseInt($("#today").val());
    loadData(isToday);
});
var salesinvoiceHeaders = [];
var headerId;
var selectedSalesinvoiceHeader;
var totalQuantity = 0;
var totalWight = 0;
var totalByaa = 0;
var totalMashal = 0;

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
function update() {
    var entity = selectedSalesinvoiceHeader;
    $.ajax({
        url: "/Salesinvoices/Update",
        data: entity,
        type: "POST",
        success: function (result) {
            $('#listModal').modal('hide');
            //cancel();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function onSelectedMashalChange(rowNumber) {
    var index = rowNumber - 1;
    selectedSalesinvoiceHeader.SalesinvoicesDetialsList[index].Mashal = parseInt($("#Mashal" + rowNumber).val());
    updateTotal();
}

function onSelectedByaaChange(rowNumber) {
    var index = rowNumber - 1;
    selectedSalesinvoiceHeader.SalesinvoicesDetialsList[index].Byaa = parseInt($("#Byaa" + rowNumber).val());
    updateTotal();
}

function onTotalMashalChange() {
    totalMashal = parseInt($("#totalMashal").val());
    updateTotal(null, totalMashal);
}

function onTotalByaaChange() {
    totalByaa = parseInt($("#totalByaa").val());
    updateTotal(totalByaa, null);
}

function updateTotal(updatedTotalByaa, updatedTotalMashal) {
    var subTotal;
    var total = 0;
    var totalByaa = 0;
    var totalMashal = 0;

    var i = 1;
    for (let item of selectedSalesinvoiceHeader.SalesinvoicesDetialsList) {
        subTotal = 0;
        subTotal += (item.Price * item.Weight);
        $("#SubTotal" + i).text(Math.ceil(subTotal));
        i++;
        total += subTotal;
        totalByaa = updatedTotalByaa == undefined ? totalByaa += item.Byaa : updatedTotalByaa;
        totalMashal = updatedTotalMashal == undefined ? totalMashal += item.Mashal : updatedTotalMashal;
    }
    total += totalByaa + totalMashal;
    $("#total").text(Math.ceil(total));
    $("#totalByaa").val(Math.ceil(totalByaa));
    $("#totalMashal").val(Math.ceil(totalMashal));

    selectedSalesinvoiceHeader.Total = total;
    selectedSalesinvoiceHeader.ByaaTotal = totalByaa;
    selectedSalesinvoiceHeader.MashalTotal = totalMashal;

}
//>>>Helper Methods 
//Binding Salesinvoice Header --LoadData() call it
function setSalesinvoiceHeader() {
    var html = '';
    var i = 1;
    $.each(salesinvoiceHeaders, function (key, item) {
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.SalesinvoicesDate) + '</td>';
        html += '<td>' + getSellerById(item.SellerId).Name + '</td>';
        html += '<td>' + '' + '</td>';
        html += '<td>';
        html += '<i style = "color:blue;cursor:pointer" class="icon-search-plus"  onclick = "getSalesinvoiceDetails(' + item.Id + ')" ></i >';
        html += '<i style="color:blue;cursor:pointer" class="icon-printer4" onclick="printReport(' + item.Id + ')"></i>';
        html += '</td>';
        html += '<td></td>';
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
    selectedSalesinvoiceHeader = salesinvoiceHeaders.find(x => x.Id == id);
    var html = '';
    totalQuantity = 0;
    totalWight = 0;
    totalByaa = 0;
    totalMashal = 0;
    var total = 0;
    for (var i = 0; i < selectedSalesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let rowNumber = i + 1;
        html += '<tr>';
        let subTotal = (selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal;
        html += '<td style="width: 30%;">' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price + '</td>';
        html += '<td>' + '<input class="form-control" type="number" id="Byaa' + rowNumber + '" value="' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + '"  onchange="onSelectedByaaChange(' + rowNumber + ')">' + '</td>';
        html += '<td>' + '<input readonly="readonly" class="form-control" type="number" id="Mashal' + rowNumber + '" value="' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal + '" onchange="onSelectedMashalChange(' + rowNumber + ')">' + '</td>';
        html += '<td id="SubTotal' + rowNumber + '">' + subTotal + '</td>';
        html += '</tr>';

        total += subTotal;
        totalQuantity += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
        totalWight += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight;
        totalByaa += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa;
        totalMashal += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal;
    }
    html = prepareSalesinvoiceTotal(html, selectedSalesinvoiceHeader, totalWight, totalQuantity );
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
function prepareSalesinvoiceTotal(html, selectedSalesinvoiceHeader, totalWight, totalQuantity) {
    html += '<tr style="background-color: #f7edbd;font-size: 20px;font-weight: bold;">';
    html += '<td>' + totalQuantity + '</td>';
    html += '<td>' + totalWight + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + '<input readonly="readonly" class="form-control" type="number" id="totalByaa" value="' + selectedSalesinvoiceHeader.ByaaTotal + '"  onchange="onTotalByaaChange()" >' + '</td>';
    html += '<td>' + '<input class="form-control" type="number" id="totalMashal" value="' + selectedSalesinvoiceHeader.MashalTotal + '" onchange="onTotalMashalChange()" >' + '</td>';
    html += '<td><span id="total">' + Math.ceil(selectedSalesinvoiceHeader.Total) + '</span></td>';
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

function printReport(id) {
    if (id != undefined)
        headerId = id;

    var selectedSalesinvoiceHeader = salesinvoiceHeaders.find(x => x.Id == headerId);
    prepareSalesinvoiceHeader(selectedSalesinvoiceHeader);
    var reportHeader = prepareReportHeader();
    var reportContent = prepareReportContent(selectedSalesinvoiceHeader);
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    var reportHead = getReportHead('كشف');
    newWin.document.write(reportHead +
        reportHeader + reportContent +
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

function prepareReportContent(selectedSalesinvoiceHeader) {

    let reportcontent = `<div class="row" id="report-content">
                        <div class="col-lg-12">
                                <table id="salesinvoice-details-table" class="table table-bordered table-hover" style="margin: 10px 0px;">
                                <thead>
                                    <tr>
                                        <th>العدد</th>
                                        <th> الكيلو</th>
                                        <th>السعر</th>
                                        <th>الجملة  </th>
                                    </tr>
                                </thead>
                                <tbody class="salesinvoice-details" >`+ getReportContent(selectedSalesinvoiceHeader) + `</tbody>
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

function getReportContent(selectedSalesinvoiceHeader) {

    var html = '';
    var totalQuantity = 0;
    var totalWight = 0;


    var total = 0;
    for (var i = 0; i < selectedSalesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let rowNumber = i + 1;

        html += '<tr>';
        let subTotal = (selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal;
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity) + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price) + '</td>';
        html += '<td>' + convertToIndiaNumbers(subTotal) + '</td>';
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
    html += '<td>' + convertToIndiaNumbers(totalQuantity) + '</td>';
    html += '<td>' + convertToIndiaNumbers(totalWight) + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td>' + convertToIndiaNumbers(Math.ceil(total)) + '</td>';
    html += '</tr>';
    return html;

}
//>>>END Helper Methods