const carouselItems = document.getElementsByClassName('carousel-item');
const carousel = document.getElementById('carousel');
const screen = document.getElementById('carousel-screen');
const toRight = 'to-right',
    toLeft = 'to-left',
    fromRight = 'from-right',
    fromLeft = 'from-left'

const goLeft = function () {
    resetAnimation()
    const current = carouselItems[1];
    addEvent(current, toRight)
    const next = carouselItems[0];
    addEvent(next, fromLeft)

    const last = carouselItems[carouselItems.length - 1];
    screen.removeChild(last);
    screen.prepend(last);
    moveDot(-1)
}

const goRight = function () {
    resetAnimation()
    const current = carouselItems[1];
    addEvent(current, toLeft)
    const next = carouselItems[2];
    addEvent(next, fromRight)

    const first = carouselItems[0];
    screen.removeChild(first);
    screen.appendChild(first);
    moveDot(1)
}

const resetAnimation = function () {
    for (let i = 0; i < carouselItems.length; i++) {
        carouselItems[i].classList.remove(toRight, toLeft, fromRight, fromLeft)
    }
}

const addEvent = function (element, animation) {
    element.classList.add(animation)
    element.addEventListener("animationend", function () {
        element.classList.remove(animation)
    }, false);
}

const dots = [];
const init = function () {
    const pager = document.createElement('div')
    pager.classList.add('carousel-pager')
    for (let i = 0; i < carouselItems.length; i++) {
        let el = document.createElement('div')
        dots.push(el)
        pager.append(el)
    }
    carousel.appendChild(pager)
    dots[0].classList.add('selected')

    if (carouselItems.length > 1) {
        const left = document.createElement('a')
        left.classList.add('nav-btn', 'left', 'is-size-2', 'is-bold', 'text-black')
        left.onclick = goLeft;
        carousel.append(left);

        const right = document.createElement('a')
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

const __selected__ = 'selected'
const moveDot = function (n) {
    let position = 0;
    for (let i = 0; i < dots.length; i++) {
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

