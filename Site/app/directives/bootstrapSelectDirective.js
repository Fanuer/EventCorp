function bootstrapSelect() {
  return {
    restrict: "A",
    link: function (scope, element, attrs) {
      $(element).selectpicker();
    }
  };
}