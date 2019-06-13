$('document').ready(function () {
    var $form = $('form')
    $form.validate();
    $validatr = $form.data('validator');
    $settngs = $validatr.settings;
    $settngs.errorClass = "text-danger field-validation-error";
    $settngs.errorElement = "span";
    

    $('#NumberOfTickets').change(function () {
        var $tickets = $(this);
        var $price = $("#price");
        var $subtotal = $("#subtotal")
        $('span#NumberOfTickets-error').hide();
        $validatr.element($tickets);
        CalculateTicketPrice($tickets.val(),$price.val(),$subtotal);
    });

    $('#NumberOfTickets').focusout(function () {
        $('span#NumberOfTickets-error').hide();
    });
});

function CalculateTicketPrice(ticketCount,price,subtotalEl) {
    var subtotal = ticketCount * price;
    subtotalEl.val(subtotal)
}