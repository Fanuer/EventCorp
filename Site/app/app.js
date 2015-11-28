var eventCorp = angular.module("eventCorp", ['ngRoute']);

//const
eventCorp.constant("localStorageAuthIndex", "eventCorp.SPA.auth");
eventCorp.constant("baseUrl", "http://eventcorp.azurewebsites.net/api/");

//config
eventCorp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorFactory');
});
eventCorp.config([
    "$routeProvider", function ($routeProvider) {
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

//init
eventCorp.run(['authFactory', function (authFactory) {
    // on start, look for available login data
    authFactory.fillAuthData();
}]);

//factories
fIT.factory("authFactory", authFactory);
fIT.factory("authInterceptorFactory", authInterceptorFactory);

//controller
eventCorp.controller("dashboardController", dashboardController);
eventCorp.controller("eventController", eventController);
eventCorp.controller("loginController", loginController);
eventCorp.controller("registerController", registerController);
eventCorp.controller("userController", userController);
