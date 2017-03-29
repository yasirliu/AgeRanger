define(function () {
  'use strict';
  var model = function (minAge, maxAge, description) {
    this.minAge = minAge;
    this.maxAge = maxAge;
    this.description = description;
  }

  return model;
});
