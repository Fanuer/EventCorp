var eventCorp = angular.module("eventCorp", ['ngRoute']);

eventCorp.config([
    "$routeProvider", function($routeProvider) {
        $routeProvider.when("/", {
            controller: "dashboardController",
            templateUrl: "app/views/dashboard.html"
        }).when("/events", {
            controller: "eventController",
            templateUrl: "app/views/events.html"
        }).when("/register", {
            controller: "registerController",
            templateUrl: "app/views/register.html"
        }).when("/user", {
            controller: "userController",
            templateUrl: "app/views/user.html"
        }).when("/login", {
            controller: "loginController",
            templateUrl: "app/views/login.html"
        }).otherwise("/");
    }
]);

eventCorp.controller("dashboardController", dashboardController);
eventCorp.controller("eventController", eventController);
eventCorp.controller("loginController", loginController);
eventCorp.controller("registerController", registerController);
eventCorp.controller("userController", userController);
