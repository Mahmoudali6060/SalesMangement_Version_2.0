$(document).ready(function () {
    getAll();//Load Data in Table when documents is ready
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
});
var farmersList = [];
var currentPage = 1;
var recordsTotal;
//////////////////////////////////////CRUD Operations Methods
//Loading data (list of entity)
function getAll() {
    var farmerDto = getPagedFarmers(this.currentPage);
    farmersList = farmerDto.List;
    preparePagination(farmerDto);
    if (farmersList == []) return;
    var html = '';
    var i = 1;
    $.each(farmersList, function (key, item) {
        if (item.Notes == null)
            item.Notes = "";
        html += '<tr>';
        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'farmers-table'" + ',event,' + i + ')"></td>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Name + '</td>';
        html += '<td>' + item.Balance + '</td>';
        html += '<td>' + item.Address + '</td>';
        html += '<td>' + item.Phone + '</td>';
        html += '<td>' + item.Notes + '</td>';
        //html += '<td>' + getFarmerBalance(item.PurechasesHeader) + '</td>';
        html += '<td>';
        html += '<i style="color:red;cursor:pointer" class="icon-trash"  onclick = "delele(' + item.Id + ')"></i>';
        html += '<i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i>';
        html += '<i style="color:green;cursor:pointer" class="icon-paper" onclick="openAccountStatement(' + item.Id + ')"></i>';
        html += '</td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    //var value = $("#search").val().toLowerCase();
    //if (value != "") {
    //    filter();
    //}
}


function selectAll() {
    var table = document.getElementById("farmers-table");
    table.style.backgroundColor = "#cedbf3";
}
function getPagedFarmers(currentPage) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Farmers/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;

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

function openAccountStatement(farmerId) {
    //window.location.replace("http:");

    var url = "/FarmerAccountStatement/Index?farmerId=" + farmerId;
    window.location.href = url;
}
function getFarmerBalance(purechasesHeaderList) {
    let sum = 0;
    for (let item of purechasesHeaderList) {
        sum += item.Total - (item.Nawlon + item.Commission);
    }
    return sum;
}
//Loading the data(entity) based upon entityId
function getById(id) {
    hideAllValidationMessage();
    var farmer = getFarmerById(id);
    $('#Id').val(farmer.Id);
    $('#Name').val(farmer.Name);
    $('#Address').val(farmer.Address);
    $('#Phone').val(farmer.Phone);
    //$('#Balance').val(farmer.Balance);
    $('#Notes').val(farmer.Notes);
    $('#formModal').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();
    $('#Balance_div').hide();


    return false;
}
//Adding new entity
function add() {

    if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Farmers/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            if (result > 0) {
                $('#formModal').modal('hide');
                clearData();
                getAll();
            }
            else {
                swal("خطأ", "هذا العميل موجود فعليا");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Updating exsited entity by entityId
function update() {
    debugger;
    if (!validateForm()) return false;
    var entity = fillEntity();

    $.ajax({
        url: "/Farmers/Update",
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
                url: "/Farmers/Delete/" + id,
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
        Balance: $('#Balance').val(),
        Notes: $('#Notes').val(),
    };
    return entity;
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

////////////////////////////End Helper Methods


