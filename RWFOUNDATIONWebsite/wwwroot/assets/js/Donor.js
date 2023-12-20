function GetSponcersssFamilies() {
    var detailsids = getSponcerFamilyDetailsToSponcer();
    var noofmonth = $("#noofmonth").val();
    var totalmeals = $("#totalmeals").text();
    $.ajax({
        url: "/Donors/SponcerTo",
        type: 'POST',
        datatype: "json",
        data: { sponcers: detailsids, noofmonth: noofmonth, totalmeals: totalmeals },
        success: function (response) {
            $('#modelSponserKitTitle').text("Sponsor To Beneficiaries");
            $("#modalSponcerKitBody").html(response);
            $("#modal_sponcergrocerykit").modal('show');
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
}
function getSponcerFamilyDetailsToSponcer() {
    var arrayDDetail = [];
    var dDetail;
    var rows = $("#grocerkittablefordonor").dataTable().fnGetNodes();
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

function SponceredFamilies() {
    var noofmonth = $("#Noofmonth").text();
    var totalfamilies = $("#totalfam").text();
    var estimates = $("#estimated").text();
    var meals = $("#meals").text();
    var beneficiaries = getbeneficiaries();
    var sponsor = {
        NoOfMonth: noofmonth,
        TotalFamilies: totalfamilies,
        EstimatedExpense: estimates,
        TotalMeals: meals,
        Beneficiaries: beneficiaries
    };
    $.ajax({
        url: "/Donors/SponsorToData",
        type: 'POST',
        datatype: "json",
        data: { model: sponsor },
        success: function (response) {
            if (response.error != true) {
                $("#modal_sponcergrocerykit").modal("hide");
                alertify.success("Beneficiaries Sposored Successfully");
                $(location).attr('href', '/Donors/DonorSponsorList'); 

            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
}
function getbeneficiaries() {
    var arrayDDetail = [];
    var dDetail;
    $('#grocerytblSponcer tbody tr').each(function () {
        dDetail = {
            GroceryKitId: $(this).find('td:eq(0) input').val(),
        };
        arrayDDetail.push(dDetail);
    });

    return arrayDDetail;
}
$('#sponcerfrom').on('change', function () {
    debugger;
    var fromdate = new Date($('#sponcerfrom').val());
    var todate = new Date($('#sponcerto').val());
    var days = (end - start) / (1000 * 60 * 60 * 24);
    var totaldays = parseInt(Math.round(days));
});

$(document).on('click', '#grocerkittablefordonor tbody .custom-control-input', function (e) {
    var arrayDDetail = [];
    var dDetail;
    var arrayformNo = [];
    var formNo;
    $('#grocerkittablefordonor tbody tr').each(function () {
        dDetail = {
            Checked: $(this).find('td:eq(0) input:checked').val(),

        };
        if (dDetail.Checked != undefined) {
            arrayDDetail.push(dDetail);
        }
    });
    if (arrayDDetail.length != 0) {
        $("#sponcerbtn").removeClass('d-none');
    }
    if (arrayDDetail.length == 0) {
        $("#sponcerbtn").addClass('d-none');
    }

    $('#grocerkittablefordonor tbody tr').each(function () {
        if ($(this).find('td:eq(0) input:checked').val() != undefined) {
            formNo = {
                FormNo: $(this).find('td:eq(1)').text(),
                FamilySize: $(this).find('td:eq(4)').text(),
            };
            arrayformNo.push(formNo);  
        }       
            
    });
    $("#totalfamilies").text(arrayformNo.length);
    var packageId = $("#packageId option:selected").val();
    var noofmonth = $("#noofmonth").val();
    var getvalues = {
        PackageId: packageId,
        NoOfMonth: noofmonth,
        FormNos: arrayformNo
    }
    $.ajax({
        url: "/Donors/AutoCalculation",
        type: 'POST',
        datatype: "json",
        data: { model: getvalues },
        success: function (response) {
            if (response.isError != true) {
                $("#estimatedexpense").text(new Intl.NumberFormat().format(response.estimatedExpense));
                $("#packagevalue").text(new Intl.NumberFormat().format(response.packagevalue));
                $("#totalmeals").text(new Intl.NumberFormat().format(response.totalMeals));

                var divorcee = 0;
                var divorceedependents = 0;
                var widow = 0;
                var widowdependents = 0;
                var disabled = 0;
                var disableddependents = 0;
                var oldage = 0;
                var oldagedependents = 0;
                var deserving = 0;
                var deservingdependents = 0;
                var orphanse = 0;
                var orphansedependents = 0;
                if (response.objectData != null) {
                    $.each(response.objectData.beneficaryForCalculations, function (index, item) {
                        if (item.status == "Divorcee") {
                            divorcee += parseInt(1);
                            divorceedependents += parseInt(item.dependents);
                        } else if (item.status == "Widow") {
                            widow += parseInt(1);
                            widowdependents += parseInt(item.dependents);
                        } else if (item.status == "Disabled") {
                            disabled += parseInt(1);
                            disableddependents += parseInt(item.dependents);
                        } else if (item.status == "Old Aged") {
                            oldage += parseInt(1);
                            oldagedependents += parseInt(item.dependents);
                        } else if (item.status == "Deserving") {
                            deserving += parseInt(1);
                            deservingdependents += parseInt(item.dependents);
                        } else if (item.status == "Orphans") {
                            orphanse += parseInt(1);
                            orphansedependents += parseInt(item.dependents);
                        }
                    });
                    $("#divorcee").text(divorcee);
                    $("#divorceedependents").text(divorceedependents);
                    $("#widows").text(widow);
                    $("#widowsdependents").text(widowdependents);
                    $("#disabled").text(disabled);
                    $("#disableddependents").text(disableddependents);
                    $("#oldage").text(oldage);
                    $("#oldagedependents").text(oldagedependents);
                    $("#deserving").text(deserving);
                    $("#deservingdependents").text(deservingdependents);
                    $("#Orphans").text(orphanse);
                    $("#orphansedependents").text(orphansedependents);
                } else {
                    $("#estimatedexpense").text(0);
                    $("#packagevalue").text(0);
                    $("#totalmeals").text(0);
                    $("#divorcee").text(0);
                    $("#divorceedependents").text(0);
                    $("#widows").text(0);
                    $("#widowsdependents").text(0);
                    $("#disabled").text(0);
                    $("#disableddependents").text(0);
                    $("#oldage").text(0);
                    $("#oldagedependents").text(0);
                    $("#deserving").text(0);
                    $("#deservingdependents").text(0);
                    $("#Orphans").text(0);
                    $("#orphansedependents").text(0);
                }
                
               
            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
});
$('#checkheaderSponcer').on('click', function () {
    var value = $('#checkheaderSponcer').is(":checked");
    if (value == true) {
        $('#checkbox').attr("checked", true)
        $(this).closest('table').find('td input:checkbox').attr('checked', true);
        var arrayDDetail = [];
        var dDetail;
        $('#grocerkittablefordonor tbody tr').each(function () {
            dDetail = {
                Checked: $(this).find('td:eq(0) input:checked').val(),

            };
            if (dDetail.Checked != undefined) {
                arrayDDetail.push(dDetail);
            }
        });
        if (arrayDDetail.length != 0) {
            $("#sponcerbtn").removeClass('d-none');
        }
        if (arrayDDetail.length == 0) {
            $("#sponcerbtn").addClass('d-none');
        }
    }
    else {
        $('#checkbox').attr("checked", false)
        $(this).closest('table').find('td input:checkbox').attr('checked', false);
        var arrayDDetail = [];
        var dDetail;
        $('#grocerkittablefordonor tbody tr').each(function () {
            dDetail = {
                Checked: $(this).find('td:eq(0) input:checked').val(),

            };
            if (dDetail.Checked != undefined) {
                arrayDDetail.push(dDetail);
            }
        });
        if (arrayDDetail.length != 0) {
            $("#sponcerbtn").removeClass('d-none');
        }
        if (arrayDDetail.length == 0) {
            $("#sponcerbtn").addClass('d-none');
        }
    }
    //sponsor calculated
    var arrayformNo = [];
    var formNo;
    $('#grocerkittablefordonor tbody tr').each(function () {
        if ($(this).find('td:eq(0) input:checked').val() != undefined) {
            formNo = {
                FormNo: $(this).find('td:eq(1)').text(),
                FamilySize: $(this).find('td:eq(4)').text(),
            };
            arrayformNo.push(formNo);
        }

    });
    $("#totalfamilies").text(arrayformNo.length);
    var packageId = $("#packageId option:selected").val();
    var noofmonth = $("#noofmonth").val();
    var getvalues = {
        PackageId: packageId,
        NoOfMonth: noofmonth,
        FormNos: arrayformNo
    }
    $.ajax({
        url: "/Donors/AutoCalculation",
        type: 'POST',
        datatype: "json",
        data: { model: getvalues },
        success: function (response) {
            if (response.isError != true) {
                if (response.objectData != null) {
                    $("#estimatedexpense").text(new Intl.NumberFormat().format(response.estimatedExpense));
                    $("#packagevalue").text(new Intl.NumberFormat().format(response.packagevalue));
                    $("#totalmeals").text(new Intl.NumberFormat().format(response.totalMeals));

                    var divorcee = 0;
                    var divorceedependents = 0;
                    var widow = 0;
                    var widowdependents = 0;
                    var disabled = 0;
                    var disableddependents = 0;
                    var oldage = 0;
                    var oldagedependents = 0;
                    var deserving = 0;
                    var deservingdependents = 0;
                    var orphanse = 0;
                    var orphansedependents = 0;
                    $.each(response.objectData.beneficaryForCalculations, function (index, item) {
                        if (item.status == "Divorcee") {
                            divorcee += parseInt(1);
                            divorceedependents += parseInt(item.dependents);
                        } else if (item.status == "Widow") {
                            widow += parseInt(1);
                            widowdependents += parseInt(item.dependents);
                        } else if (item.status == "Disabled") {
                            disabled += parseInt(1);
                            disableddependents += parseInt(item.dependents);
                        } else if (item.status == "Old Aged") {
                            oldage += parseInt(1);
                            oldagedependents += parseInt(item.dependents);
                        } else if (item.status == "Deserving") {
                            deserving += parseInt(1);
                            deservingdependents += parseInt(item.dependents);
                        } else if (item.status == "Orphans") {
                            orphanse += parseInt(1);
                            orphansedependents += parseInt(item.dependents);
                        } 
                    });
                    $("#divorcee").text(divorcee);
                    $("#divorceedependents").text(divorceedependents);
                    $("#widows").text(widow);
                    $("#widowsdependents").text(widowdependents);
                    $("#disabled").text(disabled);
                    $("#disableddependents").text(disableddependents);
                    $("#oldage").text(oldage);
                    $("#oldagedependents").text(oldagedependents);
                    $("#deserving").text(deserving);
                    $("#deservingdependents").text(deservingdependents);
                    $("#Orphans").text(orphanse);
                    $("#orphansedependents").text(orphansedependents);
                } else {
                    $("#estimatedexpense").text(0);
                    $("#packagevalue").text(0);
                    $("#totalmeals").text(0);
                    $("#divorcee").text(0);
                    $("#divorceedependents").text(0);
                    $("#widows").text(0);
                    $("#widowsdependents").text(0);
                    $("#disabled").text(0);
                    $("#disableddependents").text(0);
                    $("#oldage").text(0);
                    $("#oldagedependents").text(0);
                    $("#deserving").text(0);
                    $("#deservingdependents").text(0);
                    $("#Orphans").text(0);
                    $("#orphansedependents").text(0);
                }
               

            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });


});

$("#noofmonth").on("change", function () {

    var arrayformNo = [];
    var formNo;
    $('#grocerkittablefordonor tbody tr').each(function () {
        if ($(this).find('td:eq(0) input:checked').val() != undefined) {
            formNo = {
                FormNo: $(this).find('td:eq(1)').text(),
                FamilySize: $(this).find('td:eq(4)').text(),
            };
            arrayformNo.push(formNo);
        }

    });
    $("#totalfamilies").text(arrayformNo.length);
    var packageId = $("#packageId option:selected").val();
    var noofmonth = $("#noofmonth").val();
    var getvalues = {
        PackageId: packageId,
        NoOfMonth: noofmonth,
        FormNos: arrayformNo
    }
    $.ajax({
        url: "/Donors/GetDataMonthWise",
        type: 'POST',
        datatype: "json",
        data: { model: getvalues },
        success: function (response) {
            if (response.isError != true) {
                $("#estimatedexpense").text(new Intl.NumberFormat().format(response.estimatedExpense));
                $("#packagevalue").text(new Intl.NumberFormat().format(response.packagevalue));
                $("#totalmeals").text(new Intl.NumberFormat().format(response.totalMeals));

            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });

});
$("#packageId").on("change", function () {

    var arrayformNo = [];
    var formNo;
    $('#grocerkittablefordonor tbody tr').each(function () {
        if ($(this).find('td:eq(0) input:checked').val() != undefined) {
            formNo = {
                FormNo: $(this).find('td:eq(1)').text(),
                FamilySize: $(this).find('td:eq(4)').text(),
            };
            arrayformNo.push(formNo);
        }

    });
    $("#totalfamilies").text(arrayformNo.length);
    var packageId = $("#packageId option:selected").val();
    var noofmonth = $("#noofmonth").val();
    var getvalues = {
        PackageId: packageId,
        NoOfMonth: noofmonth,
        FormNos: arrayformNo
    }
    $.ajax({
        url: "/Donors/PackageChange",
        type: 'POST',
        datatype: "json",
        data: { model: getvalues },
        success: function (response) {
            if (response.isError != true) {
                $("#estimatedexpense").text(new Intl.NumberFormat().format(response.estimatedExpense));
                $("#packagevalue").text(new Intl.NumberFormat().format(response.packagevalue));
                $("#totalmeals").text(new Intl.NumberFormat().format(response.totalMeals));

            } else {
                alertify.error(response.message);
            }
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });

});

$(document).ready(function () {
    $('#grocerkittablefordonor').DataTable();
    var arrayDDetail = [];
    var dDetail;
    $('#grocerkittablefordonor tbody tr').each(function () {
        dDetail = {
            Checked: $(this).find('td:eq(0) input:checked').val(),

        };
        if (dDetail.Checked != undefined) {
            arrayDDetail.push(dDetail);
        }
    });
    if (arrayDDetail.length != 0) {
        $("#sponcerbtn").removeClass('d-none');
    }
   
});
$("#Request_Beneficiary_Form").validate({
    errorClass: 'errors',
    rules: {
        expecteddonation: {
            required: true
        },
        donationtype: {
            required: true
        }
    },
    messages: {
        expecteddonation: {
            required: "This is required"
        },
        donationtype: {
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
        $("#spinnerrequest").removeClass('d-none');
        var donationTypeId = $("#donationtype option:selected").val();
        var expecteddonation = $("#expecteddonation").val(); 
        var beneficiaryType = $("input[name='type']:checked").val();
        var familysize = getfamilysize();
        if (familysize.length >= 1) {
            var request = {
                BeneficiaryType: beneficiaryType,
                DonationTypeId: donationTypeId,
                ExpectedDonation: expecteddonation,
                FamilyMemberSize: familysize
            };
            $.ajax({
                url: "/Donors/RequestedBeneficiary",
                type: 'POST',
                datatype: "json",
                data: { model: request },
                success: function (response) {
                    if (response.isError !== true) {
                        $("#spinnerrequest").addClass('d-none');
                        if (response.message == "Manual") {
                            $("#message").text("Your Request have been received , usually it takes one working day to process.");
                            $("#manualalert").removeClass("d-none");
                        }
                        else
                        {
                            if (response.nullMessage != null) {
                                $("#message1").text(response.nullMessage);
                                $("#nullalert").removeClass("d-none");
                            } else {
                                alertify.success("You Request have been Processed Successfuly");
                                $("#message").text("Successfuly Assigned Beneficiary Data. Please Check your Beneficiary List");
                                $("#manualalert").removeClass("d-none");
                            }                            
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
        } else {
            $("#spinnerrequest").addClass('d-none');
            alertify.error("Please Select Family Member First");
        }
       

    }
});

function getfamilysize() {
    var size = [];
    $.each($("input[name='member']:checked"), function () {
        size.push($(this).val());
    });
    return size;
}