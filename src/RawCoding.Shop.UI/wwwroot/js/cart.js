var cart = new Vue({
    el: '#cart-app',
    data: {
        cart: {
            items: [],
            total: ""
        },
        open: false,
    },
    created() {
        this.loadCart()
    },
    methods: {
        addProduct(item) {
            this.open = true;
            return axios.post('/api/cart', item, {withCredentials: true})
                .then(res => {
                    // todo handle response
                }).catch(err => {
                    // todo handle error
                })
                .finally(() => this.loadCart())
        },
        loadCart() {
            return axios.get('/api/cart', {withCredentials: true})
                .then(res => this.cart = res.data)
        }
    },
    computed: {
        active() {
            return {'is-active': this.open}
        }
    }
})