var table = null;
var arrDepart = [];

$(document).ready(function () {
    table = $('#Division').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/divisions/LoadDiv",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            {
                "data": "id",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                    //return meta.row + 1;
                }
            },
            { "data": "name" },
            {              
                "sortable": false,
                "data": "department.name"
            },
            {
                "data": "createData",
                'render': function (jsonDate) {
                    //var date = new Date(jsonDate).toDateString();
                    //return date;
                    var date = new Date(jsonDate);
                    return moment(date).format('DD MMMM YYYY hh:mm:ss');
                    //return ("0" + date.getDate()).slice(-2) + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
                }
            },
            {
                "data": "updateDate",
                'render': function (jsonDate) {
                    //debugger;
                    //var date = new Date(jsonDate).toDateString();
                    //return date;
                    var date = new Date(jsonDate);
                    if (date.getFullYear() != 0001) {
                        return moment(date).format('DD MMMM YYYY hh:mm:ss');
                        //return ("0" + date.getDate()).slice(-2) + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
                    }
                    return "Not updated yet";
                }
            },
            {
                "sortable": false,
                "render": function (data, type, row) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<button class="btn btn-outline-warning btn-circle" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ><i class="fa fa-lg fa-edit"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-outline-danger btn-circle" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')" ><i class="fa fa-lg fa-times"></i></button>'
                }
            }
        ],
        //dom: 'Bfrtip',
        //buttons: [
        //    'pdf', 'excel'

        //],
        initComplete: function () {
            this.api().columns(2).every(function () {
                var column = this;
                var select = $('<select><option value="">Default</option></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        var val = $.fn.DataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    console.log(d);
                    select.append('<option value = "'+ d +'" >'+ d +'</option >')
                });
            });
        }
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#update').hide();
    $('#add').show();
}

function LoadDepartment(element) {
    debugger;
    if (arrDepart.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Departments/LoadDepart",
            success: function (data) {
                arrDepart = data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(arrDepart, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name));
    });
}
LoadDepartment($('#DepartmentOption'));

//function getDepartmentDropdown() {
//    var departmentSelect = $('#Department');
//    departmentSelect.empty();
//    $.ajax({
//        type: "GET",
//        url: "/Departments/LoadDepart",
//        dataType: "Json",
//        data: "",
//        success: function (results) {
//            if (results != null) {
//                departmentSelect.append($('<option/>', {
//                    value: "",
//                    text: "Choose..."
//                }));
//                $.each(results, function (index, result) {
//                    departmentSelect.append("<option value='" + result.Id + "'>" + result.Name + "</option>");
//                });
//            };
//        },
//        failure: function (response) {
//            alert(response);
//        }
//    });
//};

function GetById(id) {
    //debugger;
    $.ajax({
        url: "/Divisions/GetById/",
        data: { id: id }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Name').val(result.name);
        $('#DepartmentOption').val(result.DepartmentId);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Save() {
    debugger;
    var Div = new Object();
    Div.Id = 0;
    Div.Name = $('#Name').val();
    Div.DepartmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: "/Divisions/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Div
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data inserted Successfully',
                showConfirmButton: false,
                timer: 1500,
            })
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Update() {
    debugger;
    var Div = new Object();
    Div.Id = $('#Id').val();
    Div.Name = $('#Name').val();
    Div.DepartmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: "/Divisions/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Div
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data Updated Successfully',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((resultSwal) => {
        if (resultSwal.value) {
            debugger;
            $.ajax({
                url: "/Divisions/Delete/",
                data: { id: id }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    debugger;
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 1500,
                    });
                    table.ajax.reload(null, false);
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}
