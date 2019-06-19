$('document').ready(function () {
    var $tickets = $('#NumberOfTickets');
    var $ticketsAvailable = $("#ticketsAvailable");
    $("#RemainingTickets").val($ticketsAvailable.val() - $tickets.val());

    $tickets.change(function () {      
        var $price = $("#price");
        var $subtotal = $("#subtotal")       
        $("#RemainingTickets").val($ticketsAvailable.val() - $tickets.val());
        CalculateTicketPrice($tickets.val(),$price.val(),$subtotal);
    });
});

function CalculateTicketPrice(ticketCount,price,subtotalEl) {
    var subtotal = ticketCount * price;
    subtotalEl.val(subtotal)
}