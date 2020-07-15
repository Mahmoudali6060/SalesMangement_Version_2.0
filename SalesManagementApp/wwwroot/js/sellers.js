$(document).ready(function () {
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
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Name + '</td>';
        html += '<td>' + item.Address + '</td>';
        html += '<td>' + item.Phone + '</td>';
        //html += '<td>' + item.OppeningBalance + '</td>';
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
    var value = $("#search").val().toLowerCase();
    if (value != "") {
        filter();
    }
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
            $('#formModal').modal('hide');
            clearData();
            getAll();
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

    $.ajax({
        url: "/Sellers/Update",
        data: JSON.stringify(entity),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
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
    //$('#OppenningBalance').val("");
    $('#Notes').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
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
        Id: $('#Id').val(),
        Name: $('#Name').val(),
        Address: $('#Address').val(),
        Phone: $('#Phone').val(),
        //OppeningBalance: $('#OppeningBalance').val(),
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

