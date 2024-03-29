﻿
$(document).ready(function () {
    fillFarmersDropDownList(); //to fill farmers select
    $('#OrderDate').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker

    let orderHeaderId = parseInt($('#Id').val());
    manageActionButtonsAppearance(orderHeaderId);

    $("#select_style_ul li").click(function () {
        $("#Farmers").val($(this).attr("value"));
    });

});

//////////////////Variables
var sellers;
var farmers;
orderDetailsRowNum = 0;
orderDetailsNumList = [];//to fill rowIds list
let index = 99;
isTransfered = false;
///>>>END Variables

//Setting of Farmer in select 
function fillFarmersDropDownList() {
    this.farmers = getAllFarmers();
    var options = ''; //For farmer dropdownList
    options += '<option>اختر اسم العميل</option>';
    var i = 1;
    $.each(this.farmers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to Client DropDownList
        i++;
    });
    $('#Farmers').html(options);
    //$('#Farmers').selectstyle();///Make a selecte seachable
}
//Show/Hide Add and Update buttons
function manageActionButtonsAppearance(orderHeaderId) {
    if (orderHeaderId > 0) {
        getById(orderHeaderId);
        $('#btnUpdate').show();
        $('#btnAdd').hide();
    }
    else {
        addOrderDetailsRow(null, null, null, null, 0);
        $('#btnUpdate').hide();
        $('#btnAdd').show();
    }
}

