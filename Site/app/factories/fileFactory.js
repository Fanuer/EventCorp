﻿function fileFactory($http, $q, authbaseUrl) {
  var noAvatarSrc = '/images/no_avatar.png';

  function _uploadFile(file, global) {
    // create an new file
    // file: file-data from input[type="file"]
    // global: bool --> can other users us this file as well
    var fd = new FormData();
    fd.append("file", file);

    if (typeof (global) !== "undefined") {
      fd.append("additional", { global });
    }
    return $http.post(authbaseUrl + 'file', fd, {
      withCredentials: false,
      headers: {
        'Content-Type': undefined
      },
      transformRequest: angular.identity
    });
  }
  function _getFileMetaData(fileId) {
    // gets meta data (see http://ec-auth.azurewebsites.net/swagger/ui/index#File_File_GetFileData)
    return $http.get(authbaseUrl + 'file/' + fileId);
  }
  function _getFileContentUrl(fileId) {
    // use this method to receive an url to the content (e.g. for src-attribute of an image-tag)
    return authbaseUrl + 'file/' + fileId + '/content';
  }
  function _getFileContent(fileId) {
    return $http.get(_getFileContentUrl(fileId));
  }
  function _changeFileState(fileId, state) {
    // state: true: file is persistent false: temporary file --> is deleted after one day
    return $http.put(authbaseUrl + 'file/' + fileId + '/state/' + state);
  }
  function _changeFile(fileId, fileData) {
    return $http.put(authbaseUrl + 'file/' + fileId, fileData);
  }
  function _deleteFile(fileId, fileData) {
    return $http.delete(authbaseUrl + 'file/' + fileId, fileData);
  }
  function _getUserAvatar() {
    var result = $q.defer();
    $http
      .get(authbaseUrl + 'file/avatarId')
      .then(function (response) {
        var link = noAvatarSrc;
        if (response.data) {
          link = _getFileContentUrl(response.data);
        }
        result.resolve(link);
      }).catch(function() {
        result.reject();
      });
    return result.promise;
  }
  var factory = {
    upload: _uploadFile,
    getMetaData: _getFileMetaData,
    getFileContent: _getFileContent,
    getFileContentUrl: _getFileContentUrl,
    changeState: _changeFileState,
    update: _changeFile,
    'delete': _deleteFile,
    getUserAvatar: _getUserAvatar
  };
  return factory;
}