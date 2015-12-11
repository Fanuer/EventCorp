function eventFactory($http, eventbaseUrl) {

    function _getEvents(options) {
        //possible options: pageSize, page, onlySubscriped, onlyOpenEvents, searchTerm
        return $http.get(eventbaseUrl, { 
            params: options
        });
    }

    function _subscribe(eventId) {
        return $http.post(eventbaseUrl + eventId + "/subscribe");
    }

    function _unsubscribe(eventId) {
        return $http.delete(eventbaseUrl + eventId + "/unsubscribe");
    }

    var factory = {
        getEvents: _getEvents,
        subscribe: _subscribe,
        unsubscribe: _unsubscribe
    };
    return factory;
}