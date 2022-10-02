$(document).ready(function () {
    //$('#TransactionDate').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    //search();//Load All Dashboard Data
    $('#DateFrom').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    $('#DateTo').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
});

function search() {
    
    let dateFrom = $('#DateFrom').val();
    let dateTo = $('#DateTo').val();

    var url = `/Transaction/List?dateFrom=${dateFrom}&&dateTo=${dateTo}`;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            dashboard = JSON.parse(result);
            SetDashboardData(dashboard);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function SetDashboardData(dashboard) {
    $('#TotalPurchase').text(dashboard.TotalPurchase);
    $('#TotalCommission').text(dashboard.TotalCommission);
    $('#TotalGift').text(dashboard.TotalGift);
    $('#TotalDescent').text(dashboard.TotalDescent);
    $('#TotalSalesinvoice').text(dashboard.TotalSalesinvoice);
    $('#TotalQuantity').text(dashboard.TotalQuantity);
    $('#TotalSalesWeight').text(dashboard.TotalSalesWeight);
    $('#TotalPurchaseWeight').text(dashboard.TotalPurchaseWeight);
    $('#TotalClientsAccountStatement').text(dashboard.TotalClientsAccountStatement);
    $('#TotalSellersAccountStatement').text(dashboard.TotalSellersAccountStatement);

}
