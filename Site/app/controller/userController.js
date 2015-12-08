function userController($scope, $http, $timeout, fileFactory, userFactory, dateFilter) {

  function _changePassword() {
    userFactory
      .changePassword($scope.changePasswordModel)
      .then(function () {
        $timeout(function () {
          $scope.message = "";
          $scope.messageClass = null;
        }, 5000);
        $scope.messageClass = "info";
        $scope.message = "Successfully changed password";
        $scope.changePasswordModel = new Object();
      }).catch(function (response) {
        $scope.message = "Error on changing password: " + response;
      });
  }

  function _updateUser() {
    userFactory
      .updateCurrentUser($scope.userData)
      .then(function () {
        $timeout(function () {
          $scope.message = "";
          $scope.messageClass = null;
        }, 5000);
        $scope.messageClass = "info";
        $scope.message = "Successfully changed user data";
      }).catch(function (response) {
        $scope.message = "Error on changing userdata: " + response;
      });
  }

  $scope.$watch('avatar', function (newValue, oldValue) {
    if (newValue !== oldValue) {
      fileFactory
          .upload(newValue)
          .then(function (response) {
            $scope.userData.avatarId = response.data.id;
            $scope.imageUrl = fileFactory.getFileContentUrl(response.data.id);
            $scope.showLoader = false;
          });
      $scope.showLoader = true;
    }
  });
  $scope.$watch('dateWrapper', function (date) {
    $scope.userData.dateOfBirth = dateFilter(dateWrapper, 'yyyy-MM-dd');
  });

  $scope.$watch('userData.dateOfBirth', function (dateOfBirth) {
    $scope.dateWrapper = new Date(dateOfBirth);
  });

  function _init() {
    userFactory
        .getUserData()
        .then(function (response) {
          if (typeof (response.data.dateOfBirth) !== "undefined") {
            response.data.dateOfBirth = new Date(response.data.dateOfBirth).toISOString().substring(0, 10);
          }
          $scope.userData = response.data;
          if (response.data.avatarId) {
            $scope.imageUrl = fileFactory.getFileContentUrl(response.data.avatarId);
          }
        }).catch(function () {

        });
  }

  $scope.dateWrapper = new Date();


  $scope.imageUrl = '/images/no_avatar.png';
  $scope.userData = {};
  $scope.showLoader = false;
  $scope.changePassword = _changePassword;
  $scope.updateUser = _updateUser;
  _init();
}