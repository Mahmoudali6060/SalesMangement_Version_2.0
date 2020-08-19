$(document).ready(function () {
    getAll();
});

var orderHeaders = [];
var currentPage = 1;
var recordsTotal;

//>>>CRUD Operations Methods
//Loading Order Header Data
function getAll() {
    let isToday = parseInt($("#today").val());
    var orderDto = getPagedOrders(this.currentPage, isToday);
    preparePagination(orderDto);
    orderHeaders = orderDto.List;
    setOrderHeader();
}
//Going to Details Page to Edit Order
function getById(id) {
    window.location.href = '/Orders/Details/' + id + '';
}
//Deletig Order (Header and Details)
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
                url: "/Orders/Delete/" + id,
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

//>>>Helper Methods
//Binding Order Header
function setOrderHeader() {
    var html = '';
    var i = 1;
    $.each(orderHeaders, function (key, item) {
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.OrderDate) + '</td>';
        html += '<td>' + item.Farmer.Name + '</td>';
        html += '<td>' + '' + '</td>';
        html += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="delele(' + item.Id + ')"></i>  <i style="color:green;cursor:pointer" class="icon-pencil2" onclick="getById(' + item.Id + ')"></i>  <i style="color:blue;cursor:pointer" class="icon-search-plus"  onclick="getOrderDetails(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    //var value = $("#search").val();
    //if (value !== "") {
    //    filter();
    //}
}
//Getting Related OrderDetails to show them in modal
function getOrderDetails(id) {
    var selectedOrderHeaders = orderHeaders.find(x => x.Id == id);
    var html = '';
    for (var i = 0; i < selectedOrderHeaders.OrderDetails.length; i++) {
        let rowNumber = i + 1;
        html += '<tr>';
        html += '<td>' + rowNumber + '</td>';
        html += '<td style="width: 30%;">' + selectedOrderHeaders.OrderDetails[i].Quantity + '</td>';
        html += '<td>' + selectedOrderHeaders.OrderDetails[i].Weight + '</td>';
        html += '<td>' + selectedOrderHeaders.OrderDetails[i].Price + '</td>';
        html += '<td>' + getSellerById(selectedOrderHeaders.OrderDetails[i].SellerId).Name + '</td>';
        html += '</tr>';
    }
    $('tbody.order-details').html(html);
    $('h4#listModalLabel').text('تفاصيل البيان رقم : ' + selectedOrderHeaders.Id + ' بتاريخ : ' + getLocalDate(selectedOrderHeaders.OrderDate));
    $('#listModal').modal('show');
}

function getPagedOrders(currentPage, isToday) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Orders/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;
    if (isToday == 1) {
        url = url + "&&isToday=" + true;
    }

    var orderHeaders = [];
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            orderHeaders = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return orderHeaders;
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