$(document).ready(function () {
    setSellerData();
    getAll(sellerId);
});

var safeList = [];//Display the Safe in tables (for pgination)
var incomingTotal = 0;// اجمالي الواردات
var outcomingTotal = 0;//اجمالي المدفوعات
var currentPage = 1;//For set and get current page
var allSafeList = [];//For displaying all data in report
var sellerId;
//Set Label of seller Data
function setSellerData() {
    sellerId = parseInt($("#sellerId").val());
    $("#sellerName").text(getSellerById(sellerId).Name);
}
//>>>CRUD Operations Methods
//Loading  All Data based on current page
function getAll() {
    var url = `/SellerAccountStatement/GetPagedList?sellerId=${sellerId}&&currentPage=${currentPage}`;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var safeListDto = JSON.parse(result);//Set Data in safeListDto
            safeList = safeListDto.List;//set List of safe
            preparePagination(safeListDto);//prepare pagination labels(Current Page and number of records)
            setSellerAccountStatement();//Draw content of table(safe table body)
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Get All for report
function getAllSafeList() {
    sellerId = parseInt($("#sellerId").val());
    allSafeList = [];
    var url = "/SellerAccountStatement/List?sellerId=" + sellerId;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            allSafeList = JSON.parse(result);
            printReport(allSafeList);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    //return allSafeList;
}
//>>>END CRUD Operations Methods

//>>>Helper Methods 
//Binding Purechase Header --LoadData() call it
function setSellerAccountStatement() {
    var html = '';
    var i = 1;
    incomingTotal = 0;
    outcomingTotal = 0;
    $.each(safeList, function (key, item) {
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + getLocalDate(item.Date) + '</td>';
        html += '<td>' + Math.ceil( item.Outcoming )+ '</td>';
        html += '<td>' + Math.ceil(item.Incoming) + '</td>';
        html += '<td>' + item.Notes + '</td>';
        html += '</tr>';
        i++;
        incomingTotal += Math.ceil(item.Incoming);
        outcomingTotal += Math.ceil(item.Outcoming);
    });

    $("#incomingTotal").text(Math.ceil( incomingTotal));
    $("#outcomingTotal").text(Math.ceil( outcomingTotal));
    $("#balance").text(Math.ceil((Math.abs(incomingTotal - outcomingTotal))));

    if (incomingTotal > outcomingTotal) {
        $("#balance-description").text("جملة ما له");
    }
    else {
        $("#balance-description").text("جملة ما عليه");
    }

    $('.tbody').html(html);
    var value = $("#search").val();
    if (value !== "") {
        filter();
    }
}

//Filtering
function filter() {
    var value = $("#search").val();
    if (value !== "") {
        $("#seller-account-statement-header-table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    }
    else {
        getAll();
    }
}



function printReport(safeList) {
    var reportHeader = prepareReportHeader();//Seller Name 
    var reportContent = prepareReportContent(safeList);//Draw content of report 
    var reportFooter = prepareReportFooter();
    var newWin = window.open('', 'Print-Window');
    newWin.document.open();
    var reportHead = getReportHead(' كشف حساب تاجر');
    newWin.document.write(reportHead +
        reportHeader + reportContent + reportFooter +
        `</body></html>`);

    newWin.document.close();

    //setTimeout(function () { newWin.close(); }, 300);

}

function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header" style="margin-bottom: 10px;>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;font-size:24px;font-weight:bold;text-align: center;">
                                        اسم التاجر:`+ $('#sellerName').text() + `
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>`;
    return reportHeader;
}

function prepareReportContent(safeList) {
    return `<div class="row" id="report-content">
                        <div class="col-lg-12" style="width:100%;">
                            <table id="purechase-details-table" class="table-report table table-bordered table-hover" style="margin: 50px 0px;">
                                <thead>
                                    <tr>
                                        <th>م</th>
                                        <th>التاريخ</th>
                                        <th> المشتريات</th>
                                        <th>المدفوعات</th>
                                        <th>التفاصيل  </th>
                                    </tr>
                                </thead>
                                <tbody class="farm-details">`+ getReportContent(safeList) + `</tbody>
                            </table>
                        </div>
                    </div>`;
}

function getReportContent(safeList) {



    var html = '';
    for (var i = 0; i < safeList.length; i++) {
        let rowNumber = i + 1;
        html += '<tr>';
        html += '<td>' + convertToIndiaNumbers(rowNumber) + '</td>';
        html += '<td>' + convertToIndiaNumbers(getLocalDate(safeList[i].Date)) + '</td>';
        html += '<td>' + convertToIndiaNumbers(safeList[i].Outcoming) + '</td>';
        html += '<td>' + convertToIndiaNumbers(safeList[i].Incoming) + '</td>';
        html += '<td>' + convertToIndiaNumbers(safeList[i].Notes) + '</td>';
        html += '</tr>';
    }
    return html;
}

function prepareReportFooter() {
    var totalIncoming = 0;
    var totalOutcoming = 0;

    for (let item of safeList) {
        totalIncoming += item.Incoming;
        totalOutcoming += item.Outcoming;
    }
    var balance = totalIncoming - totalOutcoming;
    var description;
    if (balance > 0) {
        description = "لـــــــــه";
    }
    else {
        description = "عليه"
    }
    balance = Math.abs(balance);
    let reportFooter = `<div class="row" id="report-footer">
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:70%;border:none;">
                                        <table>
                                            <tr>
                                                <td>اجمالي المشتريات</td>
                                                <td>اجمالي المدفوعات</td>
                                                <td>الرصيد</td>
                                                <td>البيان</td>
                                            </tr>
                                            <tr>
                                                <td>`+ convertToIndiaNumbers(totalOutcoming) + ` </td>
                                                <td>`+ convertToIndiaNumbers(totalIncoming) + `</td>
                                                <td>`+ convertToIndiaNumbers(balance) + `</td>
                                                <td>`+ description + `</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>`;
    let author = getReportAuthor();

    reportFooter += author;
    return reportFooter;
}


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

function preparePagination(safeListDto) {
    recordsTotal = safeListDto.Total;
    $("#recordsTotal").text(recordsTotal);
    $("#pageNumber").val(this.currentPage);
}

//>>>END Helper Methods 