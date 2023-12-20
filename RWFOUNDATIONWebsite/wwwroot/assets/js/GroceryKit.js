
function AddNewRow() {
	var retRow = $('.sampleRow').clone().removeClass('sampleRow').css('display', '');	
    $('#GroceryKitTbl tbody tr:last').before(retRow);
    var rows = parseInt($('#GroceryKitTbl tbody tr').not(".sampleRow").length);  
    $('#familysize').val(1 + rows);
}


function deleteRow(row) {	
    if ($('#GroceryKitTbl tbody tr').length > 2) {
        $(row).parent().parent().remove();
        var size = parseInt($('#familysize').val());
        $('#familysize').val(size - 1);
    }       
}

function AddNewDocumentRow() {
    var retRow = $('.documentrow').clone().removeClass('documentrow').css('display', '');
    $('#Documenttble tbody tr:last').before(retRow);
}

function deleteDocumentRow(row) {
    if ($('#Documenttble tbody tr').length > 2)
        $(row).parent().parent().remove();
}

$(document).on("change", "#grocerykitform .documentview", function () {
    $(this).parent().children(':first-child').css('background-color', '#2ECC4F');
    var files = $(this).prop("files");
    pdffile_url = URL.createObjectURL(files[0]);
    $(this).parents(':eq(3)').find(".pdfbutton").attr('data-id', pdffile_url);
});
$(document).on("change", "#grocerykitform .otherdocumentview", function () {
    $(this).parent().children(':first-child').css('background-color', '#2ECC4F');
    var files = $(this).prop("files");
    pdffile_url = URL.createObjectURL(files[0]);
    $(this).parents(':eq(3)').find(".pdfbutton").attr('data-id', pdffile_url);
});

$(document).on("click", ".pdfbutton", function () {
    var link = $(this).attr('data-id');
    PDFObject.embed(link, "#PDFViewerDiv");
    $('#PDFViewerModal').modal('show');
});

$('#video').on('change', function () {
    const size =
        (this.files[0].size / 1024 / 1024).toFixed(2);
    if (size > 20) {
        $('#video').val('');
        alertify.error("File must be less then 20mb");
    } else {
        var file = this.files[0];
        objectUrl = URL.createObjectURL(file);
        $("#playvideo").prop("src", objectUrl);
        $("#playvideo").removeClass("d-none");       
        $("#videosize").html('<b>' +
            'Ok, File size is: ' + size + " MB" + '</b>');
    }
});
$('#audio').on('change', function () {
    const size =
        (this.files[0].size / 1024 / 1024).toFixed(2);

    if (size > 20) {
        $('#audio').val('');
        alertify.error("File must be less then 20mb");
    } else {
        var file = this.files[0];
        objectUrl = URL.createObjectURL(file);
        $("#playaudio").prop("src", objectUrl);
        $("#playaudio").removeClass("d-none");
        $("#audiosize").html('<b>' +
            'Ok, File size is: ' + size + " MB" + '</b>');
    }
}); 

