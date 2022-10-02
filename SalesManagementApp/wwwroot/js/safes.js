$(document).ready(function () {
    //getAll();//Load Data in Table when documents is ready
    setAccountTypeList();
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
    $('#Date').val(getLocalDateForInput(new Date().toUTCString()));
    $('#DateFrom').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    $('#DateTo').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    hideAllDropDownLists();

});
var sellers = [];
var farmers = [];
var safeList = [];

var currentPage = 1;
var recordsTotal;

var currentPage_Seller = 1;
var recordsTotal_Seller;

var currentPage_Client = 1;
var recordsTotal_Client;

clientSafeRowNum = 1;
clientSafeNumList = [];//to fill rowIds list

sellerSafeRowNum = 1;
sellerSafeNumList = [];

var sellersSafeList = [];
var clientsSafeList = [];


function setAccountTypeList() {
    this.sellers = getAllSellers();
    this.farmers = getAllFarmers();
}
//////////////////////////////// Drop Down List Handling
function hideAllDropDownLists() {
    $(".ClientId").hide();
    $(".SellerId").hide();
    $(".OtherAccount").hide();
    $(".Borrow").hide();
    $('.Outcoming').hide();
    $('.Incoming').hide();
}

$("#AccountTypeId").change(function () {
    let accountTypeId = $("#AccountTypeId").val();
    setDropDownListByAccountTypeId(accountTypeId);
});

function addSafe_Seller() {
    clearData();
    $("#AccountTypeId").val(2);
    setDropDownListByAccountTypeId(2);
}

function addSafe_Client() {
    clearData();
    $("#AccountTypeId").val(1);
    setDropDownListByAccountTypeId(1);
}

