function enumFactory($http, authbaseUrl) {
    function _getGenderTypes() {
        return $http.get(authbaseUrl + "enums/gender");
    }

    function _getEventTypes() {
        return $http.get(authbaseUrl + "enums/events");
    }
    var factory = {
        getEventTypes: _getEventTypes,
        getGenderTypes: _getGenderTypes
    }

    return factory;
}