$("#grocerykitform").validate({
    errorClass: 'errors',
    rules: {
        firstname: {
            required: true
        },
        lastname: {
            required: true
        },
        fatherorhusbandname: {
            required: true
        },
        medicalprob: {
            required: true
        },
        phonenumber1: {
            required: true
        },
        cnic: {
            required: true
        },
        meritalstatus: {
            required: true
        },
        currentstatus: {
            required: true
        },
        gender: {
            required: true
        },
        residencestatus: {
            required: true
        },
        occupation: {
            required: true
        },
        province: {
            required: true
        },
        city: {
            required: true
        },
        photo: {
            required: true
        },
        dateofbirth: {
            required: true
        },
        address: {
            required: true
        }
    },
    messages: {
        firstname: {
            required: "This is required"
        },
        lastname: {
            required: "This is required"
        },
        fatherorhusbandname: {
            required: "This is required"
        },
        medicalprob: {
            required: "This is required"
        },
        phonenumber1: {
            required: "This is required"
        },
        cnic: {
            required: "This is required"
        },
        meritalstatus: {
            required: "This is required"
        },
        currentstatus: {
            required: "This is required"
        },
        gender: {
            required: "This is required"
        },
        residencestatus: {
            required: "This is required"
        },
        occupation: {
            required: "This is required"
        },
        province: {
            required: "This is required"
        },
        city: {
            required: "This is required"
        },
        photo: {
            required: "This is required"
        },
        dateofbirth: {
            required: "This is required"
        },
        address: {
            required: "This is required"
        }        
    },
    highlight: function (element) {
        $(element).parent().addClass('error')
    },
    unhighlight: function (element) {
        $(element).parent().removeClass('error')
    },
    submitHandler: function (form) {
        $("#spinnerload").removeClass('d-none');
        var formData = new FormData();
        var formno = null;
        var firstname = $('#firstname').val();
        var lastname = $('#lastname').val();
        var fatherorhusbandname = $('#fatherorhusbandname').val();
        var address = $('#address').val();
        var medicalprobId = $("#medicalprob option:selected").val();
        var phonenumber1 = $('#phonenumber1').val();
        var phonenumber2 = $('#phonenumber2').val();
        var occupationId = $("#occupation option:selected").val();
        var provinceId = $("#province option:selected").val();
        var cityId = $("#city option:selected").val();
        var meritalstatus = $("#meritalstatus option:selected").text();
        var otherdocumentname = $("#otherdocumentname").val();
        var otherdocument2name = $("#otherdocument2name").val();

        //single photo upload
        var fileUpload = $("#photo").get(0);
        var files = fileUpload.files;
        formData.append('photo', files[0]);

        var fileUpload2 = $("#CNICFRONT").get(0);
        var files2 = fileUpload2.files;
        formData.append('cnicfront', files2[0]);

        var fileUpload3 = $("#CNICBACK").get(0);
        var files3 = fileUpload3.files;
        formData.append('cnicback', files3[0]);

        var fileUpload4 = $("#disabilitycertificate").get(0);
        var files4 = fileUpload4.files;
        formData.append('disabilitycertificate', files4[0]);

        var fileUpload5 = $("#deathcertificate").get(0);
        var files5 = fileUpload5.files;
        formData.append('deathcertificate', files5[0]);

        var fileUpload6 = $("#bform").get(0);
        var files6 = fileUpload6.files;
        formData.append('bform', files6[0]);

        var fileUpload7 = $("#electricitybill").get(0);
        var files7 = fileUpload7.files;
        formData.append('electricitybill', files7[0]);

        var fileUpload8 = $("#ptclbill").get(0);
        var files8 = fileUpload8.files;
        formData.append('ptclbill', files8[0]);

        var fileUpload9 = $("#application").get(0);
        var files9 = fileUpload9.files;
        formData.append('application', files9[0]);

        var fileUpload10 = $("#OtherDocument").get(0);
        var files10 = fileUpload10.files;
        formData.append('OtherDocument', files10[0]);

        var fileUpload11 = $("#OtherDocument2").get(0);
        var files11 = fileUpload11.files;
        formData.append('OtherDocument2', files11[0]);

        //upload video and audio file

        var videofile = $("#video").get(0);
        var files12 = videofile.files;
        formData.append('video', files12[0]);

        var audiofile = $("#audio").get(0);
        var files13 = audiofile.files;
        formData.append('audio', files13[0]);

        //end
        var dateofbirth = $('#dateofbirth').val();
        var oldformnumber = $('#oldformno').val();
        var cnic = $('#cnic').val();
        var currentstatusId = $("#currentstatus option:selected").val();
        var gender = $("#gender option:selected").text();
        var residencestatus = $("#residencestatus option:selected").text();
        var familymembersdetails = getfamilymembersdetails();
        var foodcost = $('#foodcost').val();
        var houserent = $('#houserent').val();
        var schoolcost = $('#schoolcost').val();
        var utilitiescost = $('#utilitiescost').val();
        var medicalcost = $('#medicalcost').val();
        var othercost = $('#othercost').val();
        var totalcost = $('#totalcost').val();
        var familysize = $('#familysize').val();
        var salary = $('#salary').val();
        var donations = $('#donations').val();
        var otherincome = $('#otherincome').val();
        var totalincome = $('#totalincome').val();
        var shortfallcash = $('#shortfallcash').val();
        var remarks = $('#remarks').val();
        var zakatacceptable = $("input[name='custom-radio-3']:checked").val();
        var Grocerykit = {
            FormNo: formno,
            FirstName: firstname,
            LastName: lastname,
            FatherOrHusbandName: fatherorhusbandname,
            CityId: cityId,
            ProvinceId: provinceId,
            Address: address,
            MeritalStatus: meritalstatus,
            MedicalProbId: medicalprobId,
            PhoneNumber1: phonenumber1,
            PhoneNumber2: phonenumber2,
            OccupationId: occupationId,
            CurrentStatusId: currentstatusId,
            ImageUrl: null,
            DateOfBirth: dateofbirth,
            OldFormNo: oldformnumber,
            CNIC: cnic,
            Gender: gender,
            ResidenceStatus: residencestatus,
            FamilyMembers: familymembersdetails,
            FoodCost: foodcost,
            HouseRent: houserent,
            SchoolCost: schoolcost,
            UtilitiesCost: utilitiescost,
            MedicalCost: medicalcost,
            OtherCost: othercost,
            TotalCost: totalcost,
            ZakatAcceptable: zakatacceptable,
            FamilySize: familysize,
            Salary: salary,
            Donations: donations,
            OtherIncome: otherincome,
            TotalIncome: totalincome,
            ShortFallInCash: shortfallcash,           
            Remarks: remarks,
            OtherDocumentName1: otherdocumentname,
            OtherDocumentName2: otherdocument2name
         };
        formData.append('model', JSON.stringify(Grocerykit));
        $.ajax({
            url: "/GroceryKit/GroceryKitData",
            type: 'POST',
            datatype: "json",
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                if (response.isError !== true) {
                    $("#spinnerload").addClass('d-none');
                    alertify.success("Record has been successfully saved with " + '"' + "Form NUMBER : " + response.message + '"');
                    if (response.roleName == "Beneficiary") {
                        $(location).attr('href', '/UserDashboard/SuccessforBeneficiaryAdded');
                    } else {
                        $(location).attr('href', '/GroceryKit/Index');                       
                    }                  
                                     
                }
                else {
                    alertify.error(response.message);
                }
            },
            error: function (response) {
                alertify.error(response.message);
            }
        });
        
    }
});



