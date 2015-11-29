function navController($scope, $location, authFactory) {
    var _logOut = function () {
        authFactory.logOut();
        $location.path('/');
    }

    $scope.navbarExpanded = false;
    $scope.authentication = authFactory.authentication;
    $scope.logOut = _logOut;
}