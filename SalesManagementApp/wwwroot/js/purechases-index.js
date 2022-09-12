
$(document).ready(function () {
    getAll();
    setImage('purchase-header');
});
var purechaseHeaders = [];
let headerId;
var total = 0;
var selectedPurechaseHeader;
var currentPage = 1;
var recordsTotal;


//>>>CRUD Operations Methods
//Loading Purechase Header Data
function getAll() {
    let isToday = parseInt($("#today").val());
    var purchaseDto = getPagedPurchases(this.currentPage, isToday);
    preparePagination(purchaseDto);
    purechaseHeaders = purchaseDto.List;
    setPurechaseHeader();
}
//Going to Details Page to Edit Purechase
function getById(id) {
    window.location.href = '/Purechases/Details/' + id + '';
}
function update() {
    var entity = selectedPurechaseHeader;
    $.ajax({
        url: "/Purechases/Update",
        data: entity,
        type: "POST",
        success: function (result) {
            $('#listModal').modal('hide');
            //cancel();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Deletig Purechase (Header and Details)
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
                url: "/Purechases/Delete/" + id,
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
//Binding Purechase Header --LoadData() call it
function setPurechaseHeader() {
    var html = '';
    var i = 1;
    $.each(purechaseHeaders, function (key, item) {
        if (item.IsPrinted == true) {
            html += '<tr class="is-printed" style="cursor:pointer;" onclick="getPurechaseDetails(' + item.Id + ')">';
        }

        else {
            html += '<tr id="purechase-header' + item.Id + '" style="cursor:pointer;" onclick="getPurechaseDetails(' + item.Id + ')">';
        }
        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'purechase-header-table'" + ',event,' + i + ')"></td>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Id + '</td>';
        html += '<td>' + getLocalDate(item.PurechasesDate) + '</td>';
        html += '<td>' + getFarmerById(item.FarmerId).Name + '</td>';
        html += '<td>' + '' + '</td>';
        if (item.IsPrinted == true) html += '<td>' + 'تم طباعة الفاتورة' + '</td>';
        else html += '<td>' + '' + '</td>';

        if (item.IsTransfered == true) html += '<td>' + 'مُخَّمصة' + '</td>';
        else html += '<td>' + ' غير مُخَّمصة' + '</td>';

        html += '<td><i style="color:blue;cursor:pointer" class="icon-search-plus"  onclick="getPurechaseDetails(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);

}
//Getting Related PurechaseDetails to show them in modal 
function getPurechaseDetails(id) {
    headerId = id;
    selectedPurechaseHeader = purechaseHeaders.find(x => x.Id == id);
    var balance = getBalanceByAccountId(selectedPurechaseHeader.FarmerId, 1);

    var html = '';
    var totalQuantity = 0;
    var totalWeight = 0;
    total = 0;
    var subTotal = 0;
    for (var i = 0; i < selectedPurechaseHeader.PurechasesDetialsList.length; i++) {
        let rowNumber = i + 1;
        subTotal = 0;
        subTotal = Math.ceil(parseFloat((selectedPurechaseHeader.PurechasesDetialsList[i].Price * selectedPurechaseHeader.PurechasesDetialsList[i].Weight)));
        html += '<tr>';
        html += '<td >' + selectedPurechaseHeader.PurechasesDetialsList[i].Quantity + '</td>';
        html += '<td>' + selectedPurechaseHeader.PurechasesDetialsList[i].Weight + '</td>';
        html += '<td>' + selectedPurechaseHeader.PurechasesDetialsList[i].Price + '</td>';
        html += '<td>' + subTotal + '</td>';
        html += '</tr>';

        totalQuantity += selectedPurechaseHeader.PurechasesDetialsList[i].Quantity;
        totalWeight += selectedPurechaseHeader.PurechasesDetialsList[i].Weight;

        total += subTotal;
    }

    $('tbody.purechase-details').html(html);

    preparePurechaseHeader(selectedPurechaseHeader);

    preparePurechaseFooter(total, totalQuantity, totalWeight, selectedPurechaseHeader);
    $('#listModal').modal('show');
    return html;
}
//Prepare Purechase Header to bind it in modal
function preparePurechaseHeader(selectedPurechaseHeader) {
    $('#Number').text(selectedPurechaseHeader.Id);
    $('#FarmerName').text(getFarmerById(selectedPurechaseHeader.FarmerId).Name);
    $('#Date').text(getLocalDate(selectedPurechaseHeader.PurechasesDate));
}
//Prepare Purechase footer to bind it in modal
function preparePurechaseFooter(total, totalQuantity, totalWeight, selectedPurechaseHeader) {

    $('#CommissionPercentage').val(selectedPurechaseHeader.CommissionRate);
    $('#Commission').val(Math.ceil(selectedPurechaseHeader.Commission));
    $('#Nawlon').val(selectedPurechaseHeader.Nawlon);
    $('#Expense').val(selectedPurechaseHeader.Expense);
    $('#Descent').val(Math.ceil(selectedPurechaseHeader.Descent));
    $('#Gift').val(Math.ceil(selectedPurechaseHeader.Gift));

    $('#Total').text(Math.ceil(total));
    let totalDiscounts = Math.ceil(parseFloat(selectedPurechaseHeader.Commission)) + Math.ceil(parseFloat(selectedPurechaseHeader.Descent)) + Math.ceil(parseFloat(selectedPurechaseHeader.Expense)) + Math.ceil(parseFloat(selectedPurechaseHeader.Nawlon)) + Math.ceil(parseFloat(selectedPurechaseHeader.Gift));
    $('#TotalDiscounts').text(totalDiscounts);
    let totalAfterDiscount = total - totalDiscounts;
    selectedPurechaseHeader.Total = totalAfterDiscount;
    $('#TotalAfterDiscount').text(Math.ceil(selectedPurechaseHeader.Total));

    $('#TotalQuantity').text(Math.ceil(totalQuantity));
    $('#TotalWeight').text(Math.ceil(totalWeight));


    if (selectedPurechaseHeader.IsTransfered == true) {
        document.getElementById('isTransfered').style.display = 'block';
    } else {
        document.getElementById('isTransfered').style.display = 'none';
    }
}

//gettting nawlon value is changed
function updateTotal() {
    updateCommission();
    updateTotalDiscounts();
    updateTotalAfterDiscount();
}

//Update Commission when choose new Commission Percentage
function updateCommission() {
    let commissionPercentage = $("#CommissionPercentage").val();
    $("#Commission").val(Math.ceil((commissionPercentage / 100) * total));
}
//Updateing Total Discount after changing the nawlon 
function updateTotalDiscounts() {
    let commission = $("#Commission").val();
    let descent = $("#Descent").val();
    let nawlon = $("#Nawlon").val();
    let expense = $("#Expense").val();
    let gift = $('#Gift').val();
    let totalDiscounts = 0;
    if (nawlon == "" || nawlon <= 0) {
        totalDiscounts = Math.ceil(parseFloat(commission)) + Math.ceil(parseFloat(descent)) + Math.ceil(parseFloat(gift));
    }
    else if (nawlon > 0) {
        totalDiscounts = Math.ceil(parseFloat(commission)) + Math.ceil(parseFloat(descent)) + Math.ceil(parseFloat(gift)) + Math.ceil(parseFloat(nawlon));
    }

    if (expense > 0) {
        totalDiscounts += Math.ceil(parseFloat(expense));
    }

    $("#TotalDiscounts").text(totalDiscounts);

}
//Updating Total After Discount
function updateTotalAfterDiscount() {
    let totalDiscounts = $("#TotalDiscounts").text();
    let total = $("#Total").text();

    let totalAfterDiscount = parseFloat(total) - parseFloat(totalDiscounts);

    $("#TotalAfterDiscount").text(Math.ceil(totalAfterDiscount));
    selectedPurechaseHeader.Total = $("#TotalAfterDiscount").text();
    selectedPurechaseHeader.Gift = $("#Gift").val();
    selectedPurechaseHeader.Descent = $("#Descent").val();
    selectedPurechaseHeader.Commission = $("#Commission").val();
    selectedPurechaseHeader.Nawlon = $("#Nawlon").val();
    selectedPurechaseHeader.Expense = $("#Expense").val();
    selectedPurechaseHeader.CommissionRate = $("#CommissionPercentage").val();
}

function updateInPrinting(isTransfered) {
    let purechasesHeader = preparePurechasesEntity(isTransfered);
    $.ajax({
        url: "/Purechases/UpdateInPrinting",
        data: purechasesHeader,
        type: "POST",

        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function preparePurechasesEntity(isTransfered) {
    var entity = {
        Id: headerId,
        CommissionRate: $('#CommissionPercentage').val(),
        Commission: $('#Commission').val(),
        Nawlon: $('#Nawlon').val(),
        Expense: $('#Expense').val(),
        Total: $('#TotalAfterDiscount').text(),
        Gift: $('#Gift').val(),
        Descent: $('#Descent').val(),
        IsTransfered: isTransfered

    };
    return entity;

}

function setIsPrintedClass() {
    var element = document.getElementById('purechase-header' + headerId);
    if (element != null) {
        element.classList.add("is-printed");
        element.cells[5].innerText = 'تم طباعة الفاتورة';
    }
}

function discount() {
    if (selectedPurechaseHeader.IsTransfered == true) {
        toastr.warning('هذه الفاتورة تم تخصيمها من قبل', 'تنبيه !')
    }
    printReport(true)
}

//print report
function printReport(isTransfered) {

    updateInPrinting(isTransfered);
    setIsPrintedClass();
    var reportHeader = prepareReportHeader();
    var reportContent = prepareReportContent();
    var reportFooter = prepareReportFooter();

    var newWin = window.open('', 'Print-Window');

    newWin.document.open();

    var selectedPurechaseHeader = purechaseHeaders.find(x => x.Id == headerId);
    var reportHead = selectedPurechaseHeader.IsPrinted == true ? getReportHead('فاتورة - بدل فاقد') : getReportHead('فاتورة');


    newWin.document.write(reportHead +
        reportHeader + reportContent + reportFooter +
        `</body></html>`);

    newWin.document.close();

    //setTimeout(function () {
    //    
    //    newWin.close();
    //    let isToday = parseFloat($("#today").val());
    //    getAll(isToday);
    //}, 300);

}

function prepareReportHeader() {
    let reportHeader = `<div class="row" id="report-header" style="margin-bottom: 10px;>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:100%;border:none;">
                                        <img src="`+ getImageFullPath("purchase-header.jpg") + `" style="width:100%;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:40%;border:none;">
                                        التاريخ:`+ convertToIndiaNumbers($('#Date').text()) + `
                                    </td>
                                    <td style="width:60%;border:none;">
                                        رقم الفاتورة:`+ convertToIndiaNumbers($('#Number').text()) + `
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:100%;border:none;font-size:24px;font-weight:bold;" colspan="2">
                                        اسم العميل:`+ $('#FarmerName').text() + `
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>`;
    return reportHeader;
}

function prepareReportContent() {
    return `<div class="row" id="report-content">
                        <div class="col-lg-12" style="width:95%;">
                            <table id="purechase-details-table" class="table-report table table-bordered table-hover" style="margin: 50px 0px;">
                                <thead>
                                    <tr>
                                        <th>العدد</th>
                                        <th> الوزن</th>
                                        <th>السعر</th>
                                        <th>الاجمالي  </th>
                                    </tr>
                                </thead>
                                <tbody class="purechase-details">`+ getReportContent(headerId) + `</tbody>
                            </table>
                        </div>
                     
                           <div class="col-lg-2" style=" width: 3%;writing-mode: vertical-rl;float: left;margin-top: -140px;">
    هذه الفاتورة لا تلغي المستندات أو الشيكات او الايصالات     
                           </div>
                        
                    </div>`;
}

function getReportContent(headerId) {
    var selectedPurechaseHeader = purechaseHeaders.find(x => x.Id == headerId);
    var html = '';
    var totalQuantity = 0;
    var totalQuantity = 0;
    var total = 0;
    var subTotal = 0;
    for (var i = 0; i < selectedPurechaseHeader.PurechasesDetialsList.length; i++) {
        let rowNumber = i + 1;
        subTotal = 0;
        let quantity = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Quantity);
        let weight = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Weight);
        let price = convertToIndiaNumbers(selectedPurechaseHeader.PurechasesDetialsList[i].Price);
        subTotal = Math.ceil(selectedPurechaseHeader.PurechasesDetialsList[i].Weight * selectedPurechaseHeader.PurechasesDetialsList[i].Price);
        html += '<tr>';
        html += '<td>' + quantity + '</td>';
        html += '<td>' + weight + '</td>';
        html += '<td>' + price + '</td>';
        html += '<td>' + convertToIndiaNumbers(subTotal) + '</td>';
        html += '</tr>';
        totalQuantity += selectedPurechaseHeader.PurechasesDetialsList[i].Quantity;
        total += subTotal;
    }
    return html;
}

function prepareReportFooter() {
    let reportFooter = `<div class="row" id="report-footer">
                        <div class="col-lg-12">
                            <table style="width:100%;border:none;">
                                <tr>
                                    <td style="width:50%;border:none;">
                                        <table>
                                           
                                            <tr>
                                                <td> اجمالي الوزن</td>
                                                <td>`+ convertToIndiaNumbers($("#TotalWeight").text()) + `</td>
                                            </tr>
                                            <tr>
                                                <td>الاجمالي</td>
                                                <td>`+ convertToIndiaNumbers($("#Total").text()) + `</td>
                                            </tr>
                                            <tr>
                                                <td> اجمالي الخصومات</td>
                                                <td>`+ convertToIndiaNumbers($("#TotalDiscounts").text()) + `</td>
                                            </tr>
                                            <tr style="font-size:24px;font-weight:bold;background-color:gray;color:white">
                                                <td> الصافي</td>
                                                <td>`+ convertToIndiaNumbers($("#TotalAfterDiscount").text()) + `</td>
                                            </tr>
                                        </table>
                                    </td>


                                   

                                    <td style="width:30%;border:none;">
                                        <table>
                                            <tr>
                                                <td>العمولة</td>
                                                <td>`+ convertToIndiaNumbers($("#Commission").val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td> النزول</td>
                                                <td>`+ convertToIndiaNumbers($("#Descent").val()) + `</td>
                                            </tr>
                                           
                                            <tr>
                                                <td>الوهبة </td>
                                                <td>`+ convertToIndiaNumbers($('#Gift').val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td>النولون</td>
                                                <td>`+ convertToIndiaNumbers($("#Nawlon").val()) + `</td>
                                            </tr>
                                            <tr>
                                                <td>مصروف</td>
                                                <td>`+ convertToIndiaNumbers($("#Expense").val()) + `</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>`;
    let footer = getReportFooterText();
    let author = footer + getReportAuthor();
    reportFooter += author;
    return reportFooter;
}

function getPagedPurchases(currentPage, isToday) {
    var keyword = $("#search").val().toLowerCase();
    var url = "/Purechases/GetPagedList?currentPage=" + currentPage;
    if (!isEmpty(keyword))
        url = url + "&&keyword=" + keyword;
    if (isToday == 1) {
        url = url + "&&isToday=" + true;
    }

    var purechaseHeaders = [];
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            purechaseHeaders = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return purechaseHeaders;
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