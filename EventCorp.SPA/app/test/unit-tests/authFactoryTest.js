describe("Factory: AuthFactory", function () {
  var authFactory;
  var $httpBackend;
  var lSS;
  var localStorageAuthIndex;
  var AUTHBASEFACTORY = "http://ec-auth.azurewebsites.net/api/";
  var authMock = {
    access_token: 'LaeuftSoweit',
    userName: 'Stefan',
    UserId: '1',
    '.expires': new Date(2099, 1, 1),
    refresh_token: 'passt'
  };
  var loginModel = {
    userName: 'Stefan',
    passsword: 'Test1234'
  }

  beforeEach(module('eventCorp', function ($provide) {
    $provide.constant('authbaseUrl', AUTHBASEFACTORY);
  }));
  beforeEach(inject(function (_authFactory_, _$httpBackend_, _localStorageService_, _localStorageAuthIndex_) {
    authFactory = _authFactory_;
    $httpBackend = _$httpBackend_;
    lSS = _localStorageService_;
    localStorageAuthIndex = _localStorageAuthIndex_;
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 60000;
    $httpBackend.when('POST', AUTHBASEFACTORY.replace('/api', '') + 'oauth2/token').respond(authMock);
    $httpBackend.when('Get', AUTHBASEFACTORY + 'api/accounts/ping').respond({});
  }));

  it("should perform login", function () {
    if (lSS) {
      lSS.remove(localStorageAuthIndex);
    }
    authFactory.login(loginModel).then(function (response) {
      var localData = lSS.get(localStorageAuthIndex);
      expect(localData).toBeDefined();
      expect(response.data).toBeDefined();
      expect(localData.userName).toBe(loginModel.userName);
    });
    $httpBackend.flush();
  });
  it("should perform logout", function () {
    authFactory.logOut();
    expect(authFactory.authentication).toBeDefined();
    expect(authFactory.authentication.isAuth).toBeFalsy();
  });
  it("Server should be there", function () {
    var hasErrors = false;
    $httpBackend.expectGET(AUTHBASEFACTORY + "accounts/ping").respond({});
    authFactory.checkOnlineStatus()
      .then(function() {})
      .catch(function() { hasErrors = true; })
      .finally(function() {
        expect(hasErrors).toBeFalsy();
      });
    $httpBackend.flush();
  });

  afterEach(function () {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });
});
