var app = new Vue({
    el: '#app',
    data: {
        loading: false,
        products: [],
        stock: [],
        selectedProduct: null,
    },
    created() {
        return this.getProducts();
    },
    methods: {
        getProducts() {
            this.loading = true;
            return axios.get('/api/admin/products')
                .then(res => this.products = res.data)
                .catch(err => console.log(err))
                .finally(this.resetForm)
        },
        selectProduct(product) {
            this.selectedProduct = product;
            return axios.get('/api/admin/products/' + product.id + '/stocks')
                .then(res => this.stock = res.data)
                .catch(err => console.log(err))
                .finally(() => this.loading = false)
        },
        updateStock() {
            this.loading = true;
            return axios.put('/api/admin/products/' + this.selectedProduct.id + '/stocks', this.stock)
                .catch(err => console.log(err))
                .then(this.getProducts);
        },
        addStock() {
            this.stock.push({
                description: "",
                qty: 0,
                value: 0,
            })
        },
        resetForm() {
            this.loading = false;
            this.stock = [];
            this.selectedProduct = null;
        }
    }
})