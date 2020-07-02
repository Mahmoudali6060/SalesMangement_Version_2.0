
$(document).ready(function () {
    fillFarmersDropDownList(); //to fill farmers select
    $('#OrderDate').val(getLocalDateForInput(new Date().toUTCString()));
    var sellers;
    var farmers;
    let orderHeaderId = $('#Id').val();
    if (orderHeaderId > 0) {
        getById(orderHeaderId);
        $('#btnUpdate').show();
        $('#btnAdd').hide();
    }
    else {
        addOrderDetailsRow(1, 1, 1, 0);
        $('#btnUpdate').hide();
        $('#btnAdd').show();
    }

    $("#select_style_ul li").click(function () {
        debugger;
        $("#Farmers").val($(this).attr("value"));
    });


});

orderDetailsRowNum = 0;
orderDetailsNumList = [];//to fill rowIds list
//>>>CRUD Operations Methods
//Loading the data(entity) based upon entityId
function getById(id) {
    ;
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
    var entity = fillEntity();
    $.ajax({
        url: "/Orders/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            cancel();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Adding new entity
function addFarmer() {
    if (!validateFarmForm()) return false;
    var entity = fillFarmerEntity();
    $.ajax({
        url: "/Farmers/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            $('#formModal').modal('hide');
            entity.id = result;
            addFarmerToDropDownList(entity);
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


function fillFarmerEntity() {
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

//Clearing the textboxes
function clearFarmerData() {
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
    hideFarmerValidationMessage();
}

function hideFarmerValidationMessage() {
    $('#NameIsRequired').hide();
}

//Updating exsited entity by entityId
function update() {
    var entity = fillEntity();
    $.ajax({
        url: "/Orders/Update",
        data: entity,
        type: "POST",
        success: function (result) {
            cancel();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Deleteing entity by Id
//>>>End CRUD Operations Methods


//>>>Helper Methods 
//Binding Order Header
function fillOrderHeaderData(orderHeader) {
    debugger;
    $('#Id').val(orderHeader.Id);
    var orderDate = getLocalDateForInput(orderHeader.OrderDate);
    $('#OrderDate').val(orderDate);
    $('#CreatedDate').val(orderHeader.Created);
    $('#Number').val(orderHeader.Number);
    $('#Farmers').val(orderHeader.FarmerId);
}
//Bindign Order Details in table
function fillOrderDetailsData(orderDetails) {
    for (let item of orderDetails) {
        addOrderDetailsRow(item.Quantity, item.Weight, item.Price, item.SellerId);
    }
}
function addFarmerToDropDownList(farmer) {
    var option = '<option selected="selected" value="' + farmer.Id + '">' + farmer.Name + '</option>';//Add Option to Client DropDownList
    $('#Farmers').append(option);
    //$('#Farmers').selectstyle();///Make a selecte seachable
}



function fillFarmersDropDownList() {
    this.farmers = getAllFarmers();
    var options = ''; //For farmer dropdownList
    //options += '<option>اختر اسم العميل</option>';
    var i = 1;
    $.each(this.farmers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to Client DropDownList
        i++;
    });
    $('#Farmers').html(options);
    //$('#Farmers').selectstyle();///Make a selecte seachable
}
//Adding All Sellers to select in Order Detials 
function fillSellersDropDownList(sellerId) {
    sellers = getAllSellers();
    var options = ''; //For sellers dropdownList
    var i = 1;
    //options += '<option>اختر اسم التاجر</option>';

    $.each(sellers, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to sellers DropDownList
        i++;
    });
    $('#Sellers' + orderDetailsRowNum + '').html(options);
    if (sellerId > 0) $('#Sellers' + orderDetailsRowNum + '').val(sellerId);
    $('#Sellers').selectstyle();

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
    debugger;
    var orderHeader = {
        Id: $('#Id').val(),
        OrderDate: $('#OrderDate').val(),
        Created: $('#CreatedDate').val(),
        Number: $('#Number').val(),
        FarmerId: $('#Farmers').val()
    };
    return orderHeader;
}

function getFarmerById() {
    debugger;
    console.log($('#Farmers').val());
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
            SellingPrice: $('#SellingPrice' + orderDetailsNum.row + '').val(),
            SellerId: $('#Sellers' + orderDetailsNum.row + '').val()
        };
        orderDetails.push(orderDetailsRow);
    }
    return orderDetails;
}
//Adding Row in Order Details
function addOrderDetailsRow(quantity, weight, price, sellerId) {
    orderDetailsRowNum += 1;
    orderDetailsNumList.push({ row: orderDetailsRowNum });
    html = '';
    html += '<tr id="orderDetailsRow' + orderDetailsRowNum + '">';
    html += '<td>' + orderDetailsRowNum + '</td>';
    html += '<td>' + '<input type="number" id="Quantity' + orderDetailsRowNum + '" value="' + quantity + '" >' + '</td>';
    html += '<td>' + '<input type="number" id="Weight' + orderDetailsRowNum + '"  value="' + weight + '" >' + '</td>';
    html += '<td>' + '<input type="number" id="Price' + orderDetailsRowNum + '"  value="' + price + '" >' + '</td>';
    html += '<td>' + '<input type="number" id="SellingPrice' + orderDetailsRowNum + '"  value="' + price + '" >' + '</td>';
    html += '<td>' + setSellersInOrderDetails(); + '</td>';
    html += '<td>' +
        '<i style="color:red;cursor:pointer" class="icon-trash"  onclick="removeOrderDetailsRow(' + orderDetailsRowNum + ')"></i>' +
        '<i style="color:green;cursor:pointer" class="icon-plus-circle"  onclick="addOrderDetailsRow(' + 1 + ',' + 1 + ',' + 1 + ',' + 0 + ')"></i>' +
        '</td>';
    html += '</tr>';
    $('.tbody-order-details').append(html); // Append new row of selected product
    fillSellersDropDownList(sellerId);
}
//Removing Selected row in Order Detials
function removeOrderDetailsRow(rowNum) {
    ;
    orderDetailsNumList = orderDetailsNumList.filter(x => x.row != rowNum);
    $('table#order-details-list tr#orderDetailsRow' + rowNum + '').remove();
}
//Preparing a selection of Seller included in Order Details
function setSellersInOrderDetails() {
    var html = '<select id="Sellers' + orderDetailsRowNum + '">';
    html += '</select>';
    return html;
}
$(".ss_ul li").click(function () {
    alert(this.id); // id of clicked li by directly accessing DOMElement property
    alert($(this).attr('id')); // jQuery's .attr() method, same but more verbose
    alert($(this).html()); // gets innerHTML of clicked li
    alert($(this).text()); // gets text contents of clicked li
});
//Routing to Index Page
function cancel() {
    debugger;
    window.location.href = '/Orders/Index?today=1';
}
//>>>End Helper Methods



