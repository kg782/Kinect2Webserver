'use strict';

/**
 * @ngdoc function
 * @name timelineApp.controller:NavCtrl
 * @description
 * # NavCtrl
 * Controller of the timelineApp
 */
angular.module('timelineApp')
  .controller('NavCtrl', function ($scope, $location) {

    this.isActive = function(route) {
      return route === $location.path();
    };
  });
