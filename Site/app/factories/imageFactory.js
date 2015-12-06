function imageFacotry($http, authbaseUrl) {
    function _uploadFile(file) {
        var fd = new FormData();
        fd.append("file", file);
        return $http.post(authbaseUrl + 'file', fd, {
            withCredentials: false,
            headers: {
                'Content-Type': file.type
            },
            transformRequest: angular.identity
        });
    }

    var factory = {};
    factory.uploadFile = _uploadFile;
    return factory;
}