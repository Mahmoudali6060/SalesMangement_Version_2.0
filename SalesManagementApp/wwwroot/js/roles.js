$(document).ready(function () {
    getAll();//Load Data in Table when documents is ready
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
});

//////////////////////////////////////CRUD Operations Methods
//Loading data (list of entity)
function getAll() {
    var rolesList = getAllRoles();
    if (rolesList == []) return;
    var html = '';
    var i = 1;
    $.each(rolesList, function (key, item) {
        if (item.Notes == null)
            item.Notes = "";
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.Name + '</td>';
        html += '<td><i  style="color:red;cursor:pointer" class="icon-trash"  onclick="delele(' + item.Id + ')"></i>  <i style="color:green;cursor:pointer" class="icon-pencil2" onclick="return getById(' + item.Id + ')"></i></td>';
        html += '</tr>';
        i++;
    });
    $('.tbody').html(html);
    var value = $("#search").val().toLowerCase();
    if (value != "") {
        filter();
    }
}

//Loading the data(entity) based upon entityId
function getById(id) {
    
    hideAllValidationMessage();
    var role = getRoleById(id);
    $('#Id').val(role.Id);
    $('#Name').val(role.Name);
    $('#formModal').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();
    return false;
}



//Adding new entity
function add() {
    if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Roles/Add",
        data: entity,
        type: "POST",
        success: function (result) {
            $('#formModal').modal('hide');
            clearData();
            getAll();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//Updating exsited entity by entityId
function update() {
    
    if (!validateForm()) return false;
    var entity = fillEntity();

    $.ajax({
        url: "/Roles/Update",
        data: JSON.stringify(entity),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#formModal').modal('hide');
            clearData();
            getAll();
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
                url: "/Roles/Delete/" + id,
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
    $('#Name').val("");
   
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#Name').css('border-color', 'lightgrey');
    $('#Address').css('border-color', 'lightgrey');
    $('#Mobile').css('border-color', 'lightgrey');
    //$('#OppenningBalance').css('border-color', 'lightgrey');
    $('#Notes').css('border-color', 'lightgrey');
    hideAllValidationMessage();
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
//Filtering data
function filter() {
    var value = $("#search").val().toLowerCase();
    if (value != "") {
        $("#roles-table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    }
    else {
        getAll();
    }
}
//Hide validation messages
function hideAllValidationMessage() {
    $('#NameIsRequired').hide();
}
//fill entity to make (Add or Update)
function fillEntity() {
    var entity = {
        Id: $('#Id').val(),
        Name: $('#Name').val()
    };
    return entity;
}
////////////////////////////End Helper Methods 


