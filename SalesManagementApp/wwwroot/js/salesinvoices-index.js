$(document).ready(function () {
    getAll();
    setImage('salesinvoice-header');
});

var salesinvoiceHeaders = [];
var headerId;
var selectedSalesinvoiceHeader;
var totalQuantity = 0;
var totalWight = 0;
var totalByaa = 0;
var totalMashal = 0;
var currentPage = 1;
var recordsTotal;

//>>>CRUD Operations Methods
//Loading Salesinvoice Header Data
function getAll() {
    let isToday = parseInt($("#today").val());
    var salesinvoiceDto = getPagedSalesinvoices(this.currentPage, isToday);
    preparePagination(salesinvoiceDto);
    salesinvoiceHeaders = salesinvoiceDto.List;
    setSalesinvoiceHeader();
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
                                getAll();
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

//function onTotalMashalChange() {
//    totalMashal = parseInt($("#totalMashal").val());
//    updateTotal(null, totalMashal);
//}

//function onTotalByaaChange() {
//    totalByaa = parseInt($("#totalByaa").val());
//    updateTotal(totalByaa, null);
//}

function updateTotal() {
    var subTotal;
    var total = 0;
    var totalByaa = 0;
    var totalMashal = 0;

    var i = 1;
    for (let item of selectedSalesinvoiceHeader.SalesinvoicesDetialsList) {
        subTotal = 0;
        subTotal += Math.ceil(item.Price * item.Weight) + item.Byaa + item.Mashal;
        $("#SubTotal" + i).text(subTotal);
        i++;
        total += subTotal;
        totalByaa += item.Byaa;
        totalMashal += item.Mashal;
    }
    //total += totalByaa + totalMashal;
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
        if (item.IsPrinted == true) {
            html += '<tr class="is-printed" style="cursor:pointer;" >';
        }

        else {
            html += '<tr id="salesinvoice-header' + item.Id + '" style="cursor:pointer;" >';
        }
        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'salesinvoice-header-table'" + ',event,' + i + ')"></td>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.SalesinvoicesDate) + '</td>';
        html += '<td>' + getSellerById(item.SellerId).Name + '</td>';
        html += calculateSalesinvoiceDetails(item);
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
}

function calculateSalesinvoiceDetails(salesinvoiceHeader) {

    var totalQuantity = 0;
    var total = 0;
    for (var i = 0; i < salesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let subTotal = Math.ceil((salesinvoiceHeader.SalesinvoicesDetialsList[i].Price * salesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + salesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + salesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal);
        total += subTotal;
        totalQuantity += salesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
    }
    var html = '';
    html += '<td>' + totalQuantity + '</td>';
    html += '<td>' + total + '</td>';
    return html;

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
        let subTotal = Math.ceil((selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal);
        html += '<td   >' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight + '</td>';
        html += '<td>' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price + '</td>';
        html += '<td>' + '<input class="form-control" type="number" id="Byaa' + rowNumber + '" value="' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + '"  onchange="onSelectedByaaChange(' + rowNumber + ')">' + '</td>';
        html += '<td>' + '<input  class="form-control" type="number" id="Mashal' + rowNumber + '" value="' + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal + '" onchange="onSelectedMashalChange(' + rowNumber + ')">' + '</td>';
        html += '<td style="width: 30%;">' + gatFarmerName(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].FarmerId) + '</td>';
        html += '<td id="SubTotal' + rowNumber + '">' + subTotal + '</td>';
        html += '</tr>';

        total += subTotal;
        totalQuantity += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
        totalWight += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight;
        totalByaa += Math.ceil(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa);
        totalMashal += Math.ceil(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal);
    }
    selectedSalesinvoiceHeader.ByaaTotal = totalByaa;
    selectedSalesinvoiceHeader.MashalTotal = totalMashal;
    selectedSalesinvoiceHeader.Total = total;

    html = prepareSalesinvoiceTotal(html, selectedSalesinvoiceHeader, totalWight, totalQuantity);
    $('tbody.salesinvoice-details').html(html);
    prepareSalesinvoiceHeader(selectedSalesinvoiceHeader);
    $('#listModal').modal('show');
}

