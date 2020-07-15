$(document).ready(function () {
    getAll();//Load Data in Table when documents is ready
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
    $('#Date').val(getLocalDateForInput(new Date().toUTCString()));
    hideAllDropDownLists();

});
var sellers = [];
var farmers = [];
var safeList = [];
var currentPage = 1;
var recordsTotal;
//////////////////////////////// Drop Down List Handling
function hideAllDropDownLists() {
    $(".ClientId").hide();
    $(".SellerId").hide();
    $(".OtherAccount").hide();
    $(".Borrow").hide();
}

$("#AccountTypeId").change(function () {
    let accountTypeId = $("#AccountTypeId").val();
    setDropDownListByAccountTypeId(accountTypeId);
});

function setDropDownListByAccountTypeId(accountTypeId, accountId) {
    hideAllDropDownLists();
    switch (parseInt(accountTypeId)) {
        case 1:
            fillFarmersDropDownList(accountId);
            break;
        case 2:
            fillSellersDropDownList(accountId);
            break;
        case 3:
            $(".Borrow").show();
            break;
        case 4:
            $(".OtherAccount").show();
            break;
        default:
            break;
    }
}
function fillFarmersDropDownList(id) {
    $(".ClientId").show();
    var options = ''; //For farmer dropdownList
    //options += '<option>اختر اسم العميل</option>';
    var i = 1;
    $.each(this.farmers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to Client DropDownList
        i++;
    });
    $('#ClientId').html(options);
    if (id != undefined) {
        $('#ClientId').val(id);
    }

    //$('#Farmers').selectstyle();///Make a selecte seachable
}

function fillSellersDropDownList(id) {
    $(".SellerId").show();
    var options = ''; //For farmer dropdownList
    var i = 1;
    $.each(this.sellers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to Client DropDownList
        i++;
    });
    $('#SellerId').html(options);
    if (id != undefined) {
        $('#SellerId').val(id);
    }
    //$('#Farmers').selectstyle();///Make a selecte seachable
}

////////////////////////////////END Drop Down List Handling

//////////////////////////////////////CRUD Operations Methods

//Loading data (list of entity)
function getAll() {
    this.sellers = getAllSellers();
    this.farmers = getAllFarmers();
    var safeDto = getPagedSafes(this.currentPage);
    var safesList = safeDto.List;
    preparePagination(safeDto);

    if (safesList == []) return;
    var html = '';
    var i = 1;
    $.each(safesList, function (key, item) {
        if (item.Notes == null)
            item.Notes = "";
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + getLocalDate(item.Date) + '</td>';
        html += '<td>' + item.Outcoming + '</td>';
        html += '<td>' + item.Incoming + '</td>';
        html += '<td>' + setAccountTypeName(item) + '</td>';
        html += '<td>' + setAccountName(item) + '</td>';
        html += '<td>' + item.Notes + '</td>';
        html += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="delele(' + item.Id + ')"></i>  <i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);

}

function setAccountTypeName(item) {
    switch (item.AccountTypeId) {
        case 1:
            return "عملاء";
        case 2:
            return "تجار";
        case 3:
            return "سلف من الغير";
        case 4:
            return "أخرى";
        default:
            break;

    }
}

function setAccountName(item) {
    switch (item.AccountTypeId) {
        case 1:
            return this.farmers.find(x => x.Id == item.AccountId).Name;
        case 2:
            return this.sellers.find(x => x.Id == item.AccountId).Name;
        case 3:
            return item.OtherAccountName;
        case 4:
            return item.OtherAccountName;
        default:
            break;

    }
}
//Get All Safes from Back end
function getAllSafes() {
    var safes = [];
    $.ajax({
        url: "/Safes/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            safes = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return safes;
}

function getSafeBalance(purechasesHeaderList) {
    let sum = 0;
    for (let item of purechasesHeaderList) {
        sum += item.Total - (item.Nawlon + item.Commission);
    }
    return sum;
}
//Loading the data(entity) based upon entityId
function getById(id) {
    hideAllValidationMessage();
    var safe = getSafeById(id);
    $('#AccountTypeId').val(safe.AccountTypeId);
    var date = getLocalDateForInput(safe.Date);
    $('#Date').val(date);
    $('#Id').val(safe.Id);
    $('#Incoming').val(safe.Incoming);
    $('#Outcoming').val(safe.Outcoming);
    $('#OtherAccountName').val(safe.OtherAccountName);
    $('#Notes').val(safe.Notes);
    setDropDownListByAccountTypeId(safe.AccountTypeId, safe.AccountId);

    $('#formModal').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();
    return false;
}

function getSafeById(safeId) {

    var safe;
    $.ajax({
        url: "/Safes/GetById/" + safeId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            safeId = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return safeId;
}

//Adding new entity
function add() {
    //if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Safes/Add",
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
    //if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Safes/Update",
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
                url: "/Safes/Delete/" + id,
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
    $('#AccountTypeId').val("");
    $('#Outcoming').val("");
    $('#Incoming').val("");
    $('#Notes').val("");
    $('#ClientId').val("");
    $('#SellerId').val("");
    $('#Borrow').val("");
    $('#OtherAccountName').val("");
    $('#Date').val(getLocalDateForInput(new Date().toUTCString()));
    hideAllDropDownLists();
    $('#btnUpdate').hide();
    $('#btnAdd').show();
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
        Date: $('#Date').val(),
        AccountTypeId: $('#AccountTypeId').val(),
        Outcoming: $('#Outcoming').val(),
        Incoming: $('#Incoming').val(),
        Notes: $('#Notes').val()
    };
    switch (entity.AccountTypeId) {
        case "1":
            entity.AccountId = $('#ClientId').val();
            break;
        case "2":
            entity.AccountId = $('#SellerId').val();
            break;
        case "3":
            entity.OtherAccountName = $('#Borrow').val();
            break;
        case "4":
            entity.OtherAccountName = $('#OtherAccountName').val();
            break;
        default:
            break;
    }
    return entity;
}

function getPagedSafes(currentPage) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Safes/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;

    var safes = [];
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            safes = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return safes;
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
