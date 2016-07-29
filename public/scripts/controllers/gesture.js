'use strict';

/**
 * @ngdoc function
 * @name timelineApp.controller:GestureCtrl
 * @description
 * # GestureCtrl
 * Controller of the timelineApp
 */
angular.module('timelineApp')
  .controller('GestureCtrl', function ($scope, $log, KinectManager) {

    $scope.messages = [];
    
    function init() {
      // KinectManager.enableStream(Kinect.INTERACTION_STREAM_NAME);
      KinectManager.enableStream(Kinect.GESTURE_STREAM_NAME);
      KinectManager.addStreamFrameHandler(Kinect.GESTURE_STREAM_NAME, onGesture);
      KinectManager.addEventHandler(Kinect.USERSTATE_EVENT_CATEGORY, Kinect.PRIMARYUSERCHANGED_EVENT_TYPE, onPrimaryUserChanged);
      KinectManager.addEventHandler(Kinect.USERSTATE_EVENT_CATEGORY, Kinect.USERSTATESCHANGED_EVENT_TYPE, onUserStateChanged);
      KinectManager.addEventHandler(Kinect.SENSORSTATUS_EVENT_CATEGORY, Kinect.SENSORSTATUSCHANGED_EVENT_TYPE, onSensorStatusChanged);

      $scope.$on('$destroy', function() {
        // KinectManager.disableAllStream();
        KinectManager.removeStreamFrameHandler(onGesture);
        KinectManager.removeEventHandler(onPrimaryUserChanged);
        KinectManager.removeEventHandler(onUserStateChanged);
        KinectManager.removeEventHandler(onSensorStatusChanged);
      });
    }

    function onGesture(frame) {
      writeLog(frame.trackingId + ' / ' + gestureTypeToString(frame.gestureType));
      console.log('Gesture frame:', frame);
    }

    function gestureTypeToString(gestureType) {
      switch (gestureType) {
      case Kinect.GESTURE_WAVE_RIGHT:
        return 'Wave Right';
      case Kinect.GESTURE_WAVE_LEFT:
        return 'Wave Left';
      case Kinect.GESTURE_MENU:
        return 'Menu';
      case Kinect.GESTURE_SWIPE_LEFT:
        return 'Swipe Left';
      case Kinect.GESTURE_SWIPE_RIGHT:
        return 'Swipe Right';
      }
    }

    function onPrimaryUserChanged(e) {
      console.log(e);
    }

    function onUserStateChanged(e) {
      console.log(e);
    }

    function onSensorStatusChanged(e) {
      console.log(e);
    }

    function writeLog(message) {
      $scope.messages.splice(0, 0, new Date() + ': ' + message);
      $scope.$apply();
    }

    var logged = false;
    function logOnce() {
      if (!logged) {
        logged = true;
        console.log(arguments);
      }
    }

    init();
  });
