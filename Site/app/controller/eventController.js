function eventController($scope, $timeout, eventFactory, enumFactory) {
  var searchChanged = null;
  $scope.pageSizeOptions = [10, 25, 50, 100];
  $scope.tableOptions = {
    searchTerm: "",
    pageSize: 25,
    page: 0,
    onlyOpenEvents: true
  }
  $scope.$watch('tableOptions', function (newVal, oldVal) {
    if (oldVal.searchTerm !== newVal.searchTerm) {
      if (searchChanged) {
        $timeout.cancel(searchChanged);
      }
      searchChanged = $timeout(_updateEventList, 3000);
    } else {
      _updateEventList();
    }
  }, true);

  function _init() {
    _updateEventList();
  }
  function _getCssForEventType(value) {
    return enumFactory.getCssForType('eventTypes', value);
  }
  function _updateEventList() {
    eventFactory.getEvents($scope.tableOptions).then(function (response) {
      $scope.events = response.data;
      $scope.showLoader = false;
    });
    $scope.showLoader = true;
  }
  function _subscribe(event) {
    eventFactory.subscribe(event.Id).then(_updateEventList);
  }

  function _unsubscribe(event) {
    eventFactory.unsubscribe(event.Id).then(_updateEventList);
  }

  $scope.events = {};
  $scope.getCssForType = _getCssForEventType;
  $scope.subscribe = _subscribe;
  $scope.unsubscribe = _unsubscribe;
  _init();
}