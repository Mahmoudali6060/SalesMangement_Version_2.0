$(document).ready(function () {
    //$('#TransactionDate').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    //search();//Load All Dashboard Data
    $('#DateFrom').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
    $('#DateTo').val(getLocalDateForInput(new Date().toUTCString()));//Get Today in Date picker
});

function search() {
    let entity = {
        dateFrom: $('#DateFrom').val(),
        dateTo : $('#DateTo').val()
    }

    var url = `/Transaction/GetDashboardData`;
    $.ajax({
        url: url,
        data: entity,
        type: "POST",
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
