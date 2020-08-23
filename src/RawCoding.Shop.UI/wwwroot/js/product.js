var carouselItems = document.getElementsByClassName('carousel-item');
var carousel = document.getElementById('carousel');
var screen = document.getElementById('carousel-screen');
var toRight = 'to-right',
    toLeft = 'to-left',
    fromRight = 'from-right',
    fromLeft = 'from-left'

var goLeft = function () {
    resetAnimation()
    var current = carouselItems[1];
    addEvent(current, toRight)
    var next = carouselItems[0];
    addEvent(next, fromLeft)

    var last = carouselItems[carouselItems.length - 1];
    screen.removeChild(last);
    screen.prepend(last);
    moveDot(-1)
}

var goRight = function () {
    resetAnimation()
    var current = carouselItems[1];
    addEvent(current, toLeft)
    var next = carouselItems[2];
    addEvent(next, fromRight)

    var first = carouselItems[0];
    screen.removeChild(first);
    screen.appendChild(first);
    moveDot(1)
}

var resetAnimation = function () {
    for (var i = 0; i < carouselItems.length; i++) {
        carouselItems[i].classList.remove(toRight, toLeft, fromRight, fromLeft)
    }
}

var addEvent = function (element, animation) {
    element.classList.add(animation)
    element.addEventListener("animationend", function () {
        element.classList.remove(animation)
    }, false);
}

var dots = [];
var init = function () {
    var pager = document.createElement('div')
    pager.classList.add('carousel-pager')
    for (var i = 0; i < carouselItems.length; i++) {
        var el = document.createElement('div')
        dots.push(el)
        pager.append(el)
    }
    carousel.appendChild(pager)
    dots[0].classList.add('selected')

    if (carouselItems.length > 1) {
        var left = document.createElement('a')
        left.classList.add('nav-btn', 'left', 'is-size-2', 'is-bold', 'text-black')
        left.onclick = goLeft;
        carousel.append(left);

        var right = document.createElement('a')
        right.classList.add('nav-btn', 'right', 'is-size-2', 'is-bold', 'text-black')
        right.onclick = goRight;
        carousel.append(right);
    } else {
        carouselItems[0].classList.add('only')
    }

    if (carouselItems.length === 2) {
        screen.appendChild(carouselItems[0].cloneNode(true));
        screen.appendChild(carouselItems[1].cloneNode(true));
    }
}
init()

var __selected__ = 'selected'
var moveDot = function (n) {
    var position = 0;
    for (var i = 0; i < dots.length; i++) {
        if (dots[i].classList.contains(__selected__)) {
            dots[i].classList.remove(__selected__)
            position = i;
            break;
        }
    }

    position += n;
    if (position < 0) position = dots.length - 1;
    else if (position >= dots.length) position = 0;

    dots[position].classList.add(__selected__)
}

var addToCart = function () {
    cart.addProduct({
        qty: document.getElementById('Input_Qty').value | 0,
        stockId: document.getElementById('Input_StockId').value | 0,
    })
}
