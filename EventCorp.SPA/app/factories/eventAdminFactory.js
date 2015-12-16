function eventAdminFactory($http, eventbaseUrl) {
    function _createEvent(data) {
        return $http.post(eventbaseUrl,data);
    }

    function _updateEvent(id, data) {
        return $http.post(eventbaseUrl + id, data);
    }

    function _deleteEvent(id) {
        return $http.delete(eventbaseUrl + id);
    }

    var factory = {
        create: _createEvent,
        update: _updateEvent,
        'delete': _deleteEvent
    };

    return factory;
}