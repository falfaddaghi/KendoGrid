(function() {
    
        var app = angular.module("KendoDemos");
        var gridController = function($scope) {
            $scope.mainGridOptions = {
                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/SalesOrders/Grouped",
                            beforeSend: function(xhr) {
                                xhr.setRequestHeader('Accept', "application/json");
                            }
                        },
                        dataType: "JSON",
                        parameterMap: function(options, operation) {
                            return options;
                        }
                    },
                    pageSize: 10,
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: function(data) {
                            return data.results;
                        },
                        total: function(data) {
                            return data.total;
                        }
                    }
                }),
                detailTemplate: kendo.template($("#template").html()),
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                scrollable: true,
                groupable: true,
                sortable: true,
                resizable: true,
                filterable: true,
                columns: [
                    { field: "salesOrderID ", title: "Order Id" },
                ],

            };

            $scope.detailGridOptions = function(dataItem) {
                //dataItem.find(".tabstrip").kendoTabStrip({
                //    animation: {
                //        open: { effects: "fadeIn" }
                //    }
                //});
                var skip = 0;
                return {
                    dataSource: new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: "/SalesOrders/Details/?salesOrderID=" + dataItem.salesOrderID,
                                beforeSend: function(xhr) {
                                    xhr.setRequestHeader('Accept', "application/json");
                                }
                            },
                            dataType: "JSON",
                            parameterMap: function(options, operation) {
                                if (operation == 'read')
                                    options.skip = skip;
                                return options;
                            }
                        },
                        pageSize: 3,
                        serverPaging: true,
                        serverSorting: true,
                        serverFiltering: true,
                        schema: {
                            data: function(data) {
                                skip += 3;
                                $scope.sum = data.meta['Sum'];
                                return data.results;
                            },
                            total: function(data) {
                                return data.total;
                            }
                        }
                    }),

                    scrollable: {
                        virtual: true
                    },
                    groupable: true,
                    sortable: true,
                    resizable: true,
                    filterable: true,
                    columns: [
                        { field: "carrierTrackingNumber", title: "Tracking #" },
                        { field: "orderQty", title: "Quantity" },
                        { field: "productID", title: "Product Id" },
                        { field: "specialOfferID", title: "Special Offer Id" },
                        { field: "unitPrice", title: "Price" },
                        { field: "unitPriceDiscount", title: "Discount" },
                        { field: "lineTotal", title: "Line Total", footerTemplate: "Sum = {{sum}} " },
                        { field: "rowguid", hidden: true },
                        { field: "modifiedDate", title: " Date" }
                    ]
                };
            }

        }    
    app.controller("gridController", gridController);
}())