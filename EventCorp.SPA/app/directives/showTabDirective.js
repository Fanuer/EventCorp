function showtab() {
  return {
    restrict: "A",
    link: function (scope, element, attrs) {
      element.click(function(e) {
        e.preventDefault();
        $(element).tab('show');
      });
    }
  };
}