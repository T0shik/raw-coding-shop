var app = new Vue({
    el: '#app',
    data: {
        status: 0,
        loading: false,
        orders: [],
        order: null
    },
    watch: {
        status: {
            handler: function () {
                this.getOrders();
            },
            immediate: true
        }
    },
    methods: {
        getOrders() {
            this.loading = true;
            return axios.get('/api/admin/orders?status=' + this.status)
                .then(result => this.orders = result.data)
                .then(this.reset);
        },
        selectOrder(id) {
            this.loading = true;
            return axios.get('/api/admin/orders/' + id)
                .then(result => {
                    this.order = result.data;
                    this.loading = false;
                });
        },
        updateOrder() {
            this.loading = true;
            return axios.put('/api/admin/orders/' + this.order.id, null)
                .then(this.getOrders)
        },
        reset() {
            this.order = null;
            this.loading = false;
        }
    }
});