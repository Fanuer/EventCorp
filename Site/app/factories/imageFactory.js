function imageFacotry($http, authbaseUrl) {
    function _uploadFile(file) {
        var fd = new FormData();
        fd.append("file", file);
        return $http.post(authbaseUrl + 'upload/multipart', fd, {
            withCredentials: false,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        });
    }

    var factory = {};
    factory.uploadFile = _uploadFile;
    return factory;
}