/// <reference path="../../scripts/jasmine/jasmine.js" />

describe("Factory: EnumFactory", function() {
  var enumFactory;
  var $httpBackend;
  var AUTHBASEFACTORY = "http://ec-auth.azurewebsites.net/api/";
  var mockDataSet = false;
  var localStorageAuthIndex = "eventCorp.SPA.auth";
  var genders = [{ "value": 0, "displayName": "Male" }, { "value": 1, "displayName": "Female" }, { "value": 2, "displayName": "Other" }];
  var eventTypes = [{ "value": 0, "displayName": "Cultural" }, { "value": 1, "displayName": "Sports" }, { "value": 2, "displayName": "Education" }, { "value": 3, "displayName": "Other" }];

  var authMock = {
    access_token: 'LaeuftSoweit',
    userName: 'Stefan',
    UserId: '1',
    '.expires': new Date(2099, 1, 1),
    refresh_token: 'passt'
  };

  
  // trick SPA to think user is logged in
  beforeEach(function () {
    if (typeof localStorage === "undefined") {
      throw new Error("localStorage in not supported");
    }
  });
  beforeEach(module('eventCorp', function($provide) {
    $provide.constant('authbaseUrl', AUTHBASEFACTORY);
  }));
  beforeEach(inject(function (_enumFactory_, _$httpBackend_, _localStorageAuthIndex_) {
    enumFactory = _enumFactory_;
    $httpBackend = _$httpBackend_;
    localStorageAuthIndex = _localStorageAuthIndex_;
    $httpBackend.when('POST', AUTHBASEFACTORY + 'oauth2/token').respond(authMock);
    $httpBackend.when('GET', AUTHBASEFACTORY + 'enums/gender').respond(genders);
    $httpBackend.when('GET', AUTHBASEFACTORY + 'enums/events').respond(eventTypes);

    jasmine.DEFAULT_TIMEOUT_INTERVAL = 60000;
  }));

  it("should initialize enums", function () {
    var hasErrors = false;
    expect(enumFactory.enums).toBeDefined();
    expect(enumFactory.enums.genderTypes).not.toBeDefined();
    expect(enumFactory.enums.eventTypes).not.toBeDefined();
    enumFactory.initPromise
      .then(function () {
      })
      .catch(function () {
      hasErrors = true;
      })
      .finally(function () {
      expect(hasErrors).toBeFalsy();
      expect(enumFactory.enums.genderTypes).toBeDefined();
      expect(enumFactory.enums.eventTypes).toBeDefined();
    });

    $httpBackend.flush();
  });
  it("should return genderTypes", function() {
    $httpBackend.expect('GET', AUTHBASEFACTORY + 'enums/gender').respond(genders);
    enumFactory.getGenderTypes().then(function (response) {
      expect(response.data).toBeDefined();
      expect(response.data.length).toBe(3);
      expect(response.data).toEqual(genders);
    });
    $httpBackend.flush();
  });
  it("should return eventTypes", function() {
    $httpBackend.expect('GET', AUTHBASEFACTORY + 'enums/events').respond(eventTypes);
    enumFactory.getEventTypes().then(function (response) {
      expect(response.data).toBeDefined();
      expect(response.data.length).toBe(4);
      expect(response.data).toEqual(eventTypes);
    });
    $httpBackend.flush();
  });
  it("should return a proper css-class", function () {
    enumFactory.initPromise.then(function() {
      var css = enumFactory.getCssForType("eventTypes", 0);
      expect(css).toBe("glyphicon-music");

      css = enumFactory.getCssForType("genderTypes", 0);
      expect(css).toBe("glyphicon-king");
    });
    $httpBackend.flush();
  });

  afterEach(function() {
    $httpBackend.verifyNoOutstandingExpectation();
    $httpBackend.verifyNoOutstandingRequest();
  });
});