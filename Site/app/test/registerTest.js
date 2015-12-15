describe("Register", function () {
    var registerCtr;

    beforeEach(function () {
        var injector = angular.injector(['ng']);

        var $scope = injector.get("$scope");
        var $location = injector.get("$location");
        var authFactory = injector.get("authFactory");
        var userFactory = injector.get("userFactory");

        registerCtr = new registerController($scope, $location, authFactory, userFactory);
    });

    afterEach(function () {
    });

    it("should throw an error message with wrong data", function () {
        loginCtr.$scope.loginData = "";
        loginCtr.login($scope.loginData).then(function () {
            //keine Aufgabe...
        }).catch(function (ex) {
        }).finally(function () {
            expect($scope.message.toBe("Error on logging in"));
            done();
        });
    });
})