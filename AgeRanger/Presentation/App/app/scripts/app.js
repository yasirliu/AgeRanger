/*jshint unused: vars */
define(['angular', 'controllers/main', 'controllers/person'] /*deps*/ , function (angular, MainCtrl, PersonCtrl) /*invoke*/ {
  'use strict';

  /**
   * @ngdoc overview
   * @name ageRangerApp
   * @description
   * # ageRangerApp
   *
   * Main module of the application.
   */
  return angular
    .module('ageRangerApp', ['ngRoute', 'ngResource', 'ageRangerApp.controllers.MainCtrl', 'ageRangerApp.controllers.PersonCtrl'])
    .config(['$routeProvider', '$resourceProvider', '$locationProvider', function ($routeProvider, $resourceProvider, $locationProvider) {
      $routeProvider
        .when('/', {
          templateUrl: 'views/main.html',
          controller: 'MainCtrl'
        })
        .when('/person', {
          templateUrl: 'views/person.html',
          controller: 'PersonCtrl'
        })
        .otherwise({
          redirectTo: '/'
        });
      // use the HTML5 History API
      //$locationProvider.html5Mode(true);
      $resourceProvider.defaults.stripTrailingSlashes = false;
    }]);
});