//>>>CRUD Operations Methods
//Loading the data(entity) based upon entityId
function getById(id) {
    $.ajax({
        url: "/Orders/GetById/" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var orderHeader = JSON.parse(result);
            fillOrderHeaderData(orderHeader);
            fillOrderDetailsData(orderHeader.OrderDetails);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
//Adding new entity
function add() {
    //if (!validateForm()) return false;
    disableButton('btnAdd');
    var entity = fillEntity();
    $.ajax({
        url: "/Orders/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            enableButton('btnAdd');
            if (result == true)
                cancel();
            else {
                swal("لم يتم الحفظ", "حدث خطأ في الحفظ", "خطأ");
            }
        },
        error: function (errormessage) {
            swal("لم يتم التعديل", "حدث خطأ في التعديل", "خطأ");
        }
    });
}
//Updating exsited entity by entityId
function update() {
    
    disableButton('btnUpdate');
    var entity = fillEntity();
    $.ajax({
        url: "/Orders/Update",
        data: entity,
        type: "POST",
        success: function (result) {
            enableButton('btnUpdate');
            if (result == true)
                cancel();
            else {
                swal("لم يتم التعديل", "حدث خطأ في التعديل", "خطأ");
            }
        },
        error: function (errormessage) {
            swal("لم يتم التعديل", "حدث خطأ في التعديل", "خطأ");
        }
    });
}


//>>>End CRUD Operations Methods

//>>>Helper Methods 
//Binding Order Header
function fillOrderHeaderData(orderHeader) {
    $('#Id').val(orderHeader.Id);
    var orderDate = getLocalDateForInput(orderHeader.OrderDate);
    $('#OrderDate').val(orderDate);
    $('#CreatedDate').val(orderHeader.Created);
    $('#Number').val(orderHeader.Number);
    $('#Farmers').val(orderHeader.FarmerId);
    isTransfered = orderHeader.IsTransfered;
}
//Bindign Order Details in table
function fillOrderDetailsData(orderDetails) {
    for (let item of orderDetails) {
        addOrderDetailsRow(item.Quantity, item.Weight, item.Price, item.SellingPrice, item.SellerId);
    }
}
//Adding Row in Order Details
function addOrderDetailsRow(quantity, weight, price, sellingPrice, sellerId) {
    orderDetailsRowNum += 1;
    orderDetailsNumList.push({ row: orderDetailsRowNum });

    html = '';
    html += '<tr id="orderDetailsRow' + orderDetailsRowNum + '">';
    html += '<td>' + orderDetailsRowNum + '</td>';
    html += '<td>' + '<input tabIndex="' + ++index + '" class="form-control arrow-togglable" type="number" id="Quantity' + orderDetailsRowNum + '" value="' + quantity + '" >' + '</td>';
    html += '<td>' + '<input tabIndex="' + ++index + '" class="form-control arrow-togglable" type="number" id="Weight' + orderDetailsRowNum + '"  value="' + weight + '" >' + '</td>';
    html += '<td>' + '<input tabIndex="' + ++index + '" class="form-control arrow-togglable" type="number" id="Price' + orderDetailsRowNum + '"  value="' + price + '" >' + '</td>';
    html += '<td>' + '<input tabIndex="' + ++index + '" class="form-control arrow-togglable" type="number" id="SellingPrice' + orderDetailsRowNum + '"  value="' + sellingPrice + '" >' + '</td>';
    html += '<td>' + setSellersInOrderDetails(orderDetailsRowNum, index); + '</td>';
    html += '<td>' +
        '<i class="icon-trash"  onclick="removeOrderDetailsRow(' + orderDetailsRowNum + ')"></i>' +
        '</td>';
    html += '</tr>';
    $('.tbody-order-details').append(html); // Append new row of selected product
    fillSellersDropDownList(sellerId);
}

//Adding All Sellers to select in Order Detials
function fillSellersDropDownList(sellerId) {
    sellers = getAllSellers();
    var options = ''; //For sellers dropdownList
    var i = 1;
    options += '<option>اختر اسم التاجر</option>';
    $.each(sellers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to sellers DropDownList
        i++;
    });
    $('#Sellers' + orderDetailsRowNum + '').html(options);
    if (sellerId > 0) $('#Sellers' + orderDetailsRowNum + '').val(sellerId);
    //$('#Sellers').selectstyle();

}
//Filling entity for (Add or Update)
function fillEntity() {
    var order = {
        OrderHeader: getOrderHeader(),
        OrderDetails: getOrderDetails()
    };
    return order;
}
//Preparing Order Header to send it to Back End
function getOrderHeader() {
    var orderHeader = {
        Id: $('#Id').val(),
        OrderDate: $('#OrderDate').val(),
        Created: $('#CreatedDate').val(),
        Number: $('#Number').val(),
        FarmerId: $('#Farmers').val(),
        IsTransfered:isTransfered
    };
    return orderHeader;
}
//Preparing Order Details 
function getOrderDetails() {
    var orderDetails = [];
    for (let orderDetailsNum of orderDetailsNumList) {
        var orderDetailsRow = [];
        orderDetailsRow = {
            Quantity: $('#Quantity' + orderDetailsNum.row + '').val(),
            Weight: $('#Weight' + orderDetailsNum.row + '').val(),
            Price: $('#Price' + orderDetailsNum.row + '').val(),
            SellerId: $('#Sellers' + orderDetailsNum.row + '').val()
        };
        let sellingPrice = $('#SellingPrice' + orderDetailsNum.row + '').val();
        if (isEmpty(sellingPrice)) {
            orderDetailsRow.SellingPrice = orderDetailsRow.Price;
        }
        else {
            orderDetailsRow.SellingPrice = sellingPrice;

        }
        orderDetails.push(orderDetailsRow);
    }
    return orderDetails;
}
//Removing Selected row in Order Detials
function removeOrderDetailsRow(rowNum) {
    ;
    orderDetailsNumList = orderDetailsNumList.filter(x => x.row != rowNum);
    $('table#order-details-list tr#orderDetailsRow' + rowNum + '').remove();
}
//Preparing a selection of Seller included in Order Details
function setSellersInOrderDetails(orderDetailsRowNum, index) {
    var html = '<div class="row"><div class="col-lg-8">';
    html += '<select  tabIndex="' + ++index + '" class="form-control arrow-togglable"  id="Sellers' + orderDetailsRowNum + '">';
    html += '</select></div>';
    html += '<div class="col-lg-4"><button type="button"  class="btn btn-info" data-toggle="modal" data-target="#formSellerModal" onclick="clearSellerData(' + orderDetailsRowNum + ');"> تاجر جديد</button></div>';

    return html;
}
//Routing to Index Page
function cancel() {
    window.location.href = '/Orders/Index?today=1';
}
//>>>End Helper Methods


///>>>Farm Methods
//Adding new farm entity
function addFarmer() {
    if (!validateFarmForm()) return false;
    var entity = fillFarmerEntity();
    let exsitedFarmer = farmers.find(x => x.Name == entity.Name);
    if (exsitedFarmer) {
        toastr.error('هذا العميل موجمود فعليا', 'خطأ !')
        return;
    }

    $.ajax({
        url: "/Farmers/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            if (result > 0) {
                $('#formModal').modal('hide');
                entity.Id = result;
                addFarmerToDropDownList(entity);
            }
            else {
                swal("خطأ", "هذا العميل موجود فعليا");
            }
            //clearData();
            //getAll();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Valdidation using jquery
function validateFarmForm() {
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
//Fill Farm entity for adding and updating
function fillFarmerEntity() {
    var entity = {
        //Id: $('#Id').val(),
        Name: $('#Name').val(),
        //Address: $('#Address').val(),
        //Phone: $('#Phone').val(),
        //Notes: $('#Notes').val(),
    };
    return entity;
}
//Clearing the textboxes
function clearFarmerData() {
    //$('#Id').val("");
    $('#Name').val("");
    //$('#Address').val("");
    //$('#Phone').val("");
    //$('#Notes').val("");
    $('#Name').css('border-color', 'lightgrey');
    //$('#Address').css('border-color', 'lightgrey');
    //$('#Mobile').css('border-color', 'lightgrey');
    //$('#Notes').css('border-color', 'lightgrey');
    hideFarmerValidationMessage();
}
function hideFarmerValidationMessage() {
    $('#NameIsRequired').hide();
}

function addFarmerToDropDownList(farmer) {
    var option = '<option selected="selected" value="' + farmer.Id + '">' + farmer.Name + '</option>';//Add Option to Client DropDownList
    $('#Farmers').append(option);
    //$('#Farmers').selectstyle();///Make a selecte seachable
}

//>>>END Farm Methods


///>>>Seller Methods
//Adding new seller entity
function addSeller() {
    if (!validateSellerForm()) return false;
    var entity = fillSellerEntity();
    let exsitedItem = sellers.find(x => x.Name == entity.Name);
    if (exsitedItem) {
        toastr.error('هذا التاجر موجمود فعليا', 'خطأ !')
        return;
    }
    $.ajax({
        url: "/Sellers/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            if (result > 0) {
                $('#formSellerModal').modal('hide');
                entity.Id = result;
                addSellerToDropDownList(entity);
                //clearData();
                //getAll();
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
//Valdidation using jquery
function validateSellerForm() {
    var isValid = true;
    if ($('#SellerName-input').val().trim() == "") {
        $('#SellerName-input').css('border-color', 'Red');
        $('#NameIsRequired').show();
        isValid = false;
    }
    else {
        $('#SellerName-input').css('border-color', 'lightgrey');
        $('#NameIsRequired').hide();
    }

    return isValid;
}
//Fill Farm entity for adding and updating
function fillSellerEntity() {
    var entity = {
        Id: $('#Id').val(),
        Name: $('#SellerName-input').val()
    };
    return entity;
}
selectedRowNumber = 0;
//Clearing the textboxes
function clearSellerData(rowNumber) {
    selectedRowNumber = rowNumber;
    //$('#Id').val("");
    $('#SellerName-input').val("");
    //$('#Address').val("");
    //$('#Phone').val("");
    //$('#Notes').val("");
    $('#SellerName-input').css('border-color', 'lightgrey');
    //$('#Address').css('border-color', 'lightgrey');
    //$('#Mobile').css('border-color', 'lightgrey');
    //$('#Notes').css('border-color', 'lightgrey');
    hideSellerValidationMessage();
}
function hideSellerValidationMessage() {
    $('#SellerNameIsRequired').hide();
}

function addSellerToDropDownList(seller) {
    var option = '<option selected="selected" value="' + seller.Id + '">' + seller.Name + '</option>';//Add Option to Seller DropDownList
    $('#Sellers' + selectedRowNumber).append(option);
    //$('#Farmers').selectstyle();///Make a selecte seachable
}
//>>>END Farm Methods
