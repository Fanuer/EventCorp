function eventController($scope, $timeout, eventFactory, enumFactory) {
    var searchChanged = null;
    $scope.tableOptions = {
        searchTerm: "",
        pageSize: 25,
        page: 0,
        pageSizeOptions: [10, 25, 50, 100],
        onlyOpenEvents: true
    }

    function _init() {
        eventFactory.getEvents($scope.tableOptions).then(function (response) {
            $scope.events = response.data;
        });
    }
    function _getCssForEventType(value) {
        return enumFactory.getCssForType('eventTypes', value);
    }

    $scope.$watch('tableOptions', function (newVal, oldVal) {
        if (oldVal.searchTerm !== newVal.searchTerm) {
            if (searchChanged) {
                $timeout.cancel(searchChanged);
            }
            searchChanged = $timeout(function() {
                eventFactory.getEvents($scope.tableOptions).then(function (response) {
                    $scope.events = response.data;
                });

            }, 3000);
        } else {
            eventFactory.getEvents($scope.tableOptions).then(function (response) {
                $scope.events = response.data;
            });

        }
    }, true);
    $scope.events = {};
    $scope.getCssForType = _getCssForEventType;
    _init();
}