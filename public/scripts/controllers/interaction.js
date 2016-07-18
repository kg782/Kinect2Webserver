'use strict';

/**
 * @ngdoc function
 * @name timelineApp.controller:InteractionCtrl
 * @description
 * # InteractionCtrl
 * Controller of the timelineApp
 */
angular.module('timelineApp')
  .controller('InteractionCtrl', function ($scope, $log, KinectManager, $window) {
    
    $scope.pointer = null;
    $scope.isPressed = false;

    var self = this;

    function init() {
      KinectManager.enableStream(Kinect.INTERACTION_STREAM_NAME);
      KinectManager.addStreamFrameHandler(Kinect.INTERACTION_STREAM_NAME, onInteraction);

      $scope.$on('$destroy', function() {
        // KinectManager.disableAllStream();
        KinectManager.removeStreamFrameHandler(onInteraction);
      });
    }

    var logged = false;
    function logOnce(obj) {
      if (!logged) {
        logged = true;
        console.log(obj);
      }
    }

    function onInteraction(frame) {
      for (var i = 0; i < frame.handPointers.length; i++) {
        var pointer = frame.handPointers[i];
        if (pointer.isPrimaryUser && pointer.isPrimaryHandOfUser) {
          updateHand(frame.handPointers[i]);
        }
      }
    }

    function updateHand(pointer) {
      logOnce(pointer);
      $scope.pointer = pointer;
      $scope.isPressed = pointer.isPressed;
      $scope.$apply();
    }

    $scope.handStyle = function() {
      if ($scope.pointer) {
        var windowHalfW = $window.innerWidth / 2;
        var windowHalfH = $window.innerHeight / 2;
        var mouseX = windowHalfW + ($scope.pointer.x * windowHalfW / 2);
        var mouseY = windowHalfH + ($scope.pointer.y * windowHalfH / 2);
        return {
          left: mouseX + 'px',
          top: mouseY + 'px'
        };
      }
    };

    init();
  });
