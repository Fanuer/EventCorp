﻿'use strict';

function authFactory($http, $q, $log, localStorageService, authbaseUrl, localStorageAuthIndex) {
    var factory = {};

    function AuthModel(isAuth, userName, userId, isAdmin) {
        this.isAuth = isAuth || false;
        this.userName = userName || "";
        this.userId = userId || "";
        this.isAdmin = isAdmin || false;
    }

    var updateAuthData = function (newAuthdata) {
        var defaultAuth = new AuthModel();
        newAuthdata = newAuthdata || defaultAuth;
        factory.authentication.isAuth = newAuthdata.isAuth || defaultAuth.isAuth;
        factory.authentication.userName = newAuthdata.userName || defaultAuth.userName;
        factory.authentication.userId = newAuthdata.userId || defaultAuth.userId;
        factory.authentication.isAdmin = newAuthdata.isAdmin || defaultAuth.isAdmin;
    }
    var _logOut = function () {
        localStorageService.remove(localStorageAuthIndex);
        updateAuthData();
    }
    var _register = function (registrationModel) {
        var result = $q.defer();
        _logOut();
        $http.post(authbaseUrl + 'accounts/users', registrationModel)
        .then(function (response) {
            result.resolve(response);
        })
        .catch(function (response) {
            $log.error('Error: ' + response.statusCode);
            result.reject(response);
        });
        return result;
    }
    var _login = function (loginData) {
        var data = "grant_type=password&client_id=0dd23c1d3ea848a2943fa8a250e0b2ad&username=" + loginData.userName + "&password=" + loginData.password;
        var deferred = $q.defer();
        $http.post(authbaseUrl.replace('/api', '') + 'oauth2/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
        .then(function (response) {
            try {
                localStorageService.set(localStorageAuthIndex, {
                    token: response.data.access_token,
                    userName: loginData.userName,
                    userId: response.data.UserId,
                    expireDate: new Date(response.data['.expires']),
                    refreshToken: response.data.refresh_token
                });
            } catch (e) {
                $log.error('Error: ' + e.message);
                deferred.reject(e);
            }
            $http.get(authbaseUrl + "/accounts/currentuser/roles").then(function (response) {
                var isAdmin = false;
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].toLowerCase() === "admin") {
                        isAdmin = true;
                        break;
                    }
                }

                updateAuthData(new AuthModel(factory.authentication.isAuth, factory.authentication.userName, factory.authentication.userId, isAdmin));
            });

            updateAuthData(new AuthModel(true, loginData.userName, response.data.UserId));
            deferred.resolve(response);
        })
        .catch(function (response) {
            _logOut();
            if (response.data && response.data.error_description) {
                $log.error(response.error_description);
                deferred.reject(response.data.error_description);
            } else {
                deferred.reject(response);
            }
        });

        return deferred.promise;
    }
    var _refreshLogin = function () {
        var authData = localStorageService.get(localStorageAuthIndex);

        var deferred = $q.defer();

        if (authData) {
            var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken;
            $http.post(authbaseUrl.replace('/api', '') + 'oauth2/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {
                localStorageService.set(localStorageAuthIndex, {
                    token: response.data.access_token,
                    userName: authData.userName,
                    userId: response.data.UserId,
                    expireDate: new Date(response.data['.expires']),
                    refreshToken: response.data.refresh_token
                });
            }).catch(function (response) {
                _logOut();
                if (response.data && response.data.error_description) {
                    $log.error(err.error_description);
                    deferred.reject(response.data.error_description);
                } else {
                    deferred.reject("Error on logging in");
                }
            });
        } else {
            deferred.reject();
        }

        return deferred.promise;
    }
    var _fillAuthData = function () {

        var authData = localStorageService.get(localStorageAuthIndex);
        if (authData) {
            var expireDate = new Date(authData.expireDate);
            var now = new Date();
            if (expireDate <= now) {
                _refreshLogin().then(function () {
                    updateAuthData(new AuthModel(true, authData.userName, authData.userId));
                });
            } else {
                updateAuthData(new AuthModel(true, authData.userName, authData.userId));
            }
        }
    }
    var _checkOnlineStatus = function () {
        var deferred = $q.defer();
        $http.get(authbaseUrl + 'accounts/ping')
          .success(function () {
              deferred.resolve();
          })
          .error(function () { deferred.reject(); });
        return deferred.promise;
    }

    factory.register = _register;
    factory.login = _login;
    factory.logOut = _logOut;
    factory.fillAuthData = _fillAuthData;
    factory.checkOnlineStatus = _checkOnlineStatus;
    factory.authentication = new AuthModel();
    return factory;
}