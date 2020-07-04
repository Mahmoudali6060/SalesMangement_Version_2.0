
$(".validate").keydown(
    function () {

        var element = $(this).val();
        if (element) {
            $(this).closest("div").find("p").hide();
            $(this).css('border-color', '');
        }
        else {
            $(this).closest("div").find("p").show();
            $(this).css('border-color', 'Red');
        }
    });

function getLocalDate(dateUTC) {
    var now = new Date(dateUTC);
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = day + "-" + month + "-" + now.getFullYear();
    return today;
}

function getLocalDateForInput(dateUTC) {
    var now = new Date(dateUTC);
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + month + "-" + day;
    return today;
}

function turnOnTab(elementId) {
    $('#' + elementId + '').on('shown.bs.modal', function () {
        $(document).off('focusin.modal');
    });
}

function getAllFarmers(currentPage) {
    var farmers = [];
    $.ajax({
        url: "/Farmers/List?currentPage=" + currentPage,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            farmers = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return farmers;
}

function getFarmerById(farmerId) {

    var farmer;
    $.ajax({
        url: "/Farmers/GetById/" + farmerId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            farmer = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return farmer;
}

function getAllSellers() {
    var sellers = [];
    $.ajax({
        url: "/Sellers/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            sellers = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

    return sellers;
}

function getSellerById(sellerId) {

    var seller;
    $.ajax({
        url: "/Sellers/GetById/" + sellerId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            seller = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return seller;
}

function getAllRoles() {
    var roles = [];
    $.ajax({
        url: "/Roles/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            roles = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return roles;
}

function getRoleById(roleId) {
    var role;
    $.ajax({
        url: "/Roles/GetById/" + roleId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            role = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return role;
}

function convertToIndiaNumbers(text) {

    //return text.replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
    //    .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');

    //var id = ['٠', '١', ' ٢', '۳', '٤', '٥', '٦', '۷', '۸', '۹'];
    //return text.replace(/[0-9]/g, function (w) {
    //    return id[+w]
    //});
    //text = 3.5;
    text += "";
    let splitted = text.split(".");
    let part1;
    let part2;
    let result = "";
    part1 = splitted[0].replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
        .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');
    result = "" + part1;


    if (splitted[1] != null) {
        part2 = splitted[1].replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '٤')
            .replace(/5/g, '٥').replace(/6/g, '٦').replace(/7/g, '٧').replace(/8/g, '٨').replace(/9/g, '٩');
        result = "" + part2 + ', ' + "" + part1;
    }
    return result;
    //if (text == undefined) return '';
    //var str = $.trim(text.toString());
    //if (str == "") return "";
    //str = str.replace('0','۰');
    //str = str.replace('1','۱');
    //str = str.replace('2','۲');
    //str = str.replace('3','۳');
    //str = str.replace('4','٤');
    //str = str.replace('5','٥');
    //str = str.replace('6','٦');
    //str = str.replace('7','۷');
    //str = str.replace('8','۸');
    //str = str.replace('9','۹');
    //return str;

    //String.prototype.toEnDigit = function () {
    //    return this.replace(/[\u06F0-\u06F9]+/g, function (digit) {
    //        var ret = '';
    //        for (var i = 0, len = digit.length; i < len; i++) {
    //            ret += String.fromCharCode(digit.charCodeAt(i) - 1728);
    //        }

    //        return ret;
    //    });
    //};


}

function getfolder(e) {
    debugger;
    var files = e.target.files;
    var path = files[0].webkitRelativePath;
    var Folder = path.split("/");
    alert(Folder[0]);
}

function backupDatabase() {
    debugger;
    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/BackupDatabase",
        data: databaseEntity,
        type: "POST",
        success: function () {
            debugger;
            //alert("Done");
            $('#database-Form').modal('hide');
            //swal("نسخ البيانات", "تم انشاء قاعدة بيانات احتياكية بنجاح !", "عملية ناجحة");
            alert("تم انشاء قاعدة بيانات احتياطية بنجاح");

        },
        error: function () {
            debugger;
            alert("حدث خطأ في نسخ قاعدة البيانات ");
        }
    });
}

function restoreDatabase() {
    var databaseEntity = fillDatabaseEntity();
    $.ajax({
        url: "/Database/RestoreDatabase",
        data: databaseEntity,
        type: "POST",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function fillDatabaseEntity() {
    var databaseEntity = {
        DatabaseName: $('#DatabaseName').val(),
        FilePath: $('#FilePath').val(),
        ConnectionString: $('#ConnectionString').val(),
    };
    return databaseEntity;
}

function loadDatabaseEntity() {
    debugger;
    $('#ConnectionString').val("Server =.\\SQLEXPRESS; Database = TabarakDbV2; Trusted_Connection = True;");
    $('#DatabaseName').val("TabarakDbV2");
    $("#FilePath").val("E://Backup_Dabase");
}



