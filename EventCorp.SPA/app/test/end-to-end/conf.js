/**
 * Voraussetzungen:
 * 
 * Git from https://git-scm.com/downloads
 * Node.js from https://nodejs.org/en/
 * Python 2.7 from https://www.python.org/download/releases/2.7/
 * Visual Studio 2015 Community Edition from https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx using "Download Community Free"
 * Latest version of NPM from https://github.com/npm/npm/releases you can simply download the zip, extract it and copy the contents to wherever your global node_modules/npm is
 * Set the environment variable GYP_MSVS_VERSION=2015
 * 
 * Read more at http://www.serverpals.com/blog/building-using-node-gyp-with-visual-studio-express-2015-on-windows-10-pro-x64#bg7QOMELyW0qj9MM.99
 * 
 * cmd commands:
 * ----------
 * npm install -g protractor 
 * npm install -g jasmine-reporters
 * ---------
 * webdriver-manager update
 * webdriver-manager start
 * ---------
 * protractor D:\Projekte\EventCorp\EventCorp.SPA\app\test\end-to-end\conf.js
 */

exports.config = {
  framework: 'jasmine',
  seleniumAddress: 'http://localhost:4444/wd/hub',
  specs: ['spec.js'],
  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 30000,
    isVerbose: true,
    includeStackTrace: true
  },
  // Options to be passed to Jasmine-node.
  onPrepare: function () {
    /*var jasmineReporters = require('jasmine-reporters');
    jasmine.getEnv().addReporter(new jasmineReporters.JUnitXmlReporter({
      consolidateAll: true,
      savePath: 'output',
      filePrefix: 'xmloutput'
    }));*/
  },
}