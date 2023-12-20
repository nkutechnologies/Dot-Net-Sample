$(document).ready(function () {
    $("#packagedetailtbl").DataTable();    
    $('#packageitemtblupdate tbody tr').not(".itemsampleRowupdate").each(function () {
        var p = parseFloat($(this).find('td:eq(1) input').val());
        var q = parseFloat($(this).find('td:eq(2) input').val());
        $(this).find('td:eq(3) input').val(p * q);
    });
    var total = 0;
    $('#packageitemtblupdate tbody tr').not(".itemsampleRowupdate").each(function () {
        var t = parseFloat($(this).find('td:eq(3) input').val());
        total += t;
    });
    $("#totalvalueupdate").text(parseFloat(total));
    //getrates();
   
});

function getrates() {
   
    $.ajax({
        url: "/Package/getRates",              
        dataType: 'json',        
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
                      
        success: function (response) {
            console.log(response);
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
}

function AddNewItem() {
    var retRow = $('.itemsampleRow').clone().removeClass('itemsampleRow').css('display', '');
    $('#packageitemtbl tbody tr:last').before(retRow); 
    var total = 0;
    $('#packageitemtbl tbody tr').each(function () {
        var t = parseFloat($(this).find('td:eq(3) input').val());
        total += t;
    });
    $("#totalvalue").text(parseFloat(total));
    $("#valuepkr").val(parseFloat(total));
    $("#valuesr").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
    $("#valueusd").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
}


function deleteitemRow(row) {
    if ($('#packageitemtbl tbody tr').length > 2) {
        $(row).parent().parent().remove();
        var total = 0;
        $('#packageitemtbl tbody tr').each(function () {
            var t = parseFloat($(this).find('td:eq(3) input').val());
            total += t;
        });
        $("#totalvalue").text(parseFloat(total));
        $("#valuepkr").val(parseFloat(total));
        $("#valuesr").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
        $("#valueusd").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
    }
}
function AddNewItemupdate() {
    var retRow = $('.itemsampleRowupdate').clone().removeClass('itemsampleRowupdate').css('display', '');
    $('#packageitemtblupdate tbody tr:last').before(retRow);
    var total = 0;
    $('#packageitemtblupdate tbody tr').each(function () {
        var t = parseFloat($(this).find('td:eq(3) input').val());
        total += t;
    });
    $("#totalvalueupdate").text(parseFloat(total));
    $("#valuepkrupdate").val(parseFloat(total));
    $("#valuesrupdate").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
    $("#valueusdupdate").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
}


function deleteitemRowupdate(row) {
    if ($('#packageitemtblupdate tbody tr').length > 2) {
        $(row).parent().parent().remove();
        var total = 0;
        $('#packageitemtblupdate tbody tr').each(function () {
            var t = parseFloat($(this).find('td:eq(3) input').val());
            total += t;
        });
        $("#totalvalueupdate").text(parseFloat(total));
        $("#valuepkrupdate").val(parseFloat(total));
        $("#valuesrupdate").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
        $("#valueusdupdate").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
    }
}

$(document).on('change', '.items', function () {   
    var itemid = $(this).val();
    var row = $(this);
    $.ajax({
        url: "/Package/getPrice",
        type: 'POST',
        datatype: "json",
        data: { ItemId: itemid },
        success: function (response) { 
            $(row).closest('tr').find('.price').val(response.message);           
            var total = 0;
            $('#packageitemtbl tbody tr').each(function () {
                var t = parseFloat($(this).find('td:eq(3) input').val());
                total += t;                
            });
            $("#totalvalue").text(parseFloat(total));
            $("#valuepkr").val(parseFloat(total));
            $("#valuesr").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
            $("#valueusd").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
});
$(document).on('change', '.quantity', function () {
    var quantity = $(this).closest('tr').find('.quantity').val();
    var price = $(this).closest('tr').find('.price').val();
    $(this).closest('tr').find('.total').val(parseFloat(price) * parseFloat(quantity));
    var total = 0;    
    $('#packageitemtbl tbody tr').each(function () {
        var t = parseFloat($(this).find('td:eq(3) input').val());
        total += t;      
    });
    $("#totalvalue").text(parseFloat(total));
    $("#valuepkr").val(parseFloat(total));
    $("#valuesr").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
    $("#valueusd").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
   
});

$("#PackageItems").validate({
    errorClass: 'errors',
    rules: {
        package: {
            required: true
        },
        packagename: {
            required: true
        },
        totalfamilymember: {
            required: true
        },
        item: {
            required: true
        },
        quantity: {
            required: true
        }
    },
    messages: {
        package: {
            required: "This is required"
        },
        packagename: {
            required: "This is required"
        },
        totalfamilymember: {
            required: "This is required"
        },
        item: {
            required: "This is required"
        },
        quantity: {
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
        $("#spinneritem").removeClass('d-none');
        var packageId = $("#package option:selected").val();
        var packagename = $("#packagename").val();
        var totalfamilymember = $("#totalfamilymember").val();
        var valuepkr = $("#valuepkr").val();
        var valuesr = $("#valuesr").val();
        var valueusd = $("#valueusd").val();
        var packageitems = GetPackageItems();
        var package = {
            PackageId: packageId,
            PackageDetailName: packagename,
            TotalFamilyMember: totalfamilymember,
            PackageValuePKR: valuepkr,
            PackageValueSR: valuesr,
            PackageValueUSD: valueusd,
            PackageItems: packageitems            
        };       
        $.ajax({
            url: "/Package/AddPackageItem",
            type: 'POST',
            datatype: "json",
            data: { model: package },
            success: function (response) {
                if (response.isError !== true) {
                    $("#spinneritem").addClass('d-none');
                    alertify.success("Record has been successfully saved");
                    $(location).attr('href', '/Package/PackageItemsList');
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

function GetPackageItems() {
    var arrayDDetail = [];
    var dDetail;
    $('#packageitemtbl tbody tr').not(".itemsampleRow").each(function () {
        dDetail = {
            ItemId: $(this).find('td:eq(0) option:selected').val(),
            Price: $(this).find('td:eq(1) input').val(),
            PackageQuantity: $(this).find('td:eq(2) input').val()           
        };
        arrayDDetail.push(dDetail);

    });
    return arrayDDetail;
}

$(document).on('change', '.itemsupdate', function () {
    
    var itemid = $(this).val();
    var row = $(this);
    $.ajax({
        url: "/Package/getPrice",
        type: 'POST',
        datatype: "json",
        data: { ItemId: itemid },
        success: function (response) {
            $(row).closest('tr').find('.priceupdate').val(response.message);
            var total = 0;
            $('#packageitemtblupdate tbody tr').each(function () {
                var t = parseFloat($(this).find('td:eq(3) input').val());
                total += t;
            });
            $("#totalvalueupdate").text(parseFloat(total));
            $("#valuepkrupdate").val(parseFloat(total));
            $("#valuesrupdate").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
            $("#valueusdupdate").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));
        },
        error: function (response) {
            alertify.error(response.message);
        }
    });
});
$(document).on('change', '.quantityupdate', function () {
    var quantity = $(this).closest('tr').find('.quantityupdate').val();
    var price = $(this).closest('tr').find('.priceupdate').val();
    $(this).closest('tr').find('.totalupdate').val(parseFloat(price) * parseFloat(quantity));
    var total = 0;
    $('#packageitemtblupdate tbody tr').each(function () {
        var t = parseFloat($(this).find('td:eq(3) input').val());
        total += t;
    });
    $("#totalvalueupdate").text(parseFloat(total));
    $("#valuepkrupdate").val(parseFloat(total));
    $("#valuesrupdate").val(parseFloat(parseFloat(total) / 43.64).toFixed(2));
    $("#valueusdupdate").val(parseFloat(parseFloat(total) / 163.75).toFixed(2));

});

$("#PackageItemsupdate").validate({
    errorClass: 'errors',
    rules: {
        packageupdate: {
            required: true
        },
        packagenameupdate: {
            required: true
        },
        totalfamilymemberupdate: {
            required: true
        },
        itemupdate: {
            required: true
        },
        quantityupdate: {
            required: true
        }
    },
    messages: {
        packageupdate: {
            required: "This is required"
        },
        packagenameupdate: {
            required: "This is required"
        },
        totalfamilymemberupdate: {
            required: "This is required"
        },
        itemupdate: {
            required: "This is required"
        },
        quantityupdate: {
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
        $("#spinneritemupdate").removeClass('d-none');
        var packagedetailId = $("#packagedetailid").val();
        var packageId = $("#packageupdate option:selected").val();
        var packagename = $("#packagenameupdate").val();
        var totalfamilymember = $("#totalfamilymemberupdate").val();
        var valuepkr = $("#valuepkrupdate").val();
        var valuesr = $("#valuesrupdate").val();
        var valueusd = $("#valueusdupdate").val();
        var packageitems = GetPackageItemsupdate();
        var package = {
            PackageDetailId: packagedetailId,
            PackageId: packageId,
            PackageDetailName: packagename,
            TotalFamilyMember: totalfamilymember,
            PackageValuePKR: valuepkr,
            PackageValueSR: valuesr,
            PackageValueUSD: valueusd,
            PackageItems: packageitems
        };
        $.ajax({
            url: "/Package/UpdatePackageItem",
            type: 'POST',
            datatype: "json",
            data: { model: package },
            success: function (response) {
                if (response.isError !== true) {
                    $("#spinneritemupdate").addClass('d-none');
                    alertify.success("Record has been successfully Updated");
                    $(location).attr('href', '/Package/PackageItemsList');
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

function GetPackageItemsupdate() {
    var arrayDDetail = [];
    var dDetail;
    $('#packageitemtblupdate tbody tr').not(".itemsampleRowupdate").each(function () {
        dDetail = {
            ItemId: $(this).find('td:eq(0) option:selected').val(),
            Price: $(this).find('td:eq(1) input').val(),
            PackageQuantity: $(this).find('td:eq(2) input').val()
        };
        arrayDDetail.push(dDetail);

    });
    return arrayDDetail;
}

function DeletePackage(id) {

    //swal({
    //    title: "Are you sure?",
    //    text: "You will not be able to recover this imaginary file!",
    //    type: "warning",
    //    showCancelButton: true,
    //    confirmButtonClass: "btn-danger",
    //    confirmButtonText: "Yes, delete",
    //    cancelButtonText: "No, cancel",
    //    closeOnConfirm: false,
    //    closeOnCancel: false
    //},
    //    function (isConfirm) {
    //        if (isConfirm) {
    //            swal("Deleted!", "Your imaginary file has been deleted.", "success");
    //        } else {
    //            swal("Cancelled", "Your imaginary file is safe :)", "error");
    //        }
    //    });
    alertify.confirm('Delete', 'Do you want to Delete this Package?', function () {
        $.ajax({
            url: "/Package/DeletePackageItem",
            type: 'POST',
            datatype: "json",
            data: { Id: id },
            success: function (response) {
                if (response.isError !== true) {
                    alertify.success("Package has been Successfuly Deleted");
                    $(location).attr('href', '/Package/PackageItemsList');
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