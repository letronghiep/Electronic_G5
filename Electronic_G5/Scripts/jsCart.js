$(document).ready(function () {
    $('.product-option').click(function () {
        // Remove active class from all options
        $('.product-option').removeClass('active');
        // Add active class to the clicked option
        $(this).addClass('active');
        // Update the product price
        var optionPrice = $(this).data('price');
        $('#productPrice').text(optionPrice);
    });

    $('.gallery_item').click(function () {
        var largeImageUrl = $(this).data('img');
        $('#mainImage').attr('src', largeImageUrl);
    });
    $('.p-plus').click(function () {
        var $input = $(this).closest('.input-group').find('.quantity');
        var val = parseInt($input.val());
        $input.val(val + 1);
    });
    $('.p-minus').click(function () {
        var $input = $(this).closest('.input-group').find('.quantity');
        var val = parseInt($input.val());
        if (val > 1) {
            $input.val(val - 1);
        }
    });
    $(".add-to-cart").click(function () {
        var productId = parseInt($("#sp_ma").val());
        var quantity = parseInt($(".quantity").val());
        var price = $("#productPrice").text();
        console.log("Pice", price)
        $.ajax({
            url: '/Carts/AddToCart',
            type: 'POST',
            data: {
                product_id: productId,
                quantity: quantity,
                price: parseInt(price.replace(/\./g, ''), 10)
            },
            //console.log("P", product)
            success: function (response) {
                if (response.success) {
                    alert('Sản phẩm đã được thêm vào giỏ hàng!');

                } else {
                    alert(response.message);
                    if (response.message === "Vui long dang nhap.") {
                        window.location.href = '/Users/Login'; // Adjust to your login URL
                    }
                }
            },
            error: function (err) {
                alert('Có lỗi xảy ra, vui lòng thử lại.');
                console.log(err)
            }
        })
    });
});