function getfamilymembersdetails() {   
    var arrayDDetail = [];
    var dDetail;
    $('#GroceryKitTbl tbody tr').not(".sampleRow").each(function () {
        dDetail = {
            Name: $(this).find('td:eq(0) input').val(),
            RelationId: $(this).find('td:eq(1) option:selected').val(),
            Age: $(this).find('td:eq(2) input').val(),
            FamilyMemberStatusId: $(this).find('td:eq(3) option:selected').val()            
        };
        arrayDDetail.push(dDetail);

    });
    return arrayDDetail;
}

$(document).ready(function () {  
    
    var arrayDDetail = [];
    var dDetail;
    $('#grocerkittable tbody tr').each(function () {
        dDetail = {
            Checked: $(this).find('td:eq(0) input:checked').val(),

        };
        if (dDetail.Checked != undefined) {
            arrayDDetail.push(dDetail);
        }
    });
    if (arrayDDetail.length != 0) {
        $("#assignbtn").removeClass('d-none');
    }   
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome =parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome); 
    //grocerykit tbl data
    $.fn.dataTable.moment("DD/MM/YYYY HH:mm:ss");
    $.fn.dataTable.moment("DD/MM/YYYY");

 var table = $("#grocerkittable").DataTable({
        // Design Assets
        stateSave: true,
        autoWidth: true,
        // ServerSide Setups
        processing: true,
        serverSide: true,
        // Paging Setups
        paging: true,
        // Searching Setups
        searching: { regex: true },
        // Ajax Filter
        ajax: {
            url: "/GroceryKit/LoadData",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        // Columns Setups
     columns: [            
            { data: "groceryKitId"},
            {
                mRender: function (data, type, row) {
                    return ` <label class="custom-control custom-checkbox mb-0">
                            <input type="checkbox" value="`+ row.groceryKitId +`" class="custom-control-input">
                            <span class="custom-control-label"></span>
                        </label>`;
                   
                }
            },
            { data: "formNo" },
            { data: "oldFormNo" },
            { data: "firstName" },
            { data: "lastName" },
            { data: "age" },
            { data: "familySize" },
            { data: "status" },
            { data: "residenceStatus" },
            { data: "medicalProb" },
            { data: "zakatAcceptable" },
            {
                mRender: function (data, type, row) {
                    return `<div class="d-flex">
                                    <a href="/GroceryKit/Detail/`+ row.groceryKitId +`" title="Details" class="btn btn-sm btn-light mr-1  border-0"> <i class="feather icon-list text-dark"></i></a>                                    
                                    <a href="/GroceryKit/Update/`+ row.groceryKitId +`" title="Update"   class="btn btn-sm btn-dark mr-1 text bg-warning border-0"> <i class="feather icon-edit-2 text-white"></i></a>
                                    <a onclick="Delete(`+ row.groceryKitId +`)" class="btn btn-sm btn-dark mr-1 bg-danger border-0"><i class="feather icon-trash-2 text-white"></i></a>
                                 </div>`;
                   
                }
                
            }
           
        ],
        // Column Definitions
        columnDefs: [
            { targets: "no-sort", orderable: false },
            { targets: "no-search", searchable: false },
            {
                targets: "trim",
                render: function (data, type, full, meta) {
                    if (type === "display") {
                        data = strtrunc(data, 10);
                    }

                    return data;
                }
            },
            { targets: "date-type", type: "date-eu" }
        ]
 });
    table.column(0).visible(false);
});
function strtrunc(str, num) {
    if (str.length > num) {
        return str.slice(0, num) + "...";
    }
    else {
        return str;
    }
}

