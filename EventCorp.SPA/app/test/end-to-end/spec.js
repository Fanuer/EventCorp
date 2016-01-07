describe('End-to-End-Tests', function () {

  describe('login tests', function () {

    beforeEach(function () {
      browser.get('http://eventcorp.azurewebsites.net/');
    });
    it('should redirect to login', function () {

      expect(browser.getTitle()).toEqual('EventCorp');
      expect(browser.getCurrentUrl()).toEqual('http://eventcorp.azurewebsites.net/#/login');
    });
    it('should log in', function () {
      var submitButton = element(by.id('btn-login'));
      expect(submitButton.isEnabled()).toEqual(false);

      element(by.model('loginData.userName')).sendKeys('Stefan');
      element(by.model('loginData.password')).sendKeys('Test1234');

      expect(submitButton.isEnabled()).toEqual(true);

      element(by.id('btn-login')).click();
      browser.waitForAngular();
      expect(browser.getCurrentUrl()).toEqual('http://eventcorp.azurewebsites.net/#/');
      element(by.id('logout')).click();
    });
  });

  describe('event tests', function () {
    beforeEach(function () {
      browser.get('http://eventcorp.azurewebsites.net/');
      element(by.model('loginData.userName')).sendKeys('Stefan');
      element(by.model('loginData.password')).sendKeys('Test1234');
      element(by.id('btn-login')).click();
      browser.waitForAngular();
    });

    it("should navigate to events", function () {
      element(by.id('events')).click();
      browser.waitForAngular();
      expect(browser.getCurrentUrl()).toEqual('http://eventcorp.azurewebsites.net/#/events');
    });
    it("pageSize is initialised correctly", function () {
      element(by.id('events')).click();
      browser.waitForAngular();
      var pageSize = element(by.css('select[id="changePageSize"] option[selected]')).getText();
      var eventEntries = element.all(by.css('.event-item'));
      expect(pageSize).toEqual('25');
      expect(eventEntries.count()).toEqual(25);
    });
    it("pageSize can be changed", function () {
      element(by.id('events')).click();
      browser.waitForAngular();

      var pageSize = element(by.css('select[id="changePageSize"] option[selected]')).getText();
      var eventEntries = element.all(by.css('.event-item'));
      expect(pageSize).toEqual('25');
      expect(eventEntries.count()).toEqual(25);

      var select = element(by.css('select[id="changePageSize"]')).click();
      select.click();
      element.all(by.css('select[id="changePageSize"] option')).get(0).click();

      browser.waitForAngular();
      expect(element.all(by.css('.event-item')).count()).toEqual(10);
    });
    it("inserting searchTerms can filter results", function () {
      element(by.id('events')).click();
      browser.waitForAngular();

      element(by.id("inputSearchTerm")).sendKeys("ligula");
      browser.driver.sleep(5000);
      browser.waitForAngular();

      var eventEntries = element.all(by.css('.event-item'));
      expect(eventEntries.count()).toEqual(2);
    });

    afterEach(function () {
      element(by.id('logout')).click();
      browser.waitForAngular();
    });
  });
});