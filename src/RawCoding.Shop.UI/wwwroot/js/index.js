const app = new Vue({
    el: '#app',
    data: {
        products: __state__
    },
    methods: {
        gotoProduct(p) {
            window.location.href = '/product/' + p.slug
        },
        buy(p) {

        }
    }
})