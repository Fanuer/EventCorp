function registerController($scope, $location, authFactory, userFactory) {
  function _init() {
    userFactory.getGender()
      .then(function (response) {
        $scope.enums.genders = response.data;
        $scope.registerData.genderType = response.data[0].displayName;
      });
    userFactory.getEventTypes()
      .then(function (response) {
        $scope.enums.eventTypes = response.data;
        $scope.registerData.favoriteEventType = response.data[0].displayName;
      });
  }
  function _register() {
    authFactory
      .register($scope.registerData)
      .then(function (response) {
        return authFactory.login({ userName: $scope.username, password: $scope.password });
      }).then(function () {
        $location.path('/');
      }).catch(function(response) {
        $scope.message = "Registration failed: " + response.message;
        $scope.messageClass = "danger";
      });

    $scope.message = "Registration in process";
    $scope.messageClass = "info";
  }

  $scope.registerData = {
    dateOfBirth: new Date()
  };
  $scope.enums = {};
  $scope.register = _register;
  _init();
}