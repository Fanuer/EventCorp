function eventController($scope, $timeout, eventFactory, enumFactory) {
  var searchChanged = null;
  $scope.pagination = {};
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
  function _updatePagination(resultList, paginationSize) {
    paginationSize = paginationSize || 2;
    
    var lastPageCount = Math.floor(resultList.AllEntriesCount / resultList.PageSize);
    var currentIndex = resultList.Page;
    var pageIndexes = new Array();

    for (var i = resultList.Page - paginationSize; i <= resultList.Page + paginationSize; i++) {
      pageIndexes.push(i);
    }
    $scope.pagination = {
      lastPageCount,
      currentIndex,
      pageIndexes
    }
  }
  function _getCssForEventType(value) {
    return enumFactory.getCssForType('eventTypes', value);
  }
  function _updateEventList() {
    eventFactory.getEvents($scope.tableOptions).then(function (response) {
      $scope.events = response.data.Entries;
      $scope.showLoader = false;
      _updatePagination(response.data);
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