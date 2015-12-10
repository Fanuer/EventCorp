function eventController($scope, eventFactory, enumFactory) {
    var eventTypes = {}

    function _init() {
        eventFactory.getEvents(50, 0, false, false).then(function (response) {
            $scope.events = response.data;
        });
        enumFactory.getEventTypes().then(function (response) {
            if (response.data.length >= 4) {
                response.data[0].css = "glyphicon-music"; //Cultural
                response.data[1].css = "glyphicon-modal-window"; //Sports
                response.data[2].css = "glyphicon-education"; //Education
                response.data[3].css = "glyphicon-flag"; //Other
            }
            eventTypes = response.data;
        });
    }
    function _getCssForType(type) {
        var result = null;
        for (var i = 0; i < eventTypes.length; i++) {
            if (eventTypes[i].value === type || eventTypes[i].displayName === type) {
                result = eventTypes[i].css;
                break;
            }
        }
        return result;
    }

    $scope.events = {};
    $scope.getCssForType = _getCssForType;
    _init();
}