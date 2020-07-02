$(document).ready(function () {
    getDashboardData();//Load All Dashboard Data
});

function getDashboardData() {
    $.ajax({
        url: "/Home/List",
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
    debugger;
    $('#TotalPurchase').text(dashboard.TotalPurchase);
    $('#TotalCommission').text(dashboard.TotalCommission);
    $('#TotalGift').text(dashboard.TotalGift);
    $('#TotalDescent').text(dashboard.TotalDescent);
}
