define(['angular'], function (angular) {
  'use strict';

  /**
   * @ngdoc function
   * @name ageRangerApp.controller:AboutCtrl
   * @description
   * # AboutCtrl
   * Controller of the ageRangerApp
   */
  angular.module('ageRangerApp.controllers.AboutCtrl', [])
    .controller('AboutCtrl', function () {
      this.awesomeThings = [
        'HTML5 Boilerplate',
        'AngularJS',
        'Karma'
      ];
    });
});
