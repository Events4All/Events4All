$('document').ready(function () {

    var $barcode = $('#barcode');
    $barcode.focus();

    $("#checkin").click(function (e) {
        e.preventDefault();
        dataString = $("form").serialize();

        $.ajax(
            {
                type: "POST", 
                url: "CheckIn",
                data: dataString,
                dataType: "json",
                success: function (data) {
                    LoadCheckInResult(data.error, data.errorColor);                   
                }             
            });
    });
});

function LoadCheckInResult(error, colorCode) {

    var $confirmation = $('#confirmation');
    var $colormsg = $('#colormsg');
    var $barcode = $('#barcode');
    $confirmation.empty();
    $confirmation.html(error);
    $('#lastbarcode').html('Last Barcode: ' + $barcode.val());
    $barcode.val("");
    $barcode.focus();

    if (colorCode == "green") {
        $colormsg.removeClass("checkInDuplicate");
        $colormsg.removeClass("checkInFailure");
        $colormsg.addClass("checkInSuccess");
    }
    else if (colorCode == "yellow") {
        $colormsg.removeClass("checkInSuccess");
        $colormsg.removeClass("checkInFailure");
        $colormsg.addClass("checkInDuplicate");
    }
    else
    {
        $colormsg.removeClass("checkInSuccess");
        $colormsg.addClass("checkInFailure");
        $colormsg.removeClass("checkInDuplicate");
    }

}