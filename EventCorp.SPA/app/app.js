var eventCorp = angular.module("eventCorp", ['ngRoute', "LocalStorageModule"]);

//const
eventCorp.constant("localStorageAuthIndex", "eventCorp.SPA.auth");
//eventCorp.constant("authbaseUrl", "http://localhost:54867/api/");
//eventCorp.constant("eventbaseUrl", "http://localhost:55811/api/events/");
eventCorp.constant("eventbaseUrl", "http://ecevent.azurewebsites.net/api/events/");
eventCorp.constant("authbaseUrl", "http://ec-auth.azurewebsites.net/api/");

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
            templateUrl: "app/views/events.html",
            resolve: {
            'init': function(enumFactory) {
              return enumFactory.initPromise;
            }
          }
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
eventCorp.factory("authFactory", authFactory);
eventCorp.factory("eventAdminFactory", eventAdminFactory);
eventCorp.factory("eventFactory", eventFactory);
eventCorp.factory("userFactory", userFactory);
eventCorp.factory("fileFactory", fileFactory);
eventCorp.factory("authInterceptorFactory", authInterceptorFactory);
eventCorp.factory("enumFactory", enumFactory);

//controller
eventCorp.controller("dashboardController", dashboardController);
eventCorp.controller("eventController", eventController);
eventCorp.controller("loginController", loginController);
eventCorp.controller("registerController", registerController);
eventCorp.controller("userController", userController);
eventCorp.controller("navController", navController);

//directives
eventCorp.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);
eventCorp.directive('showtab', showtab);
eventCorp.directive('showAvatarUpload', showAvatarUpload);
eventCorp.directive('compareTo', compareTo);
eventCorp.directive('bootstrapSelect', bootstrapSelect);
