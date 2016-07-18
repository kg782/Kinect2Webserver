'use strict';

/**
 * @ngdoc service
 * @name timelineApp.kinectGesture
 * @description
 * # kinectGesture
 * Factory in the timelineApp.
 */
angular.module('timelineApp')
  .factory('kinectGesture', function ($injector, $log, KinectManager) {
    // Service logic

    /**
    * KinectGesture
    **/
    function KinectGesture() {
      this.SWIPE_GESTURE = 'SWIPE_GESTURE';
      this.GO_FORWARD_GESTURE = 'NEXT_GESTURE';
      this.GO_BACKWARD_GESTURE = 'GO_BACKWARD_GESTURE';

      this.primaryUserId = 0;
      this.handlers = {
        swipe: []
      };

      KinectManager.addEventHandler(Kinect.USERSTATE_EVENT_CATEGORY, Kinect.PRIMARYUSERCHANGED_EVENT_TYPE, this.onPrimaryUserChanged);
      KinectManager.addEventHandler(Kinect.USERSTATE_EVENT_CATEGORY, Kinect.USERSTATESCHANGED_EVENT_TYPE, this.onUserStateChanged);
      KinectManager.addStreamFrameHandler(Kinect.INTERACTION_STREAM_NAME, this.onInteraction);
    }

    KinectGesture.prototype.destroy = function() {
      KinectManager.removeEventHandler(this.onPrimaryUserChanged);
      KinectManager.removeEventHandler(this.onUserStateChanged);
      KinectManager.removeStreamFrameHandler(this.onInteraction);
      this.handlers = null;
    };

    KinectGesture.prototype.addGestureHandler = function(gestureType, handler) {

    };

    KinectGesture.prototype.removeGestureHandler = function(gestureType, handler) {

    };

    KinectGesture.prototype.onPrimaryUserChanged = function(event) {
      console.log('onPrimaryUserChanged', event);
      this.primaryUserId = event.newValue;
    };

    KinectGesture.prototype.onUserStateChanged = function(event) {
      console.log('onUserStateChanged:', event);
    };

    KinectGesture.prototype.onInteraction = function(frame) {

    };

    /**
    * Gesture
    **/
    function Gesture() {

    }

    /**
    * GesturePart
    **/
    function GesturePart() {
      
    }

    // Public API here
    return function(locals) {
      return $injector.instantiate(KinectGesture, locals);
    };
  });
