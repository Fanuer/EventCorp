function statisticsFactory($http, statisticsUrl) {
  var factory = {}

  function _getStatistics() {
    return $http.get(statisticsUrl);
  }

  factory.getStatistics = _getStatistics;
  return factory;
}