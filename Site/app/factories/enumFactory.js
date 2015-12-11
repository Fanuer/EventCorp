function enumFactory($http, $q, authbaseUrl) {

    var factory = { enums: {} };
    var isIntialise = false;

    function _getGenderTypes() {
        return $http.get(authbaseUrl + "enums/gender");
    }
    function _getEventTypes() {
        return $http.get(authbaseUrl + "enums/events");
    }
    function _getCssForType(type, value) {
        var result = null;
        var enumValues = enumFactory.enums[type];
        for (var i = 0; i < enumValues.length; i++) {
            if (enumValues[i].value === value || enumValues[i].displayName === value) {
                result = enumValues[i].css;
                break;
            }
        }
        return result;
    }

    function _init() {
        var genderPromise = $q.defer();
        var eventPromise = $q.defer();

        _getGenderTypes().then(function (response) {
            if (response.data.length >= 3) {
                response.data[0].css = "glyphicon-king"; //Male
                response.data[1].css = "glyphicon-queen"; //Female
                response.data[2].css = "glyphicon-pawn"; //Others
            }
            factory.enums.genderTypes = response.data;
            genderPromise.resolve();
        }).catch(function (response) {
            genderPromise.reject(response);
        });

        _getEventTypes().then(function (response) {
            if (response.data.length >= 4) {
                response.data[0].css = "glyphicon-music"; //Cultural
                response.data[1].css = "glyphicon-modal-window"; //Sports
                response.data[2].css = "glyphicon-education"; //Education
                response.data[3].css = "glyphicon-flag"; //Other
            }
            factory.enums.eventTypes = response.data;
            eventPromise.resolve();
        }).catch(function (response) {
            eventPromise.reject(response);
        });

        var result = $q.all([genderPromise.promise, eventPromise.promise]);
        result.then(function () {
            isIntialise = true;
        });
        return result;
    }


    factory.getEventTypes = _getEventTypes;
    factory.getGenderTypes = _getGenderTypes;
    factory.getCssForType = _getCssForType;

    _init();
    return factory;
}