'use strict';

/**
 * @ngdoc overview
 * @name timelineApp
 * @description
 * # timelineApp
 *
 * Main module of the application.
 */
angular
  .module('timelineApp', [
    'ngAnimate',
    'ngRoute',
    'ngSanitize',
    'ngTouch'
  ])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl',
        redirectTo: '/body'
      })
      .when('/body', {
        templateUrl: 'views/body.html',
        controller: 'SkeletonCtrl'
      })
      .when('/interaction', {
        templateUrl: 'views/interaction.html',
        controller: 'InteractionCtrl'
      })
      .when('/gesture', {
        templateUrl: 'views/gesture.html',
        controller: 'GestureCtrl'
      })
      .otherwise({
        redirectTo: '/'
      });
  })
  .run(function (KinectManager) {
    KinectManager.init();
  });
