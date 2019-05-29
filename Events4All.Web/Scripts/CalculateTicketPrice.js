$(function () {
    $('#NumberOfTickets').change(function () {
        var $tickets = $(this);
        var $price = $("#price");
        var $subtotal = $("#subtotal")
        CalculateTicketPrice($tickets.val(),$price.val(),$subtotal);
    });
});

function CalculateTicketPrice(ticketCount,price,subtotalEl) {
    var subtotal = ticketCount * price;
    subtotalEl.val(subtotal)
}