function AddNewRowupdate() {
    var retRow = $('.sampleRowupdate').clone().removeClass('sampleRowupdate').css('display', '');
    $('#GroceryKitTblupdate tbody tr:last').before(retRow);
    var rows = parseInt($('#GroceryKitTblupdate tbody tr').not(".sampleRowupdate").length);
    $('#familysizeupdate').val(1 + rows);
}


function deleteRowupdate(row) {
    if ($('#GroceryKitTblupdate tbody tr').length > 2)
        $(row).parent().parent().remove();
    var size = parseInt($('#familysizeupdate').val());
    $('#familysizeupdate').val(size - 1);
}


$(document).on("change", "#grocerykitform_update .documentview", function () {
    $(this).parent().children(':first-child').css('background-color', '#2ECC4F');
    var files = $(this).prop("files");
    pdffile_url = URL.createObjectURL(files[0]);
    $(this).parents(':eq(3)').find(".pdfbutton").attr('data-id', pdffile_url);
});
$(document).on("change", "#grocerykitform_update .otherdocumentview", function () {
    $(this).parent().children(':first-child').css('background-color', '#2ECC4F');
    var files = $(this).prop("files");
    pdffile_url = URL.createObjectURL(files[0]);
    $(this).parents(':eq(3)').find(".pdfbutton").attr('data-id', pdffile_url);
});

$(document).on("click", ".pdfbutton", function () {
    var link = $(this).attr('data-id');
    PDFObject.embed(link, "#PDFViewerDiv_update");
    $('#PDFViewerModal_update').modal('show');
});

$('#videoupdate').on('change', function () {
    const size =
        (this.files[0].size / 1024 / 1024).toFixed(2);
    if (size > 20) {
        $('#videoupdate').val('');
        alertify.error("File must be less then 20mb");
    } else {
        var file = this.files[0];
        objectUrl = URL.createObjectURL(file);
        $("#playvideoupdate").prop("src", objectUrl);       
        $("#videosizeupdate").html('<b>' +
            'Ok, File size is: ' + size + " MB" + '</b>');
    }
});
$('#audioupdate').on('change', function () {
    const size =
        (this.files[0].size / 1024 / 1024).toFixed(2);

    if (size > 20) {
        $('#audioupdate').val('');
        alertify.error("File must be less then 20mb");
    } else {
        var file = this.files[0];
        objectUrl = URL.createObjectURL(file);
        $("#playaudioupdate").prop("src", objectUrl);       
        $("#audiosizeupdate").html('<b>' +
            'Ok, File size is: ' + size + " MB" + '</b>');
    }
});



