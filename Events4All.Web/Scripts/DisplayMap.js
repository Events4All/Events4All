
function init_map() {

    var address = $("#number").text() + $("#street").text();
    address = address.replace("Number", "").replace(":", "").replace("Street", "");
    address = address.replace(":", "");

    var geocoder = new google.maps.Geocoder();
    var latitude = 52.3800447;          //Set default value in case address is null.
    var longitude = 9.728811599999972;  //Set default value in case address is null.

    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            latitude = results[0].geometry.location.lat();
            longitude = results[0].geometry.location.lng();

            var myOptions = {
                zoom: 12,
                center: new google.maps.LatLng(latitude, longitude), mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            map = new google.maps.Map(document.getElementById('gmap_canvas'), myOptions);
            marker = new google.maps.Marker({ map: map, position: new google.maps.LatLng(latitude, longitude) });


            infowindow = new google.maps.InfoWindow({ content: '<strong>' + address + '</br>' });

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });

            infowindow.open(map, marker);
        }
    });
}

google.maps.event.addDomListener(window, 'load', init_map);
