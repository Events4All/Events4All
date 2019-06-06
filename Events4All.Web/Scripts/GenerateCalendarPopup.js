$(document).ready(function () {
    $("#result").dialog({
        autoOpen: false,
        width: 400,
        height: 'auto',
        modal: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }       
    });

    
});

function openPopup() {
    
    $("#result").dialog("open");

    $('#close1').click(function () {
        $("#result").dialog("close");
    });

    $('#close2').click(function () {
        $("#result").dialog("close");
    });
}

function Download(url) {
    $('#wait-animation').show();
    window.location.href = url;
    $('#wait-animation').hide(0);
    $("#result").dialog("close");
}
    