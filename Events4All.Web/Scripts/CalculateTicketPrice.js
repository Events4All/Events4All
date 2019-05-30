$(function () {
    $('#NumberOfTickets').change(function () {
        var $tickets = $(this);
        var $price = $("#price");
        var $subtotal = $("#subtotal")
        var $validatr = $('form').data('validator');

        $validatr.element($tickets);
        CalculateTicketPrice($tickets.val(),$price.val(),$subtotal);
    });
});

function CalculateTicketPrice(ticketCount,price,subtotalEl) {
    var subtotal = ticketCount * price;
    subtotalEl.val(subtotal)
}