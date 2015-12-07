function userController($scope, $http, fileFactory, userFactory) {
    $scope.imageUrl = '';
    $scope.userData = {};
    $scope.showLoader = false;

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

    function _init() {
        userFactory
            .getUserData()
            .then(function(response) {
                $scope.userData = response.data;
                if (response.data.avatarId) {
                    $scope.imageUrl = fileFactory.getFileContentUrl(response.data.avatarId);
                }
            }).catch(function() {
                
            });
    }
    _init();
}