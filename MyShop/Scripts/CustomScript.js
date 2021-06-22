//Product Image
$('.thumbnails-image img').click(function () {
    var src = $(this).attr('src');
    $('.large-image img').attr('src', src).fadeOut('fast').fadeIn('fast');
});
//End

//Comment
function successComment() {
    $("#Email").val("");
    $("#Website").val("");
    $("#Comment").val("");
    $("#Name").val("");
    $("#ParentID").val("");

}
function ReplyComment(id) {
    $("#ParentID").val(id);
    $('html, body').animate({ scrollTop: $("#commentProduct").offset().top }, 600);
    //$('html, body').animate({ scrollTop: $("#commentProduct").offset().top - 70 };
}
//End Comment

//ShopCart
$(function () {
    CountShopCart();
});
function CountShopCart() {
    $.get("/Api/Shop", function (res) {
        $("#CountShopCart").html(res);
    });
}
function AddShopCart(id) {
    $.get("/Api/Shop/" + id, function (res) {
        $("#CountShopCart").html(res);
        UpdateShopCart();
    });
}
function UpdateShopCart() {
    $("#ShowCart").load("/ShopCart/ShowCart");
}
function DeleteCart(id) {
    

}
//End ShopCart

//Compare
function AddCompare(id) {
    $.get("/CompareList/AddToCompare/" + id, function (res) {
        $("#compare").html(res);
    });
}
function DeleteCompare(id) {
    $.get("/CompareList/DeleteCompare/" + id, function (res) {
        $("#compare").html(res);
    });
}
//End