$("#grocerykitform_update").validate({
    rules: {
        firstnameupdate: {
            required: true
        },
        lastnameupdate: {
            required: true
        },
        fatherorhusbandnameupdate: {
            required: true
        },
        medicalprobupdate: {
            required: true
        },
        phonenumber1update: {
            required: true
        },
        cnicupdate: {
            required: true
        },
        meritalstatusupdate: {
            required: true
        },
        currentstatusupdate: {
            required: true
        },
        genderupdate: {
            required: true
        },
        residencestatusupdate: {
            required: true
        },
        occupationupdate: {
            required: true
        },
        provinceupdate: {
            required: true
        },
        cityupdate: {
            required: true
        },
        dateofbirthupdate: {
            required: true
        },
        addressupdate: {
            required: true
        }
    },
    messages: {
        firstnameupdate: {
            required: "This is required"
        },
        lastnameupdate: {
            required: "This is required"
        },
        fatherorhusbandnameupdate: {
            required: "This is required"
        },
        medicalprobupdate: {
            required: "This is required"
        },
        phonenumber1update: {
            required: "This is required"
        },
        cnicupdate: {
            required: "This is required"
        },
        meritalstatusupdate: {
            required: "This is required"
        },
        currentstatusupdate: {
            required: "This is required"
        },
        genderupdate: {
            required: "This is required"
        },
        residencestatusupdate: {
            required: "This is required"
        },
        occupationupdate: {
            required: "This is required"
        },
        provinceupdate: {
            required: "This is required"
        },
        cityupdate: {
            required: "This is required"
        },       
        dateofbirthupdate: {
            required: "This is required"
        },
        addressupdate: {
            required: "This is required"
        }
    },
    highlight: function (element) {
        $($(element).attr('id') + '-error').addClass('error')
    },
    unhighlight: function (element) {
        $($(element).attr('id') + '-error').removeClass('error')
    },
    submitHandler: function (form) {
        $("#spinnerloadupdate").removeClass('d-none');
        var formData = new FormData();
        var groceryKitId = $("#grocerykitId").val();
        var formno = $("#formnumber").val();
        var existringImageUrl = $("#existingimageurl").val();

        var existingcnicfronturl = $("#existingcnicfronturl").val();
        var existingcnicbackurl = $("#existingcnicbackurl").val();
        var existingdisabilityurl = $("#existingdisabilityurl").val();
        var existingdeathurl = $("#existingdeathurl").val();
        var existingbformurl = $("#existingbformurl").val();
        var existingelectricityurl = $("#existingelectricityurl").val();
        var existingptclurl = $("#existingptclurl").val();
        var existingappurl = $("#existingappurl").val();
        var existingotherdoc1url = $("#existingotherdoc1url").val();
        var existingotherdoc2url = $("#existingotherdoc2url").val();
        var existingvideourl = $("#existingvideo").val();
        var existingaudiourl = $("#existingaudio").val();

        var firstname = $('#firstnameupdate').val();
        var lastname = $('#lastnameupdate').val();
        var fatherorhusbandname = $('#fatherorhusbandnameupdate').val();
        var address = $('#addressupdate').val();
        var medicalprobId = $("#medicalprobupdate option:selected").val();
        var phonenumber1 = $('#phonenumber1update').val();
        var phonenumber2 = $('#phonenumber2update').val();
        var occupationId = $("#occupationupdate option:selected").val();
        var provinceId = $("#provinceupdate option:selected").val();
        var cityId = $("#cityupdate option:selected").val();
        var meritalstatus = $("#meritalstatusupdate option:selected").text();
        var otherdocumentname = $("#otherdocumentnameupdate").val();
        var otherdocument2name = $("#otherdocument2nameupdate").val();

        //single photo upload
        var fileUpload = $("#photoupdate").get(0);
        var files = fileUpload.files;
        formData.append('photoupdate', files[0]);

        var fileUpload2 = $("#CNICFRONTupdate").get(0);
        var files2 = fileUpload2.files;
        formData.append('cnicfrontupdate', files2[0]);

        var fileUpload3 = $("#CNICBACKupdate").get(0);
        var files3 = fileUpload3.files;
        formData.append('cnicbackupdate', files3[0]);

        var fileUpload4 = $("#disabilitycertificateupdate").get(0);
        var files4 = fileUpload4.files;
        formData.append('disabilitycertificateupdate', files4[0]);

        var fileUpload5 = $("#deathcertificateupdate").get(0);
        var files5 = fileUpload5.files;
        formData.append('deathcertificateupdate', files5[0]);

        var fileUpload6 = $("#bformupdate").get(0);
        var files6 = fileUpload6.files;
        formData.append('bformupdate', files6[0]);

        var fileUpload7 = $("#electricitybillupdate").get(0);
        var files7 = fileUpload7.files;
        formData.append('electricitybillupdate', files7[0]);

        var fileUpload8 = $("#ptclbillupdate").get(0);
        var files8 = fileUpload8.files;
        formData.append('ptclbillupdate', files8[0]);

        var fileUpload9 = $("#applicationupdate").get(0);
        var files9 = fileUpload9.files;
        formData.append('applicationupdate', files9[0]);

        var fileUpload10 = $("#OtherDocumentupdate").get(0);
        var files10 = fileUpload10.files;
        formData.append('OtherDocumentupdate', files10[0]);

        var fileUpload11 = $("#OtherDocument2update").get(0);
        var files11 = fileUpload11.files;
        formData.append('OtherDocument2update', files11[0]);

        var fileUpload12 = $("#videoupdate").get(0);
        var files12 = fileUpload12.files;
        formData.append('VideoUpdate', files12[0]);

        var fileUpload13 = $("#audioupdate").get(0);
        var files13 = fileUpload13.files;
        formData.append('AudioUpdate', files13[0]);

        var dateofbirth = $('#dateofbirthupdate').val();
        var oldformnumber = $('#oldformnoupdate').val();
        var cnic = $('#cnicupdate').val();
        var currentstatusId = $("#currentstatusupdate option:selected").val();
        var gender = $("#genderupdate option:selected").text();
        var residencestatus = $("#residencestatusupdate option:selected").text();
        var familymembersdetails = getfamilymembersdetailsupdate();
        var foodcost = $('#foodcostupdate').val();
        var houserent = $('#houserentupdate').val();
        var schoolcost = $('#schoolcostupdate').val();
        var utilitiescost = $('#utilitiescostupdate').val();
        var medicalcost = $('#medicalcostupdate').val();
        var othercost = $('#othercostupdate').val();
        var totalcost = $('#totalcostupdate').val();
        var familysize = $('#familysizeupdate').val();
        var salary = $('#salaryupdate').val();
        var donations = $('#donationsupdate').val();
        var otherincome = $('#otherincomeupdate').val();
        var totalincome = $('#totalincomeupdate').val();
        var shortfallcash = $('#shortfallcashupdate').val();
        var remarks = $('#remarksupdate').val();
        var zakatacceptable = $("input[name='custom-radio-3-update']:checked").val();
        var Grocerykit = {
            GroceryKitId: groceryKitId,
            FormNo: formno,
            ExistingPhotoUrl: existringImageUrl,
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
            Documents: null,
            Remarks: remarks,
            CNICFrontUrl: existingcnicfronturl,
            CNICBackUrl: existingcnicbackurl,
            DisabilityCertificateUrl: existingdisabilityurl,
            DeathCertificateUrl: existingdeathurl,
            BFormUrl: existingbformurl,
            ElectricityBill: existingelectricityurl,
            PtclBillUrl: existingptclurl,
            ApplicationUrl: existingappurl,
            OtherDocument1Url: existingotherdoc1url,
            OtherDocument2Url: existingotherdoc2url,
            OtherDocumentName1: otherdocumentname,
            OtherDocumentName2: otherdocument2name,
            VideoUrl: existingvideourl,
            AudioUrl: existingaudiourl
        };
        formData.append('modelupdate', JSON.stringify(Grocerykit));
        $.ajax({
            url: "/GroceryKit/UpdateForm",
            type: 'POST',
            datatype: "json",
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                if (response.isError !== true) {  
                    $("#spinnerloadupdate").addClass('d-none');
                    alertify.success("Record has been successfully Updated with " + '"' + "Form NUMBER : " + response.message + '"');
                    if (response.roleName == "Beneficiary") {
                        $(location).attr('href', '/UserDashboard/UpdateSuccessforBeneficiary');
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

function getfamilymembersdetailsupdate() {    
    var arrayDDetailupdate = [];
    var dDetailupdate;
    $('#GroceryKitTblupdate tbody tr').not(".sampleRowupdate").each(function () {
        dDetailupdate = {
            Name: $(this).find('td:eq(0) input').val(),
            RelationId: $(this).find('td:eq(1) option:selected').val(),
            Age: $(this).find('td:eq(2) input').val(),
            FamilyMemberStatusId: $(this).find('td:eq(3) option:selected').val()
        };
        arrayDDetailupdate.push(dDetailupdate);

    });
    return arrayDDetailupdate;
}

$('#foodcostupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#houserentupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#schoolcostupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#utilitiescostupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#medicalcostupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#othercostupdate').on('change', function () {
    var foodcost = parseFloat($('#foodcostupdate').val());
    var houserent = parseFloat($('#houserentupdate').val());
    var schoolcost = parseFloat($('#schoolcostupdate').val());
    var utilitiescost = parseFloat($('#utilitiescostupdate').val());
    var medicalcost = parseFloat($('#medicalcostupdate').val());
    var othercost = parseFloat($('#othercostupdate').val());

    $('#totalcostupdate').val(foodcost + houserent + schoolcost + utilitiescost + medicalcost + othercost);
    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);

});
$('#salaryupdate').on('change', function () {
    var salary = parseFloat($('#salaryupdate').val());
    var donations = parseFloat($('#donationsupdate').val());
    var otherincome = parseFloat($('#otherincomeupdate').val());
    $('#totalincomeupdate').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);
});
$('#donationsupdate').on('change', function () {
    var salary = parseFloat($('#salaryupdate').val());
    var donations = parseFloat($('#donationsupdate').val());
    var otherincome = parseFloat($('#otherincomeupdate').val());
    $('#totalincomeupdate').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);
});
$('#otherincomeupdate').on('change', function () {
    var salary = parseFloat($('#salaryupdate').val());
    var donations = parseFloat($('#donationsupdate').val());
    var otherincome = parseFloat($('#otherincomeupdate').val());
    $('#totalincomeupdate').val(salary + donations + otherincome);

    var totalcost = parseFloat($('#totalcostupdate').val());
    var totalincome = parseFloat($('#totalincomeupdate').val());
    $('#shortfallcashupdate').val(totalcost - totalincome);
});