function setDropDownListByAccountTypeId(accountTypeId, accountId) {
    hideAllDropDownLists();
    switch (parseInt(accountTypeId)) {
        case 1:
            fillFarmersDropDownList(accountId);
            $('.Outcoming').show();
            break;
        case 2:
            fillSellersDropDownList(accountId);
            $('.Incoming').show();
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

    let dateFrom = $('#DateFrom').val();
    let dateTo = $('#DateTo').val();

    var safeDto = getPagedSafes(this.currentPage, dateFrom, dateTo, 0);
    var safesList = safeDto.List;
    preparePagination(safeDto);

    if (safesList == []) return;
    var html = '';
    var i = 1;
    $.each(safesList, function (key, item) {
        if (item.Notes == null)
            item.Notes = "";
        html += '<tr>';

        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'safes-table'" + ',event,' + i + ')"></td>';
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

function getAllClientsSellers() {
    getAllSafe_Clients();
    getAllSafe_Sellers();

}

function getAllSafe_Clients() {

    let dateFrom = $('#DateFrom').val();
    let dateTo = $('#DateTo').val();

    var clientsSafeDto = getPagedSafes(this.currentPage_Client, dateFrom, dateTo, 1);
    clientsSafeList = clientsSafeDto.List;
    prepareClientsPagination(clientsSafeDto);
    if (clientsSafeList && clientsSafeList.length > 0) {
        var html_safe_clients = '';
        var i = 1;
        $.each(clientsSafeList, function (key, item) {

            html_safe_clients += '<tr>';
            html_safe_clients += '<td>' + i + '</td>';
            html_safe_clients += '<td>' + item.Outcoming + '</td>';
            html_safe_clients += '<td>' + farmers.find(x => x.Id == item.AccountId)?.Name + '</td>';
            html_safe_clients += '<td>' + getLocalDate(item.Date) + '</td>';
            html_safe_clients += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="delele(' + item.Id + ')"></i>  <i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i></td>';
            html_safe_clients += '</tr>';
            i++;
        });
        $('.tbody_safe_clientsList').html(html_safe_clients);
        clientSafeRowNum = i;

    }
    else {
        $('.tbody_safe_clientsList').html('');
    }
}

function getAllSafe_Sellers() {

    let dateFrom = $('#DateFrom').val();
    let dateTo = $('#DateTo').val();


    var sellersSafeDto = getPagedSafes(this.currentPage_Seller, dateFrom, dateTo, 2);
    sellersSafeList = sellersSafeDto.List;
    prepareSellersPagination(sellersSafeDto);

    if (sellersSafeList && sellersSafeList.length > 0) {
        var html_safe_sellers = '';
        var i = 1;
        $.each(sellersSafeList, function (key, item) {
            if (item.Notes == null)
                item.Notes = "";
            html_safe_sellers += '<tr>';
            html_safe_sellers += '<td>' + i + '</td>';
            html_safe_sellers += '<td>' + item.Incoming + '</td>';
            html_safe_sellers += '<td>' + sellers.find(x => x.Id == item.AccountId)?.Name + '</td>';
            html_safe_sellers += '<td>' + getLocalDate(item.Date) + '</td>';
            html_safe_sellers += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="delele(' + item.Id + ')"></i>  <i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i></td>';
            html_safe_sellers += '</tr>';
            i++;
        });
        $('.tbody_sellersSafeList').html(html_safe_sellers);

    }
    else {
        $('.tbody_sellersSafeList').html('');
    }


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
    disableButton('btnAdd');
    //if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Safes/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            //$('#formModal').modal('hide');
            toastr.success('تم إضافة السجل بنجاح');
            clearBasicData();
            enableButton('btnAdd');

            //clearData();
            //getAll();
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
            toastr.success('تم التعديل بنجاح');
            $('#formModal').modal('hide');
            clearData();
            //getAll();
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

function clearBasicData() {
    $('#Outcoming').val("");
    $('#Incoming').val("");
    $('#Notes').val("");
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
        Notes: $('#Notes').val(),
        IsTransfered: true
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

function getPagedSafes(currentPage, dateFrom, dateTo, accountTypesId) {
    //var keyword = $("#search").val().toLowerCase();
    var url = "/Safes/GetPagedList?currentPage=" + currentPage + "&&dateFrom=" + dateFrom + "&&dateTo=" + dateTo + "&&accountTypesId=" + accountTypesId;
    //if (!isEmpty(keyword))
    //    url = url + "&&keyword=" + keyword;

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
    //let dateFrom = $('#DateFrom').val();
    //let dateTo = $('#DateTo').val();
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

//Client Pagination
function next_Client() {
    currentPage_Client++;
    getAllSafe_Clients();
}


function back_Client() {
    currentPage_Client--;
    if (currentPage_Client <= 0) {
        $("#pageNumber_Client").val(1);
        currentPage_Client = 1;
        return;
    }

    getAllSafe_Clients();
}

function getToPageNumber_Client() {
    currentPage_Client = $("#pageNumber_Client").val();
    if (currentPage_Client > 0)
        this.getAllSafe_Clients();
}


function prepareClientsPagination(safeDto) {
    recordsTotal_Client = safeDto.Total;
    $("#recordsTotal_Client").text(recordsTotal_Client);
    $("#pageNumber_Client").val(this.currentPage_Client);
}


//End Client Pagination

//Seller Pagination
function next_Seller() {
    currentPage_Seller++;
    getAllSafe_Sellers();
}

function back_Seller() {
    currentPage_Seller--;
    if (currentPage_Seller <= 0) {
        $("#pageNumber_Seller").val(1);
        currentPage_Seller = 1;
        return;
    }

    getAllSafe_Sellers();
}

function getToPageNumber_Seller() {
    currentPage_Seller = $("#pageNumber_Seller").val();
    if (currentPage_Seller > 0)
        this.getAllSafe_Sellers();
}


function prepareSellersPagination(safeDto) {
    recordsTotal_Seller = safeDto.Total;
    $("#recordsTotal_Seller").text(recordsTotal_Seller);
    $("#pageNumber_Seller").val(this.currentPage_Seller);
}

//End Seller Pagination

function getToPageNumber() {
    currentPage = $("#pageNumber").val();
    if (currentPage > 0)
        this.getAllClientsSellers();
}

function preparePagination(safeDto) {
    recordsTotal = safeDto.Total;
    $("#recordsTotal").text(recordsTotal);
    $("#pageNumber").val(this.currentPage);
}

//End Pagination Methods

////////////////////////////End Helper Methods

////////////////////////////>>>>>Clients/Sellers Details
//Adding Row in Client Safe 
function addClientSafeRow() {
    clientSafeNumList.push({ row: clientSafeRowNum });
    var html = '';
    html += '<tr id="clientSafeRowNum' + clientSafeRowNum + '">';
    html += '<td>' + clientSafeRowNum + '</td>';
    html += '<td>' + '<input tabIndex="' + clientSafeRowNum + '" class="form-control arrow-togglable" type="number" id="ClientAmount' + clientSafeRowNum + '" >' + '</td>';
    html += '<td>' + '<select  tabIndex="' + clientSafeRowNum + '" class="form-control arrow-togglable"  id="Clients' + clientSafeRowNum + '"> </select>' + '</td>';
    html += '<td>' + '<input tabIndex="' + clientSafeRowNum + '" class="form-control arrow-togglable" type="Date" id="ClientDate' + clientSafeRowNum + '" >' + '</td>';
    html += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="removeClientSafeRow(' + clientSafeRowNum + ')"></i>  </td>';
    html += '</tr>';
    $('.tbody_safe_clientsList').append(html); // Append new row of selected product
    fillClientsDropDownList(clientSafeRowNum);
    $('#ClientDate' + clientSafeRowNum).val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    clientSafeRowNum += 1;
}
//Adding All Clients to drop down list
function fillClientsDropDownList(clientSafeRowNum) {
    clients = farmers;
    var options = ''; //For clients dropdownList
    var i = 1;
    options += '<option>اختر اسم المزارع</option>';
    $.each(clients, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to clients DropDownList
        i++;
    });
    $('#Clients' + clientSafeRowNum + '').html(options);

}

function removeClientSafeRow(rowNum) {
    clientSafeNumList = clientSafeNumList.filter(x => x.row != rowNum);
    $('table#safes-table-client tr#clientSafeRowNum' + rowNum + '').remove();
}

function prepareClientSafeList() {
    var clientsSafeList = [];
    for (let i of clientSafeNumList) {
        let entity = {
            AccountId: $('#Clients'+i.row).val()  ,
            AccountTypeId: 1,
            Date: $('#ClientDate' + i.row).val(),
            HeaderId: 0,
            Id: 0,
            Incoming: 0,
            IsHidden: false,
            IsTransfered: true,
            Notes: null,
            OrderId: 0,
            OtherAccountName: null,
            Outcoming: $('#ClientAmount' + i.row).val()
        }
        clientsSafeList.push(entity);
    }
    return clientsSafeList;
}

function saveClientSafeRange() {
    disableButton('btnSaveClientSafeRange');
    var clientsSafeList=prepareClientSafeList();
    var safeDTO = {};
    safeDTO.List = clientsSafeList
    $.ajax({
        url: "/Safes/SaveRange",
        data: safeDTO,
        type: "POST",

        success: function (result) {
            //$('#formModal').modal('hide');
            toastr.success('تم الحفظ بنجاح');
            enableButton('btnSaveClientSafeRange');
            //clearData();
            //getAll();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


///////Seller Safe
function addSellerSafeRow() {
    sellerSafeNumList.push({ row: sellerSafeRowNum });
    var html = '';
    html += '<tr id="sellerSafeRowNum' + sellerSafeRowNum + '">';
    html += '<td>' + sellerSafeRowNum + '</td>';
    html += '<td>' + '<input tabIndex="' + sellerSafeRowNum + '" class="form-control arrow-togglable" type="number" id="SellerAmount' + sellerSafeRowNum + '" >' + '</td>';
    html += '<td>' + '<select  tabIndex="' + sellerSafeRowNum + '" class="form-control arrow-togglable"  id="Sellers' + sellerSafeRowNum + '"> </select>' + '</td>';
    html += '<td>' + '<input tabIndex="' + sellerSafeRowNum + '" class="form-control arrow-togglable" type="Date" id="SellerDate' + sellerSafeRowNum + '" >' + '</td>';
    html += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="removeSellerSafeRow(' + sellerSafeRowNum + ')"></i>  </td>';
    html += '</tr>';
    $('.tbody_sellersSafeList').append(html); // Append new row of selected product
    fillSellersDropDownListForAddRange(sellerSafeRowNum);
    $('#SellerDate' + sellerSafeRowNum).val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    sellerSafeRowNum += 1;
}
//Adding All Sellers to drop down list
function fillSellersDropDownListForAddRange(sellerSafeRowNum) {
    sellers = sellers;
    var options = ''; //For sellers dropdownList
    var i = 1;
    options += '<option>اختر اسم التاجر</option>';
    $.each(sellers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to sellers DropDownList
        i++;
    });
    $('#Sellers' + sellerSafeRowNum + '').html(options);

}

function removeSellerSafeRow(rowNum) {
    sellerSafeNumList = sellerSafeNumList.filter(x => x.row != rowNum);
    $('table#safes-table-seller tr#sellerSafeRowNum' + rowNum + '').remove();
}

function prepareSellerSafeList() {
    var sellersSafeList = [];
    for (let i of sellerSafeNumList) {
        let entity = {
            AccountId: $('#Sellers' + i.row).val(),
            AccountTypeId: 2,
            Date: $('#SellerDate' + i.row).val(),
            HeaderId: 0,
            Id: 0,
            Incoming: $('#SellerAmount' + i.row).val(),
            IsHidden: false,
            IsTransfered: true,
            Notes: null,
            OrderId: 0,
            OtherAccountName: null,
            Outcoming: 0
        }
        sellersSafeList.push(entity);
    }
    return sellersSafeList;
}

function saveSellerSafeRange() {
    disableButton('btnSaveSellerSafeRange');
    var sellersSafeList = prepareSellerSafeList();
    var safeDTO = {};
    safeDTO.List = sellersSafeList
    $.ajax({
        url: "/Safes/SaveRange",
        data: safeDTO,
        type: "POST",

        success: function (result) {
            //$('#formModal').modal('hide');
            toastr.success('تم الحفظ بنجاح');
            enableButton('btnSaveSellerSafeRange');
            //clearData();
            //getAll();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

////////////////////////////>>>>>End Clients/Sellers Details

