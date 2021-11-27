$(document).ready(function () {
    getAll();//Load Data in Table when documents is ready
    fillRolesDropDownList();
    turnOnTab('formModal');//to allow tab in form modal >>>is called From shared.js
});
//Variables
var imageBase64 = null;
var imageUrl = null;

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
        html += '<td><input type="checkbox" name="cb' + i + '" onclick="selectRow(' + "'users-table'" + ',event,' + i + ')"></td>';
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
    imageUrl = user.ImageUrl;
    if (imageUrl != null) {
        setImageBase64("/images/Users/" + imageUrl);
        imageBase64 = user.ImageBase64;
    }
    return false;
}



function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
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
            if (result == true) {
                //$('#formModal').modal('hide');
                //clearData();
                //getAll();
                location.reload();
            }
            else {
                swal("خطأ", "هذا المستخدم موجود بالفعل");
            }
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
            if (result == true) {
                //$('#formModal').modal('hide');
                //clearData();
                //getAll();
                location.reload();
            }
            else {
                swal("خطأ", "هذا المستخدم موجود بالفعل");
            }
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
    $('#FirstName').val("");
    $('#LastName').val("");
    $('#Username').val("");
    $('#Password').val("");
    $('#Roles').val("");
    $('#file-upload').val("");

    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#FirstName').css('border-color', 'lightgrey');
    $('#LastName').css('border-color', 'lightgrey');
    $('#Username').css('border-color', 'lightgrey');
    $('#Password').css('border-color', 'lightgrey');
    setImageBase64('');
    imageBase64 = null;
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
        Password: $('#Password').val(),
        ImageBase64: imageBase64,
        ImageUrl: imageUrl
    };
    return entity;
}

function showPreview(event) {
    if (event.target.files.length > 0) {
        var file = event.target.files[0];
        var src = URL.createObjectURL(file);
        setImageBase64(src);
        generateImageToBase64(file);
    }
}

function setImageBase64(src) {
    var preview = document.getElementById("imageBase64");
    preview.src = src;
}

function generateImageToBase64(file) {
    var reader = new FileReader();
    reader.onloadend = function () {
        imageBase64 = reader.result;
    }
    reader.readAsDataURL(file);
}

////////////////////////////End Helper Methods


