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
  function _changePassword(changePasswordModel) {
    return $http.put(authbaseUrl + "accounts/changepassword", changePasswordModel);
  }
  function _getGenders() {
    return $http.get(authbaseUrl + 'enums/gender');
  }
  function _getEventTypes() {
    return $http.get(authbaseUrl + 'enums/events');
  }


  var factory = {
    getUserData: _getUserData,
    updateCurrentUser: _updateOwnData,
    changePassword: _changePassword,
    getGender: _getGenders,
    getEventTypes: _getEventTypes
  };
  return factory;
}