const url = '/api/admin/products';

const app = new Vue({
    el: '#app',
    data: () => ({
        form: {
            id: 0,
            name: "Product Name",
            description: "Product Description",
            value: 100,
            images: []
        },
        editing: false,
        loading: false,
        objectIndex: 0,
        products: []
    }),
    mounted() {
        this.getProducts();
    },
    methods: {
        addImage(event) {
            const file = event.target.files[0];
            this.form.images.push(file)
        },
        moveImageUp(index) {
            const image = this.form.images[index]
            this.form.images.splice(index, 1)
            this.form.images.splice(index - 1, 0, image)
        },
        moveImageDown(index) {
            const image = this.form.images[index]
            this.form.images.splice(index, 1)
            this.form.images.splice(index + 1, 0, image)
        },
        getProduct(id) {
            this.loading = true;
            axios.get(url + '/' + id)
                .then(res => {
                    console.log(res);
                    var product = res.data;
                    this.form = {
                        id: product.id,
                        name: product.name,
                        description: product.description,
                        value: product.value,
                    };
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        getProducts() {
            this.loading = true;
            axios.get(url)
                .then(res => {
                    console.log(res);
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        createProduct() {
            this.loading = true;
            const form = new FormData();

            form.append('form.name', this.form.name);
            form.append('form.description', this.form.description);
            form.append('form.value', this.form.value);

            for (let i = 0; i < this.form.images.length; i++) {
                form.append('form.images', this.form.images[i])
            }

            axios.post(url, form, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
                .then(res => {
                    console.log(res.data);
                    this.products.push(res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        updateProduct() {
            this.loading = true;
            axios.put(url, this.form)
                .then(res => {
                    console.log(res.data);
                    this.products.splice(this.objectIndex, 1, res.data);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                    this.editing = false;
                });
        },
        deleteProduct(id, index) {
            this.loading = true;
            axios.delete(+id)
                .then(res => {
                    console.log(res);
                    this.products.splice(index, 1);
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        },
        newProduct() {
            this.editing = true;
            this.form.id = 0;
        },
        editProduct(id, index) {
            this.objectIndex = index;
            this.getProduct(id);
            this.editing = true;
        },
        cancel() {
            this.editing = false;
        }
    },
    computed: {
        fileImages() {
            return this.form.images.map(x => URL.createObjectURL(x))
        }
    }
})