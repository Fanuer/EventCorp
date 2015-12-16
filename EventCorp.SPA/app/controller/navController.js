function navController($scope, $location, authFactory, fileFactory) {
    var _logOut = function () {
        authFactory.logOut();
        $location.path('/login');
    }
    function _init() {
        fileFactory.updateAvatarUrl().then(function() {
            $scope.avatarSrc = fileFactory.avatarUrl;
        });
    }

    $scope.$watch('authentication.isAuth', function (newValue, oldValue) {
        if (newValue) {
            fileFactory.updateAvatarUrl().then(function () {
                $scope.avatarSrc = fileFactory.avatarUrl;
            });
        }
    });

    $scope.navbarExpanded = false;
    $scope.authentication = authFactory.authentication;
    $scope.logOut = _logOut;
    _init();
}