﻿$(document).ready(function () {
    fixSalesinvoiceTotal();
    updateSellersBalance();
    getAll();//Load Data in Table when documents is ready
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
});
var sellersList = [];
var currentPage = 1;
var recordsTotal;
//////////////////////////////////////CRUD Operations Methods
//Loading data (list of entity)
function getAll() {
    var sellerDto = getPagedSellers(this.currentPage);
    sellersList = sellerDto.List;
    preparePagination(sellerDto);
    if (sellersList == []) return;
    var html = '';
    var i = 1;
    $.each(sellersList, function (key, item) {
        if (item.Notes == null)
            item.Notes = "";
        html += '<tr>';
        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'sellers-table'" + ',event,' + i + ')"></td>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Name + '</td>';
        html += '<td>' + item.Balance + '</td>';
        html += '<td>' + item.Address + '</td>';
        html += '<td>' + item.Phone + '</td>';
        html += '<td>' + item.Notes + '</td>';
        html += '<td>';
        html += '<i style="color:red;cursor:pointer" class="icon-trash" onclick="delele(' + item.Id + ')"></i>';
        html += '<i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i>';
        html += '<i style="color:green;cursor:pointer" class="icon-paper" onclick="openAccountStatement(' + item.Id + ')"></i>';
        html += '</td >';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    //var value = $("#search").val().toLowerCase();
    //if (value != "") {
    //    filter();
    //}
}

function getPagedSellers(currentPage) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Sellers/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;

    var sellers = [];
    $.ajax({
        url: url,
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


function openAccountStatement(sellerId) {
    var url = "/SellerAccountStatement/Index?sellerId=" + sellerId;
    window.location.href = url;
}

//Loading the data(entity) based upon entityId
function getById(id) {
    hideAllValidationMessage();
    var seller = getSellerById(id);
    $('#Id').val(seller.Id);
    $('#Name').val(seller.Name);
    $('#Address').val(seller.Address);
    $('#Phone').val(seller.Phone);
    //$('#OppeningBalance').val(seller.OppeningBalance);
    $('#Notes').val(seller.Notes);
    $('#formModal').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();
    $('#Balance_div').hide();

}
//Adding new entity
function add() {
    if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Sellers/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            if (result > 0) {
                $('#formModal').modal('hide');
                clearData();
                getAll();
            }
            else {
                swal("خطأ", "هذا التاجر موجود فعليا");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Updating exsited entity by entityId
function update() {

    if (!validateForm()) return false;
    var entity = fillEntity();
    entity.Id = $('#Id').val(),
        $.ajax({
            url: "/Sellers/Update",
            data: entity,
            type: "POST",
            success: function (result) {
                $('#formModal').modal('hide');
                clearData();
                getAll();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
}
//Deleteing entity by Id
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
                url: "/Sellers/Delete/" + id,
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
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
////////////////////////////////End CRUD Operations Methods

///////////////////////////////Helper Methods 
//Clearing the textboxes
function clearData() {
    $('#Id').val("");
    $('#Name').val("");
    $('#Address').val("");
    $('#Phone').val("");
    $('#Balance').val("");
    $('#Notes').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Balance_div').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#Address').css('border-color', 'lightgrey');
    $('#Mobile').css('border-color', 'lightgrey');
    //$('#OppenningBalance').css('border-color', 'lightgrey');
    $('#Notes').css('border-color', 'lightgrey');
    hideAllValidationMessage();
}
//Valdidation using jquery
function validateForm() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#NameIsRequired').show();
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
        $('#NameIsRequired').hide();
    }

    return isValid;
}
//Filtering data
function filter() {

    //var value = $("#search").val().toLowerCase();
    //if (value != "") {
    //    $("#sellers-table tbody tr").filter(function () {
    //        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    //    });
    //}
    //else {
    this.currentPage = 1;
    getAll();
    //}
}
//Hide validation messages
function hideAllValidationMessage() {
    $('#NameIsRequired').hide();
}
//fill entity to make (Add or Update)
function fillEntity() {
    var entity = {
        Name: $('#Name').val(),
        Address: $('#Address').val(),
        Phone: $('#Phone').val(),
        Balance: $('#Balance').val(),
        Notes: $('#Notes').val(),
    };
    return entity;
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

function preparePagination(sellerDto) {
    recordsTotal = sellerDto.Total;
    $("#recordsTotal").text(recordsTotal);
    $("#pageNumber").val(this.currentPage);
}
//End Pagination Methods

////////////////////////////End Helper Methods


///Sellers Report
function prepareReport() {
    getAllSellersList();
}

function getAllSellersList() {
    allSellersList = [];
    var url = "/Sellers/List";
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            allSellersList = JSON.parse(result);
            printReport(allSellersList);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //return allSafeList;
}

function printReport(allSellersList) {
    var reportHeader = prepareReportHeader();//Client Name 
    var reportContent = prepareReportContent(allSellersList);//Draw content of report 
    //var reportFooter = prepareReportFooter(allSellersList);
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    var reportHead = getReportHead(' كشوفات حسابات  التجار');
    newWin.document.write(reportHead +
        reportHeader + reportContent +
        `</body></html>`);

    newWin.document.close();

    //setTimeout(function () { newWin.close(); }, 300);

}

function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header" style="margin-bottom: 10px;>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;font-size:24px;font-weight:bold;text-align: center;">
                                        كشوفات حسابات التجار
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>`;
    return reportHeader;
}

function prepareReportContent(allSellersList) {
    return `<div class="row" id="report-content">
                        <div class="col-lg-12" style="width:100%;">
                            <table id="purechase-details-table" class="table-report table table-bordered table-hover" style="margin: 50px 0px;">
                                <thead>
                                    <tr>
                                        <th>م</th>
                                        <th>اسم التاجر</th>
                                        <th>الرصيد</th>
                                        <th>ملاحظات  </th>
                                    </tr>
                                </thead>
                                <tbody class="farm-details">`+ getReportContent(allSellersList) + `</tbody>
                            </table>
                        </div>
                    </div>`;
}

function getReportContent(allSellersList) {

    var html = '';
    for (var i = 0; i < allSellersList.length; i++) {
        let rowNumber = i + 1;
        html += '<tr>';
        html += '<td>' + convertToIndiaNumbers(rowNumber) + '</td>';
        html += '<td>' + allSellersList[i].Name + '</td>';
        html += '<td>' + convertToIndiaNumbers(allSellersList[i].Balance) + '</td>';
        html += '<td>' + "" + '</td>';
        html += '</tr>';
    }
    return html;
}

//##End Reoport Methods
