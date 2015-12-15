describe('A suite', function() {
    var testMe;
    beforeEach(module('eventCorp'));

    beforeEach(function() {
        testMe = true;
    });

    it('should be true', function() {
        expect(testMe).toBe(true);
    });

    it('should exist', inject(function (userFactory) {
        expect(userFactory).toBeDefined();
    }));
});