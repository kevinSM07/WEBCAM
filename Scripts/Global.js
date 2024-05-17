$(document).ready(function () {
    $('.alert').click(function () {
        $(this).remove();
    });
    $('.alert').mouseover(function () {
        $(this).remove();
    });
    $('.alert').ready(function () {
        setTimeout(function () {
            $(this).fadeOut(1500);
        }, 3000);

        setTimeout(function () {
            $(this).fadeIn(1500);
        }, 6000);
    });
});