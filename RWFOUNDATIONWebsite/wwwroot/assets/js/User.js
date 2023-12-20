
$(document).ready(function () {
    RequestCount();
    $("#Request_list_tbl").DataTable();
});
function RequestCount() {
    $.ajax({
        url: "/Admin/GetCount",
        type: 'GET',
        async: true,
        datatype: "json",        
        success: function (result) {           
            if (!result.isError) {
                if (result.message == "Admin") {
                    $("#reqcount").text(result.requestCount);
                    $("#requestcount").text(result.requestCount);
                }
            }
            else {
                alertify.error(result.error);
            }
        },
        error: function (error) {
            alertify.error(error);
        }
    });
}
var connection = new signalR.HubConnectionBuilder().withUrl("/DonorRequestHub").build()
connection.on('DonorRequest', function (count, Subject) {
    RequestCount();    

});
connection.start();

$('#requestify').click(function (e) {
    var isSessionSetCount = true;
    getRequest("", "", isSessionSetCount);   
});

Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}


// Calaulcate Diffrecne in day
const _MS_PER_DAY = 1000 * 60 * 60 * 24;
function dateDiffInDays(a, b) {   
    const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

function getRequest(count, Subject, isSessionSetCount) {
    var res = "<div class='list-group list - group - flush'>";
    var obj = { isSessionSetCount: isSessionSetCount };
    
    $.ajax({
        url: "/Admin/GetDonorRequest",
        type: 'POST',
        async: true,
        datatype: "json",
        data: obj,
        success: function (result) {            
            if (!result.isError) {
                $('#contentrequest').html("");
                if (result.requestCount != 0) {
                    
                    var requests = result.objectData;
                    requests.forEach(element => {
                        var res = "<div class='list-group list - group - flush'>";
                        var d = new Date,
                            dformat = [(d.getMonth() + 1).padLeft(),
                            d.getDate().padLeft(),
                            d.getFullYear()].join('-') +
                                ' ' +
                                [d.getHours().padLeft(),
                                d.getMinutes().padLeft(),
                                d.getSeconds().padLeft()].join(':');

                        var d1 = new Date(element.createdOn),
                            dformatRequestDate = [(d1.getMonth() + 1).padLeft(),
                            d1.getDate().padLeft(),
                            d1.getFullYear()].join('-') +
                                ' ' +
                                [d1.getHours().padLeft(),
                                d1.getMinutes().padLeft(),
                                    d1.getSeconds().padLeft()].join(':');
                        
                        dayDiffernace = dateDiffInDays(new Date(dformatRequestDate), new Date(dformat))
                        if (dayDiffernace > 0) {
                            if (dayDiffernace == 1) {
                                res = `<a href="javascript:" id=` + element.id + ` class="list-group-item list-group-item-action media d-flex align-items-start">
                                                        <div class="media-body line-height-condenced ml-3">
                                                            <div class="text-dark">`+ element.donorName + `</div>
                                                            <div class="text-light small mt-1">
                                                                Rs.: `+ element.expectedDonation +`
                                                            </div>
                                                            <div class="text-light small mt-1">`+ dayDiffernace + ` days ago</div>
                                                        </div>
                                                    </a></div>`;                               
                            }
                            else {
                               res = `<a href="javascript:" id=` + element.id + ` class="list-group-item list-group-item-action media d-flex align-items-start">
                                                        <div class="media-body line-height-condenced ml-3">
                                                            <div class="text-dark">`+ element.donorName + `</div>
                                                            <div class="text-light small mt-1">
                                                                Rs.: `+ element.expectedDonation +`
                                                            </div>
                                                            <div class="text-light small mt-1">`+ dayDiffernace + ` days ago</div>
                                                        </div>
                                                    </a></div>`;                               
                            }

                        }
                        else {
                           res = `<a href="javascript:" id=` + element.id + ` class="list-group-item list-group-item-action media d-flex align-items-start">
                                                        <div class="media-body line-height-condenced ml-3">
                                                            <div class="text-dark">`+ element.donorName + `</div>
                                                            <div class="text-light small mt-1">
                                                                Rs. : `+ element.expectedDonation +`
                                                            </div>
                                                            <div class="text-light small mt-1">Today</div>
                                                        </div>
                                                    </a></div>`;                           
                        }                       
                       
                        $('#contentrequest').append(res);
                        console.log(res);
                    });
                   
                }
                else {
                    $('#contentrequest').empty();
                    $('#contentrequest').append($('<span>No data available</span>'));      
                }
            }
            else {

            }

            // Show alert message only realted user
            var isAlert = parseInt(result.requestCount) - parseInt(count);
            if (isAlert == 1) {
                alertify.success(Subject);
            }           
        },
        error: function (error) {
            alertify.error(error);
        }
    });
}

$("#Donorupdate").click(function () {
    var asvalue = $("input[name='As']:checked").val();
    if (asvalue == "Donor") {
        $("#todonatelabelupdate").removeClass("d-none");
        $("#todonateupdate").removeClass("d-none");
        $("#estimatelabelupdate").removeClass("d-none");
        $("#estimateupdate").removeClass("d-none");
    } else {
        $("#todonatelabelupdate").addClass("d-none");
        $("#todonateupdate").addClass("d-none");
        $("#estimatelabelupdate").addClass("d-none");
        $("#estimateupdate").addClass("d-none");
    }
});
$("#Beneficiaryupdate").click(function () {
    var asvalue = $("input[name='As']:checked").val();
    if (asvalue == "Beneficiary") {
        $("#todonatelabelupdate").addClass("d-none");
        $("#todonateupdate").addClass("d-none");
        $("#estimatelabelupdate").addClass("d-none");
        $("#estimateupdate").addClass("d-none");
    } else {
        $("#todonatelabelupdate").removeClass("d-none");
        $("#todonateupdate").removeClass("d-none");
        $("#estimatelabelupdate").removeClass("d-none");
        $("#estimateupdate").removeClass("d-none");

    }
});
$("#userupdateform").validate({
    errorClass: 'errors',
    rules: {
        PhoneNumberupdate: {
            required: true
        },
        dateofbirthupdate: {
            required: true
        },
        dateofbirth: {
            required: true
        },
        addressupdate: {
            required: true
        }
    },
    messages: {
        PhoneNumberupdate: {
            required: "This is required"
        },
        dateofbirthupdate: {
            required: "This is required"
        },
        dateofbirth: {
            required: "This is required"
        },
        addressupdate: {
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
        $("#spinnerloaduser").removeClass('d-none');       
        var as = $("input[name='As']:checked").val();
        var mobile = $("#PhoneNumberupdate").val();
        var dateofbirth = $("#dateofbirthupdate").val();
        var address = $("#addressupdate").val();
        var todonate = $("#todonateupdate option:selected").text();
        var estimate = $("#estimateupdate").val();
        var user = {
            As: as,
            PhoneNumber: mobile,
            DateOfBirth: dateofbirth,
            Address: address,
            ToDonate: todonate,
            Estimate: estimate
        };       
        $.ajax({
            url: "/UserDashboard/UpdateExternalLoginUser",
            type: 'POST',
            datatype: "json",           
            data: { model: user },
            success: function (response) {
                if (response.isError !== true) {
                    $("#spinnerloaduser").addClass('d-none');
                    alertify.success("Record has been successfully Updated");
                    $(location).attr('href', '/UserDashboard/Index');
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