// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(function () {
//    $('.menulink').click(function () {
//        $("#bg").attr('src', "img/picture1.jpg");
//        return false;
//    });
//});

$(function () {
    $('.thumb-item').click(function () {
        //alert('asd')
        let src = $(this).children('img').attr('src');
        ChangePicture(src)
    });
});

function Test() {
    alert('asd')
}

function ChangePicture(src) {
    $('#main-product-img').attr('src', src)
}