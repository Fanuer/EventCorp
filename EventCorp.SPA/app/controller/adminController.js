function adminController($scope, $q, enumFactory, eventAdminFactory, statisticsFactory) {

  var initObject = {
    name: '',
    place: '',
    type: 'Cultural',
    maxUsers: '',
    startUtc: new Date(Date.now()),
    description: ''
  }

  function init() {
    $scope.isLoading = true;
    $scope.createEvent = _createEvent;
    $scope.eventData = angular.copy(initObject);
    $scope.eventData.startUtc = new Date(Date.now());
    var eventProm = enumFactory.getEventTypes().then(function (response) {
      $scope.eventData.type = response.data[0].displayName;
      $scope.eventTypes = response.data;
    });

    var statisticsProm = statisticsFactory.getStatistics().then(function (response) {
      $scope.statisticsData = response.data;
    });

    $q.all(eventProm, statisticsProm).then(function() {
      $scope.isLoading = false;
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