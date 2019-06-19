$(document).ready(function () {
    $sms = $('#sms');
    $phone = $('#PhoneNumber');
    $phonediv = $('#phonediv');
    $validationConstant = $('#validationConstant').val();

    if ($sms.is(':checked')) {
        $phonediv.attr("style", "display:block");
    }

    $sms.change(function () {
        if ($sms.is(':checked')) {
            $phonediv.attr("style", "display:block");
            $phone.val("");
        }
        else if (!$sms.is(':checked')) {
            $phone.val($validationConstant);
            $phonediv.attr("style", "display:none");
        }
    });  
});
