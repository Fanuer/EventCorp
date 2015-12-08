function showAvatarUpload() {
  return {
    restrict: "A",
    link: function (scope, element, attrs) {
      element.click(function (e) {
        if (attrs.fileuploadid) {
          $('#' + attrs.fileuploadid).click();
        }
      });
    }
  };
}