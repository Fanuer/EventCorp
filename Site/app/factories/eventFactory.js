function eventFactory($http, eventbaseUrl) {

    function _getEvents(pageSize, page, onlySubscriped, onlyOpenEvents, searchTerm) {
        return $http.get(eventbaseUrl, { 
            params: {
                pageSize, 
                page, 
                onlySubscriped, 
                onlyOpenEvents, 
                searchTerm
            } });
    }

    function _subscribe(eventId) {
        return $http.post(eventbaseUrl + eventId + "/subscribe");
    }

    function _unsubscribe(eventId) {
        return $http.put(eventbaseUrl + eventId + "/unsubscribe");
    }

    var factory = {
        getEvents: _getEvents,
        subscribe: _subscribe,
        unsubscribe: _unsubscribe
    };
    return factory;
}