$(document).ready(function () {
    getAll();//Load Data in Table when documents is ready
    fillRolesDropDownList();
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
});

//////////////////////////////////////CRUD Operations Methods
//Loading data (list of entity)
function getAll() {
    var usersList = getAllUsers();
    if (usersList == []) return;
    var html = '';
    var i = 1;
    $.each(usersList, function (key, item) {
        html += '<tr>';
        html += '<td>' + i + '</td>';
        html += '<td>' + item.FirstName + '</td>';
        html += '<td>' + item.LastName + '</td>';
        html += '<td>' + item.Username + '</td>';
        html += '<td>' + item.Password + '</td>';
        html += '<td>' + item.Role.Name + '</td>';
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

function getAllUsers() {
    var users = [];
    $.ajax({
        url: "/Users/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            users = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return users;
}
//Loading the data(entity) based upon entityId
function getById(id) {
    hideAllValidationMessage();
    var user = getUserById(id);
    $('#Id').val(user.Id);
    $('#FirstName').val(user.FirstName);
    $('#LastName').val(user.LastName);
    $('#Username').val(user.Username);
    $('#Roles').val(user.RoleId);
    $('#Password').val(user.Password);
    $('#formModal').modal('show');
    $('#btnUpdate').show();
    $('#btnAdd').hide();

    return false;
}

function getUserById(userId) {
    var user;
    $.ajax({
        url: "/Users/GetById/" + userId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            user = JSON.parse(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return user;
}
//Adding new entity
function add() {
    if (!validateForm()) return false;
    var entity = fillEntity();
    $.ajax({
        url: "/Users/Add",
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
        url: "/Users/Update",
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
                url: "/Users/Delete/" + id,
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
//Adding All Roles to select
function fillRolesDropDownList() {
    var roles = getAllRoles();
    var options = ''; //For role dropdownList
    options += '<option>اختر اسم الصلاحية</option>';
    var i = 1;
    $.each(roles, function (key, item) {
        options += '<option value="' + item.Id + '">' + item.Name + '</option>';//Add Option to Role DropDownList
        i++;
    });
    $('#Roles').html(options);
}
//Clearing the textboxes
function clearData() {
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
    hideAllValidationMessage();
}
//Valdidation using jquery
function validateForm() {
    var isValid = true;
    if ($('#Username').val().trim() == "") {
        $('#Username').css('border-color', 'Red');
        $('#UsernameIsRequired').show();
        isValid = false;
    }
    else {
        $('#Username').css('border-color', 'lightgrey');
        $('#UsernameIsRequired').hide();
    }

    return isValid;
}
//Filtering data
function filter() {
    var value = $("#search").val().toLowerCase();
    if (value != "") {
        $("#users-table tbody tr").filter(function () {
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
        FirstName: $('#FirstName').val(),
        LastName: $('#LastName').val(),
        Username: $('#Username').val(),
        RoleId: $('#Roles').val(),
        Password: $('#Password').val()
    };
    return entity;
}
////////////////////////////End Helper Methods 


