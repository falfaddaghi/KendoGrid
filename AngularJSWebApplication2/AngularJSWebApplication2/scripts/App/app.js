(function() {
    var app = angular.module("KendoDemos", ["kendo.directives", "ngRoute"]);
    app.config(function($routeProvider) {
        $routeProvider
            .when("/default", {
                templateUrl: "/views/gridTemplate.cshtml",
                controller: "defaultController"
            })
            .when("/grid", {
                templateUrl: "/views/subGridTemplate.cshtml",
                controller: "gridController"
            })
            .otherwise({ redirectTo: "/default" });

    });

}());