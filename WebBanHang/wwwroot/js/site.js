document.addEventListener('DOMContentLoaded', function () {
    var decreaseBtn = document.querySelector('.decrease-btn');
    var increaseBtn = document.querySelector('.increase-btn');
    var quantityInput = document.getElementById('quantityInput');

    decreaseBtn.addEventListener('click', function () {
        decreaseQuantity();
    });

    increaseBtn.addEventListener('click', function () {
        increaseQuantity();
    });

    function decreaseQuantity() {
        var currentQuantity = parseInt(quantityInput.value);

        if (currentQuantity > 1) {
            quantityInput.value = currentQuantity - 1;
        }
    }

    function increaseQuantity() {
        var currentQuantity = parseInt(quantityInput.value);

        if (currentQuantity < 10) {
            quantityInput.value = currentQuantity + 1;
        }
    }
});