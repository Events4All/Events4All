$(document).ready(function () {
    $sms = $('#sms');
    $phone = $('#PhoneNumber');
    $phonediv = $('#phonediv');
   
    if ($sms.is(':checked')) {
        $phonediv.attr("style", "display:block");
    }

    if (!$sms.is(':checked')) {
        $phonediv.attr("style", "display:none");
        $phone.val("not_required");
    }

    $sms.change(function () {
        if ($sms.is(':checked')) {
            $phonediv.attr("style", "display:block");
            $phone.val("");
        }
        else if (!$sms.is(':checked')) {
            $phone.val("not_required");
            $phonediv.attr("style", "display:none");
        }
    });  
});
