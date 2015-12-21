function dashboardController($scope, $location, authFactory, eventFactory, enumFactory, recommendationFactory) {
  $scope.myEventWeek = {};
  $scope.recommendations = {};

  function _init() {
    if (!authFactory.authentication.isAuth) {
      $location.path('/login');
    }

    var untilDate = new Date();
    untilDate.setDate(untilDate.getDate() + 30);
    eventFactory.getEvents({
      tilDate: untilDate,
      onlySubscriped: true,
      onlyOpenEvents: true
    }).then(function (response) {
      $scope.myEvents = response.data.Entries;
    });

    recommendationFactory.getRecommendations().then(function(response) {
      $scope.recommendations = response.data.Entries;
    });
  }

  $scope.getCssForType = function (value) {
    return enumFactory.getCssForType('eventTypes', value);
  }
  ;
  _init();
}