function gatFarmerName(farmerId) {
    let farmer = getFarmerById(farmerId);
    if (farmer != null)
        return farmer.Name;
    return "";
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
    html += '<td>' + '<input readonly="readonly" class="form-control" type="number" id="totalMashal" value="' + selectedSalesinvoiceHeader.MashalTotal + '" onchange="onTotalMashalChange()" >' + '</td>';
    html += '<td>' + '' + '</td>';
    html += '<td><span id="total">' + Math.ceil(selectedSalesinvoiceHeader.Total) + '</span></td>';
    html += '</tr>';
    return html;
}

function printReport(id) {
    if (id != undefined)
        headerId = id;

    let selectedSalesinvoiceHeader = salesinvoiceHeaders.find(x => x.Id == headerId);
    updateInPrinting(selectedSalesinvoiceHeader);
    setIsPrintedClass();

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


function updateInPrinting(purechasesHeader) {

    $.ajax({
        url: "/Salesinvoices/UpdateInPrinting",
        data: purechasesHeader,
        type: "POST",

        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function setIsPrintedClass() {
    var element = document.getElementById('salesinvoice-header' + headerId);
    if (element != null) {
        element.classList.add("is-printed");
        element.cells[6].innerText = 'تم طباعة الطشف';
    }
}
function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header">
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;" align="center">
                                        <img src="`+ getImageFullPath("salesinvoice-header.jpg") + `" style="width:100%;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:none;font-size: 20px;">
                                        التاريخ/ `+ convertToIndiaNumbers($("#Date").text()) + `
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:none;font-size: 20px;font-weight: bold;">
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
                        <div class="col-lg-12" style="width:100%;">
                                <table style="border:2px;margin: 10px 0px;font-size: 25px;" id="salesinvoice-details-table" class="table-report table table-bordered table-hover" >
                                <thead>
                                    <tr style="font-weight: bold;">
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
    let author = getReportAuthor();
    reportcontent += author;
    return reportcontent;
}

function getReportContent(selectedSalesinvoiceHeader) {

    var html = '';
    //var totalQuantity = 0;
    //var totalWight = 0;


    var total = 0;
    totalWight = 0;
    totalQuantity = 0;
    for (var i = 0; i < selectedSalesinvoiceHeader.SalesinvoicesDetialsList.length; i++) {
        let rowNumber = i + 1;

        html += '<tr>';
        let subTotal = (selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price * selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Byaa + selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Mashal;
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity) + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight) + '</td>';
        html += '<td>' + convertToIndiaNumbers(selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Price) + '</td>';
        html += '<td>' + convertToIndiaNumbers(Math.ceil(subTotal)) + '</td>';
        html += '</tr>';

        //total += subTotal;
        totalQuantity += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Quantity;
        totalWight += selectedSalesinvoiceHeader.SalesinvoicesDetialsList[i].Weight;
        total += Math.ceil(subTotal);

    }
    selectedSalesinvoiceHeader.Total = total;
    html = prepareSalesinvoiceTotalReport(html, selectedSalesinvoiceHeader.Total, totalWight, totalQuantity);
    return html;
}

function prepareSalesinvoiceTotalReport(html, total, totalWight, totalQuantity) {
    html += '<tr style="background-color: #f7edbd;font-size: 25px;font-weight: bold;">';
    html += '<td>' + convertToIndiaNumbers(totalQuantity) + '</td>';
    html += '<td>' + convertToIndiaNumbers(totalWight) + '</td>';
    html += '<td>' + 'اجمالي الكشــــف' + '</td>';
    html += '<td>' + convertToIndiaNumbers(Math.ceil(total)) + '</td>';
    html += '</tr>';
    return html;

}



function getPagedSalesinvoices(currentPage, isToday) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Salesinvoices/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;
    if (isToday == 1) {
        url = url + "&&isToday=" + true;
    }

    var salesinvoicesHeaders = [];
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            salesinvoicesHeaders = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return salesinvoicesHeaders;
}

//Filtering data
function filter() {
    this.currentPage = 1;
    getAll();
}
///Pagination Methods
function next() {
    currentPage++;
    getAll();
}

function back() {
    currentPage--;
    if (currentPage <= 0) {
        $("#pageNumber").val(1);
        currentPage = 1;
        return;
    }

    getAll();
}

function getToPageNumber() {
    currentPage = $("#pageNumber").val();
    if (currentPage > 0)
        this.getAll();
}

function preparePagination(farmerDto) {
    recordsTotal = farmerDto.Total;
    $("#recordsTotal").text(recordsTotal);
    $("#pageNumber").val(this.currentPage);
}
//End Pagination Methods


//>>>END Helper Methods