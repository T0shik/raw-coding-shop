var cart = new Vue({
    el: '#cart-app',
    data: {
        cart: {
            items: [],
            total: ""
        },
        open: false,
        popup: {
            active: false,
            message: "",
            success: false,
            timeout: null
        },
        loading: false
    },
    created() {
        this.loadCart()
    },
    watch: {
        open: function (v) {
            document.getElementsByTagName('html')[0].style.overflowY = v ? 'hidden' : 'scroll';
        }
    },
    methods: {
        addProduct(item) {
            return this.actionWrap(() => axios.post('/api/cart', item, {withCredentials: true}))
        },
        removeProduct(item) {
            return this.actionWrap(() => axios.delete('/api/cart/' + item.stockId, null, {withCredentials: true}))
        },
        loadCart() {
            return axios.get('/api/cart', {withCredentials: true})
                .then(res => this.cart = res.data)
        },
        setPopup(response) {
            this.popup.active = true;
            this.popup.message = response.data;
            this.popup.success = response.status === 200;
            this.popup.timeout = setTimeout(function () {
                this.popup.active = false
            }.bind(this), 1200);
        },
        actionWrap(action) {
            if (this.loading) return;
            this.loading = true;
            this.popup.active = false;
            if (this.popup.timeout) {
                clearTimeout(this.popup.timeout)
            }

            action().then(res => this.setPopup(res))
                .catch(err => this.setPopup(err.response))
                .finally(() => {
                    this.loadCart();
                    this.loading = false;
                })
        },
        gotoCheckout() {
            if (this.disabled) return;
            window.location = '/checkout'
        }
    },
    computed: {
        active() {
            return {'is-active': this.open}
        },
        popupStyle() {
            if (this.popup.success)
                return 'has-text-success'
            else
                return 'has-text-danger'

        },
        totalCount() {
            return this.cart.items.reduce((a, c) => a.qty + c.qty)
        },
        disabled() {
            return this.loading || this.cart.items.length === 0
        }
    }
})