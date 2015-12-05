function userController($scope, imageFactory) {

    // set user picture, if user uploads a new one
    $scope.uploadedFile = function (element) {
        $scope.$apply(function ($scope) {
            $scope.files = element.files;
        });
    }
}