describe("Login", function () {
    var loginCtr;
    var scope;
    var location;
    var authFactory;

    beforeEach(angular.mock.module('eventCorp'));

    beforeEach(inject(function ($rootScope, $location, $authFactory) {
        scope = $rootScope.isolateScope();
        location = $location;
        authFactory = $authFactory;
    }));
/*
    beforeEach(function() {
        var injector = angular.injector(['ng']);

        var $scope = injector.isolateScope();
        var $location = injector.get("$location");
        var authFactory = injector.get("authFactory");

        loginCtr = new loginController($scope, $location, authFactory);
    });
*/
    afterEach(function() {
    });

    it("should throw an error message with wrong data", function () {
        loginCtr.$scope.loginData = "";
        loginCtr.login($scope.loginData).then(function() {
            //keine Aufgabe...
        }).catch(function(ex) {
        }).finally(function() {
            expect($scope.message.toBe("Error on logging in"));
            done();
        });
    });
})