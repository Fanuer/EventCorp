function recommendationFactory($http, recommendationUrl) {
  var factory = {}

  function _getRecommendations() {
    return $http.get(recommendationUrl);
  }

  factory.getRecommendations = _getRecommendations;
  return factory;
}