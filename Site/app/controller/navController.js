function navController($scope, $location, authFactory, fileFactory) {
  var _logOut = function () {
    authFactory.logOut();
    $location.path('/');
  }
  function _init() {
    fileFactory
      .getUserAvatar()
      .then(function (response) {
        $scope.avatarSrc = response;
      });
  }

  $scope.navbarExpanded = false;
  $scope.authentication = authFactory.authentication;
  $scope.logOut = _logOut;
  _init();
}