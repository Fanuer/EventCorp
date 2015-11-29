function dashboardController($scope, $location, authFactory) {
    function _init() {
        if (!authFactory.authentication.isAuth) {
            $location.path('/login');
        }
    }

    _init();
}