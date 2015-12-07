function userFactory($http, authbaseUrl) {
    function _getUserData(key) {
        var result = null;

        if (typeof (key) == "string" || typeof (key) == "number") {
            result = $http.get(authbaseUrl + 'Accounts/User/' + key);
        }
         else {
            result = $http.get(authbaseUrl + 'Accounts/CurrentUser');
        }
        return result;
    }

    function _updateOwnData(userData) {
        return $http.put(authbaseUrl + 'Accounts/CurrentUser', userData);
    }

    var factory = {
        getUserData: _getUserData,
        updateCurrentUser: _updateOwnData
    };
    return factory;
}