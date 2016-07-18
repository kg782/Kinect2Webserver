'use strict';

/**
 * @ngdoc service
 * @name timelineApp.kinect
 * @description
 * # kinect
 * Service in the timelineApp.
 */
angular.module('timelineApp')
  .run(function() {
    // Add additional constants to Kinect

    Kinect.GESTURE_STREAM_NAME = 'gesture';

    // JointType
    Kinect.SPINE_BASE = 0;
    Kinect.SPINE_MID = 1;
    Kinect.NECK = 2;
    Kinect.HEAD = 3;
    Kinect.SHOULDER_LEFT = 4;
    Kinect.ELBOW_LEFT = 5;
    Kinect.WRIST_LEFT = 6;
    Kinect.HAND_LEFT = 7;
    Kinect.SHOULDER_RIGHT = 8;
    Kinect.ELBOW_RIGHT = 9;
    Kinect.WRIST_RIGHT = 10;
    Kinect.HAND_RIGHT = 11;
    Kinect.HIP_LEFT = 12;
    Kinect.KNEE_LEFT = 13;
    Kinect.ANKLE_LEFT = 14;
    Kinect.FOOT_LEFT = 15;
    Kinect.HIP_RIGHT = 16;
    Kinect.KNEE_RIGHT = 17;
    Kinect.ANKLE_RIGHT = 18;
    Kinect.FOOT_RIGHT = 19;
    Kinect.SPINE_SHOULDER = 20;
    Kinect.HAND_TIP_LEFT = 21;
    Kinect.THUMB_LEFT = 22;
    Kinect.HAND_TIP_RIGHT = 23;
    Kinect.THUMB_RIGHT = 24;

    // JointTrackingState
    Kinect.JOINT_NOT_TRACKED = 0;
    Kinect.JOINT_INFERRED = 1;
    Kinect.JOINT_TRACKED = 2;

    // HandState
    Kinect.HAND_UNKNOWN = 0;
    Kinect.NOT_TRACKED = 1;
    Kinect.OPEN = 2;
    Kinect.CLOSED = 3;
    Kinect.LASSO = 4;

    // GESTURE_TYPE
    Kinect.GESTURE_WAVE_RIGHT = 0;
    Kinect.GESTURE_WAVE_LEFT = 1;
    Kinect.GESTURE_MENU = 2;
    Kinect.GESTURE_SWIPE_LEFT = 3;
    Kinect.GESTURE_SWIPE_RIGHT = 4;
  })
  .service('KinectManager', function KinectManager($q, $log, $timeout, Constants) {

    this.sensor = null;

    var self = this;
    var isSensorConnected = false;
    var engagedUser = null;
    var streamFrameHandlers = [];
    var streamFrameHandlerNames = [];
    var eventHandlers = [];
    var eventHandlerNames = [];
    var currentConfig = null;

    this.init = function() {
      Kinect.connect(Constants.kinectUri, Constants.kinectPort);

      this.sensor = Kinect.sensor(Kinect.DEFAULT_SENSOR_NAME, function(sensorToConfig, isConnected) {
        if (isConnected) {
          console.log('Connected to Kinect');

          if (currentConfig) {
            self.postConfig(currentConfig);
          }
        } else {
          $log.error('Coudn\'t connect to Kinect');
        }
      });

      self.bindEventHandlers(this.sensor);
    };

    this.bindEventHandlers = function(sensor) {
      sensor.addEventHandler(function (e) {
        for (var i = 0; i < eventHandlerNames.length; i++) {
          if (eventHandlerNames[i].category === e.category && eventHandlerNames[i].eventType === e.eventType) {
            eventHandlers[i](e);
          }
        }
      });

      sensor.addStreamFrameHandler(function(frame) {
        for (var i = 0; i < streamFrameHandlerNames.length; i++) {
          if (streamFrameHandlerNames[i] === frame.stream) {
            streamFrameHandlers[i](frame);
          }
        }
      });
    };

    this.getConfig = function() {
      var deferred = $q.defer();

      this.sensor.getConfig(function(configData) {
        deferred.resolve(configData);
      }, function(error) {
        deferred.reject(error);
      });

      return deferred.promise;
    };

    this.postConfig = function(config) {
      console.log('postConfig:', config);
      this.sensor.postConfig(config, function(statusText, errorData) {
        $log.error('postConfig failed:', (errorData !== null) ? JSON.stringify(errorData) : statusText);
      });
      this.saveConfig();
    };

    this.saveConfig = function() {
      this.getConfig().then(function(config) {
        console.log(config);
        currentConfig = {
          // interaction: {
          //   enabled: config.interaction.enabled
          // },
          // userviewer: {
          //   enabled: config.userviewer.enabled
          // },
          // backgroundRemoval: {
          //   enabled: config.backgroundRemoval.enabled
          // },
          body: {
            enabled: config.body.enabled
          },
          gesture: {
            enabled: config.gesture.enabled
          }
        };
        console.log('saveConfig:', config);
      });
    };

    this.enableStream = function(streamName) {
      console.log('enableStream(\'' + streamName + '\')');
      var config = {};
      config[streamName] = {
        enabled: true
      };
      this.postConfig(config);
    };

    this.disableAllStream = function() {
      console.log('disableAllStream()');
      this.postConfig({
        interaction: {
          enabled: false,
        },
        userviewer: {
          enabled: false,
          resolution: '640x480', //320x240, 160x120, 128x96, 80x60
          // 'userColors': { 'engaged': 0xffffffff, 'tracked': 0xffffffff },
          // 'defaultUserColor': 0xffffffff, //RGBA
        },
        backgroundRemoval: {
          enabled: false,
          resolution: '640x480', //1280x960
        },
        body: {
          enabled: false,
        },
        sensorStatus: {
          // enabled: false,
        },
        gesture: {
          enabled: false
        }
      });
    };

    this.addStreamFrameHandler = function(streamName, handler) {
      if (streamName && handler) {
        streamFrameHandlerNames.push(streamName);
        streamFrameHandlers.push(handler);
        console.log('streamFrameHandler was added for', streamName);
      } else {
        $log.error('both streamName and handler need to be set');
      }
    };

    this.removeStreamFrameHandler = function(handler) {
      var index = streamFrameHandlers.indexOf(handler);
      if (index >= 0) {
        var streamName = streamFrameHandlerNames[index];
        streamFrameHandlers.splice(index, 1);
        streamFrameHandlerNames.splice(index, 1);
        console.log(streamName, 'streamFrameHandler was removed');
      } else {
        $log.error('streamFrameHandler was not found to remove');
      }
    };

    this.addEventHandler = function(category, eventType, handler) {
      if (category && eventType && handler) {
        eventHandlerNames.push({
          category: category,
          eventType: eventType
        });
        eventHandlers.push(handler);
        console.log('eventHandler was added:', category, '/', eventType);
      } else {
        $log.error('category, eventType and handler is required to add event handler');
      }
    };

    this.removeEventHandler = function(handler) {
      var index = eventHandlers.indexOf(handler);
      if (index >= 0) {
        var category = eventHandlerNames[index].category;
        var eventType = eventHandlerNames[index].eventType;
        eventHandlers.splice(index, 1);
        eventHandlerNames.splice(index, 1);
        console.log(category + '/' + eventType + ' eventHandler was removed');
      } else {
        $log.error('eventHandler was not found to remove');
      }
    };

  });