$('#checkheaderAssign').on('click', function () {
    var value = $('#checkheaderAssign').is(":checked")
    
    if (value  == true) {
        $('#checkbox').attr("checked", true)
        $(this).closest('table').find('td input:checkbox').attr('checked', true);        
        var arrayDDetail = [];
        var dDetail;
        $('#grocerkittable tbody tr').each(function () {
            dDetail = {
                Checked: $(this).find('td:eq(0) input:checked').val(),

            };
            if (dDetail.Checked != undefined) {
                arrayDDetail.push(dDetail);
            }
        });
        if (arrayDDetail.length != 0) {
            $("#assignbtn").removeClass('d-none');
        }
        if (arrayDDetail.length == 0) {
            $("#assignbtn").addClass('d-none');
        }
    }
    else {
        $('#checkbox').attr("checked", false)
        $(this).closest('table').find('td input:checkbox').attr('checked', false);      
        var arrayDDetail = [];
        var dDetail;
        $('#grocerkittable tbody tr').each(function () {
            dDetail = {
                Checked: $(this).find('td:eq(0) input:checked').val(),

            };
            if (dDetail.Checked != undefined) {
                arrayDDetail.push(dDetail);
            }
        });
        if (arrayDDetail.length != 0) {
            $("#assignbtn").removeClass('d-none');
        }
        if (arrayDDetail.length == 0) {
            $("#assignbtn").addClass('d-none');
        }
    }
    

});

