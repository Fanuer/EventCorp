function adminController($scope, enumFactory, eventAdminFactory) {

    var initObject = {
        name: '',
        place: '',
        type: 'Cultural',
        maxUsers: '',
        startUtc: new Date(Date.now()),
        description: ''
    }

    function init() {
        $scope.createEvent = _createEvent;
        $scope.eventData = angular.copy(initObject);
        $scope.eventData.startUtc = new Date(Date.now());
        enumFactory.getEventTypes().then(function (response) {
            $scope.eventData.type = response.data[0].displayName;
            $scope.eventTypes = response.data;
        });

    }
    function _createEvent() {
        eventAdminFactory.create($scope.eventData)
            .then(function () {
                $scope.message = "Event erfolgreich angelegt";
                $scope.messageClass = "info";
                $scope.createEventForm.$setPristine();
                $scope.createEventForm.$setUntouched();
                $scope.eventData = angular.copy(initObject);
            }).catch(function (response) {
                $scope.message = "Event konnte nicht angelegt werden: " + response.data.Message;
                $scope.messageClass = "danger";
            });
    }


    init();
}