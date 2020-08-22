var cartContainer = document.getElementById('cart-container')

var openCart = function (){
    cartContainer.classList.add('is-active')
}

var closeCart = function (){
    cartContainer.classList.remove('is-active')
}