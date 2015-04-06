(function() {
    var skip = 0;
    var app = angular.module("KendoDemos");
    var defaultController = function ($scope) {
        $scope.mainGridOptions = {

                dataSource: new kendo.data.DataSource({
                    transport: {
                        read: {
                           
                        async: false,
                        url: "/SalesOrders",
                        beforeSend: function (xhr) {
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
                pageSize: 10,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                aggregates: [
                    { field: "LineTotal", aggregate: "sum" }
                ],
                serverAggregates: true,
                schema: {
                    data: function(data) {
                        return data.results;
                    },
                    groups: function(response) {
                        skip += 10;
                        return response.groups;
                    },
                    aggregates: function(response) {
                        return response.aggregates;
                    },
                    total: function(data) {
                        return data.total;
                    },
                },
                group: {
                    field: "salesOrderID",
                    aggregates: [
                        { field: "LineTotal", aggregate: "sum" }
                    ]
                }
            }),
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
                { field: "salesOrderDetailID", hidden: true },
                { field: "carrierTrackingNumber", title: "Tracking #" },
                { field: "orderQty", title: "Quantity" },
                { field: "productID", title: "Product Id" },
                { field: "specialOfferID", title: "Special Offer Id" },
                { field: "unitPrice", title: "Price" },
                { field: "unitPriceDiscount", title: "Discount" },
                { field: "lineTotal", title: "Line Total" },
                {
                    field: "lineTotal",
                    title: "Total",
                    aggregates: ["sum"],
                    type: "decimal",
                    groupFooterTemplate: "Sum:#= sum #"
                },
                { field: "rowguid", hidden: true },
                { field: "modifiedDate", title: " Date" }
            ],
            dataBinding: function() { $("#grid").find(".k-icon.k-collapse").trigger("click"); },
            dataBound: function(e) {
                var grid = $("#grid").data("kendoGrid");
                if (this.dataSource.group().length > 0) {
                    var group = $(".k-grouping-row");
                    $.each(group, function(inx, elem) {
                        grid.collapseGroup(elem);
                    });


                }
            },

        };
    };
   
    app.controller("defaultController", defaultController);
}());