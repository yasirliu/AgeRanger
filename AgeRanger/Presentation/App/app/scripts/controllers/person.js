define(['angular',
  //entity
  '../models/person',
  '../models/aggregates/personAge',
  //service
  '../services/personService'
], function (angular, person, personAge) {
  'use strict';

  /**
   * @ngdoc function
   * @name ageRangerApp.controller:PersonCtrl
   * @description
   * # PersonCtrl
   * Controller of the ageRangerApp
   */
  angular.module('ageRangerApp.controllers.PersonCtrl', ['ageRangerApp.services.PersonService'])
    .controller('PersonCtrl', ['$scope', 'personService',
      function ($scope, personService) {

        $scope.queryfilter;
        //person entity
        $scope.person = new person(0, null, null, 0);
        //person version
        $scope.personVersion = "";

        function _entityMapper(personsFromServer) {
          $scope.persons = [];
          personsFromServer.forEach(function (element) {
            var p = new personAge(element.id,
              element.firstName, element.lastName, element.age, element.group.description);
            p.setVersion(element.rowVersion);
            $scope.persons.push(p);
          }, this);
        };
        //query person
        $scope.getPersons = function () {
          var persons = personService.query({
              filter: $scope.queryfilter
            },
            function () {
              _entityMapper(persons);
            })
        };

        $scope.getPerson = function (id) {
          var persons = personService.get({
              id: id
            },
            function () {
              _entityMapper(persons);
            })
        };

        $scope.createPerson = function () {
          //validate user input
          $scope.person.validate();
          if ($scope.person.validateResult.length === 0) {
            personService.save($scope.person,
              function () {
                console.log("Done");
              })
          }
          else{
            throw new Error("Test");
          }
        };

        $scope.editPerson = function () {};
      }
    ]);
});
