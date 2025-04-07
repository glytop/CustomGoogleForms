$(document).ready(function () {
    $('#selectAll').click(function () {
        $('.user-select').prop('checked', this.checked);
    });

    $("#filterInput").on("keyup", function () {
        const value = $(this).val().toLowerCase();
        $("tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    $("tr:has(.timestamp-popup)").hover(function () {
        $(this).find(".timestamp-popup").addClass("show");
    }, function () {
        $(this).find(".timestamp-popup").removeClass("show");
    });
});