$('#foodcost').on('change', function () {    
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat( $('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#houserent').on('change', function () {
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat($('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#schoolcost').on('change', function () {
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat($('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#utilitiescost').on('change', function () {
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat($('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#medicalcost').on('change', function () {
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat($('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#othercost').on('change', function () {
    var foodcost = parseFloat($('#foodcost').val());
    var houserent = parseFloat($('#houserent').val());
    var schoolcost = parseFloat($('#schoolcost').val());
    var utilitiescost = parseFloat($('#utilitiescost').val());
    var medicalcost = parseFloat($('#medicalcost').val());
    var othercost = parseFloat($('#othercost').val());

    $('#totalcost').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);

});
$('#salary').on('change', function () {
    var salary = parseFloat($('#salary').val());
    var donations = parseFloat($('#donations').val());
    var otherincome = parseFloat($('#otherincome').val());
    $('#totalincome').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);
});
$('#donations').on('change', function () {
    var salary = parseFloat($('#salary').val());
    var donations = parseFloat($('#donations').val());
    var otherincome = parseFloat($('#otherincome').val());
    $('#totalincome').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);
});
$('#otherincome').on('change', function () {
    var salary = parseFloat($('#salary').val());
    var donations = parseFloat($('#donations').val());
    var otherincome = parseFloat($('#otherincome').val());
    $('#totalincome').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcost').val());
    var totalincome = parseFloat($('#totalincome').val());
    $('#shortfallcash').val(totalcost - totalincome);
});

function Delete(id) {
    alertify.confirm('Delete', 'Do you want to Delete this Record?', function () {
        $.ajax({
            url: "/GroceryKit/Delete",
            type: 'POST',
            datatype: "json", 
            data: {Id: id},
            success: function (response) {
                if (response.isError !== true) {
                    alertify.success("Record has been Successfuly Deleted against " + '"' + "Form NUMBER # " + response.message + '"');
                    $(location).attr('href', '/GroceryKit/Index');
                }
                else {
                    alertify.error(response.message);
                }
            },
            error: function (response) {
                alertify.error(response.message);
            }
        });
        
    }, function () {
        alertify.error('Delete Canceled')
    });
    
}
$(document).on('click', '#grocerkittable tbody .custom-control-input', function (e) {
    var arrayDDetail = [];
    var dDetail;
    $('#grocerkittable tbody tr').each(function () {
        dDetail = {
            Checked: $(this).find('td:eq(0) input:checked').val(),

        };
        if (dDetail.Checked != undefined) {
            arrayDDetail.push(dDetail);
        }
    });
    if (arrayDDetail.length != 0) {
        $("#assignbtn").removeClass('d-none');
    }
    if (arrayDDetail.length == 0) {
        $("#assignbtn").addClass('d-none');
    }
});

// Get assign to details of families
function GetAssignFamilies() {
    var detailsids = getdetails();
    $.ajax({
        url: "/GroceryKit/AssignTo",
        type: 'POST',
        datatype: "json",
        data: { assign: detailsids},
        success: function (response) {
            $('#modelAssignKitTitle').text("Assign Beneficiaries");
            $("#modalAssignKitBody").html(response);
            $("#modal_assigngrocerykit").modal('show');
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
}
function getdetails() {
    var arrayDDetail = [];
    var dDetail;   
    var rows = $("#grocerkittable").dataTable().fnGetNodes();
    for (var i = 0; i < rows.length; i++) {       
        dDetail = {
            GroceryKitId: $(rows[i]).find('td:eq(0) input:checked').val(),
        };
        if (dDetail.GroceryKitId != undefined) {
            arrayDDetail.push(dDetail);
        }
    }  

    return arrayDDetail;
}

function AssignGroceryKit() {
    var userid = $("#userid option:selected").val();
    var grocerydetails = grocerykitassigndetails();
    var grocerykitassgin = {
        UserId: userid,
        Assign: grocerydetails
    };
    $.ajax({
        url: "/GroceryKit/AssignToData",
        type: 'POST',
        datatype: "json",
        data: { model: grocerykitassgin },
        success: function (response) {
            if (response.error != true) {
                $("#modal_assigngrocerykit").modal("hide");
                alertify.success("Beneficiaries Assigned Successfuly");

            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });


}

function grocerykitassigndetails() {
    var arrayDDetail = [];
    var dDetail;
    $('#grocerytblAssign tbody tr').each(function () {
        dDetail = {
            GroceryKitId: $(this).find('td:eq(0) input').val(),
        };
        arrayDDetail.push(dDetail);
    });

    return arrayDDetail;
}

$('#cnic').keydown(function () {

    //allow  backspace, tab, ctrl+A, escape, carriage return
    if (event.keyCode == 8 || event.keyCode == 9
        || event.keyCode == 27 || event.keyCode == 13
        || (event.keyCode == 65 && event.ctrlKey === true))
        return;
    if ((event.keyCode < 48 || event.keyCode > 57)) 
        event.preventDefault();

    var length = $(this).val().length;

    if (length == 5 || length == 13)
        $(this).val($(this).val() + '-');

});