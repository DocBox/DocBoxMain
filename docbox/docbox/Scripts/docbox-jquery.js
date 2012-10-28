$(document).ready(function () {
    $('.date').datepicker({ dateFormat: "dd/mm/yy" });
});

jQuery(document).ready(function ($) {
    $('#radio_3').prop('disabled', true);
    $('#radio_2').prop('checked', true);
});

$('#callback-toggle-button').toggleButtons({
    label: {
        enabled: "It's Checked In!!",
        disabled: "Check it out!!"
    },
    onChange: function ($el, status, e) {
        $('#magic-text').text("Status is: " + status);
    }
});