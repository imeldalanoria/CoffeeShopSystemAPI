var ViewModel = function () {
    var self = this;
    self.offices = ko.observableArray();
    self.error = ko.observable();

    var officesUri = '/api/offices/';


    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllOffices() {
        ajaxHelper(officesUri, 'GET').done(function (data) {
            self.offices(data);
        });
    }

    // Fetch the initial data.
    getAllOffices();

    self.details = ko.observable();

    self.getOfficeDetail = function (item) {
       // this.lblOfficeName(this.OfficeName);
        ajaxHelper(officesUri + item.OfficeID, 'GET').done(function (data) {
            self.details(data);
            createProductChart(data);
        });
    }

    function createProductChart(data) {
        var CoffeeBeans = 0;
        var Sugar = 0;
        var Milk = 0;
        $.each(data, function (index, value) {
            if (value.ProductName == "Coffee Beans") {
                CoffeeBeans = value.Unit;
            }
            else if (value.ProductName == "Sugar") {
                Sugar = value.Unit;
            }
            else if (value.ProductName == "Milk") {
                Milk = value.Unit;
            }
        });

        var ctx = document.getElementById('myChart').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: ["Coffee Beans", "Sugar", "Milk"],
                datasets: [{
                    label: "Items",
                    backgroundColor: ["rgba(128, 128, 128, 1)", "rgba(222, 163, 104, 1)", "rgba(237, 236, 227, 1)"],
                    data: [CoffeeBeans, Sugar, Milk],
                }]
            },
            options: {
                scales: {
                    xAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }],
                    //yAxes: [{
                    //    barPercentage: 0.5
                    //}]
                }
            }
        });
    }

    self.products = ko.observableArray();
    self.newProduct = {
        ProductName: ko.observable(),
        Unit: ko.observable(),
        ClientName: ko.observable(),
        Office: ko.observable()
    }

    var productsUri = '/api/products/';
    var ordersUri = '/api/orders/';



    self.updateProducts = function (data, event) {
        var product = {
            ProductName: self.newProduct.ProductName(),
            Unit: self.newProduct.Unit(),
            ClientName: self.newProduct.ClientName(),
            OfficeID: self.newProduct.Office().OfficeID,
            OrderName: data
        };
        ajaxHelper(ordersUri + product.OfficeID, 'PUT', product).done(function (data) {
            self.products.push(data);
            getOrderDetail(product.OfficeID);
        });
    }

    self.orders = ko.observableArray();

    function getOrderDetail(item) {
        ajaxHelper(ordersUri + item, 'GET').done(function (data) {
            self.orders(data);
            createOrderChart(data);
            //createProductChart(data);
        });
    }

    function createOrderChart(data) {
        var DoubleAmericano = 0;
        var FlatWhite = 0;
        var SweetLatte = 0;
        $.each(data, function (index, value) {
            if (value.OrderName == "Double Americano") {
                DoubleAmericano +=1;
            }
            else if (value.OrderName == "Flat White") {
                FlatWhite += 1;
            }
            else if (value.OrderName == "Sweet Latte") {
                SweetLatte += 1;
            }
        });

        var ctx = document.getElementById('myOrderChart').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'horizontalBar',
            data: {
                labels: ["Double Americano", "Sweet Latte", "Flat White"],
                datasets: [{
                    label: "Order Items",
                    backgroundColor: ["rgba(128, 128, 128, 1)", "rgba(222, 163, 104, 1)", "rgba(237, 236, 227, 1)"],
                    data: [DoubleAmericano, SweetLatte, FlatWhite],
                }]
            },
            options: {
                scales: {
                    xAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }],
                    //yAxes: [{
                    //    barPercentage: 0.5
                    //}]
                }
            }
        });
    }

    var ProductModel = function (product) {
        var self = this;
        var id;
        var OfficeName;
        var PantryName;
        self.product = ko.observableArray(product);

        self.addProduct = function (item) {
            id = item.OfficeID;
            OfficeName = item.OfficeName;
            PantryName = item.PantryName;
            self.product.push({
                ProductName: "",
                Unit: ""
            });
        };

        self.removeProduct = function (item) {
            self.product.remove(item);
        };

        self.createProduct = function (formElement) {
            var prod = ko.toJS(self.product);

            $.each(prod, function (index, value) {
                var prodModel = {
                    OfficeID: id,
                    OfficeName: OfficeName,
                    PantryName: PantryName,

                    ProductName: value.ProductName,
                    Unit: value.Unit
                };
                ajaxHelper(productsUri, 'POST', prodModel).done(function (item) {
                    self.products.push(item);
                });
            });
        }
    };
    ProductModel();

    var OfficeModel = function (office) {
        var self = this;
        self.office = ko.observableArray(office);

        self.newOffice = {
            OfficeName: ko.observable(),
            PantryName: ko.observable()
        }

        self.addOffice = function (formElement) {
            var office = {
                OfficeName: self.newOffice.OfficeName(),
                PantryName: self.newOffice.PantryName()
            };
            ajaxHelper(officesUri, 'POST', office).done(function (item) {
                self.offices.push(item);
            });
        }
    };
    OfficeModel();

    function getOffices() {
        ajaxHelper(officesUri, 'GET').done(function (data) {
            self.offices(data);
        });
    }
    getOffices();
};
ko.applyBindings(new ViewModel());
