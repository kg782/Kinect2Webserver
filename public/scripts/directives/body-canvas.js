'use strict';

/**
 * @ngdoc directive
 * @name timelineApp.directive:bodyCanvas
 * @description
 * # bodyCanvas
 */
angular.module('timelineApp')
  .directive('bodyCanvas', function ($log, KinectManager) {
    return {
      templateUrl: 'views/body-canvas.html',
      restrict: 'E',
      replace: true,
      controllerAs: 'ctrl',
      controller: function Controller($scope, $element) {
        var self = this;

        var scene = null;
        var camera = null;
        var renderer = null;

        var jointMaterial = new THREE.PointsMaterial({color: 0x0000ff, size: 0.03});
        var inferredJointMaterial = new THREE.PointsMaterial({color: 0x333333, size: 0.03});
        var boneMaterial = new THREE.LineBasicMaterial({color: 0x0000ff, linewidth: 2});
        var inferredBoneMaterial = new THREE.LineBasicMaterial({color: 0x333333, linewidth: 2});

        this.init = function() {
          initKinect();
          initEventListener();
          initScene();
        };

        var logged = false;
        function logOnce(obj) {
          if (!logged) {
            logged = true;
          }
        }

        function initKinect() {
          KinectManager.enableStream(Kinect.BODY_STREAM_NAME);

          $scope.$on('$destroy', function() {
            // KinectManager.disableAllStream();
          });
        }

        function initEventListener() {
          KinectManager.addStreamFrameHandler(Kinect.BODY_STREAM_NAME, onStreamFrame);

          $scope.$on('$destroy', function() {
            KinectManager.removeStreamFrameHandler(onStreamFrame);
          });
        }

        function initScene() {
          scene = new THREE.Scene();
          camera = new THREE.PerspectiveCamera(75, 800 / 600, 0.1, 1000);

          renderer = new THREE.WebGLRenderer();
          renderer.setSize(800, 600);
          $element.append(renderer.domElement);

          camera.position.z = 0;
        }

        function onStreamFrame(frame) {
          render(frame);
        }

        function render(frame) {
          clearScene();

          for (var i = 0; i < frame.bodies.length; ++i) {
            var body = frame.bodies[i];
            if (body.isTracked) {
              renderSkeleton(body);
            }
          }

          renderer.render(scene, camera);
        }

        function renderSkeleton(body) {
          for (var i in body.joints) {
            renderJoint(body.joints[i]);
          }

          renderBone(body.joints[Kinect.HEAD], body.joints[Kinect.NECK]);
          renderBone(body.joints[Kinect.NECK], body.joints[Kinect.SPINE_SHOULDER]);
          renderBone(body.joints[Kinect.SPINE_SHOULDER], body.joints[Kinect.SPINE_MID]);
          renderBone(body.joints[Kinect.SPINE_MID], body.joints[Kinect.SPINE_BASE]);
          
          renderBone(body.joints[Kinect.SPINE_SHOULDER], body.joints[Kinect.SHOULDER_LEFT]);
          renderBone(body.joints[Kinect.SHOULDER_LEFT], body.joints[Kinect.ELBOW_LEFT]);
          renderBone(body.joints[Kinect.ELBOW_LEFT], body.joints[Kinect.WRIST_LEFT]);
          renderBone(body.joints[Kinect.WRIST_LEFT], body.joints[Kinect.HAND_LEFT]);
          renderBone(body.joints[Kinect.HAND_LEFT], body.joints[Kinect.HAND_TIP_LEFT]);
          renderBone(body.joints[Kinect.HAND_LEFT], body.joints[Kinect.THUMB_LEFT]);

          renderBone(body.joints[Kinect.SPINE_SHOULDER], body.joints[Kinect.SHOULDER_RIGHT]);
          renderBone(body.joints[Kinect.SHOULDER_RIGHT], body.joints[Kinect.ELBOW_RIGHT]);
          renderBone(body.joints[Kinect.ELBOW_RIGHT], body.joints[Kinect.WRIST_RIGHT]);
          renderBone(body.joints[Kinect.WRIST_RIGHT], body.joints[Kinect.HAND_RIGHT]);
          renderBone(body.joints[Kinect.HAND_RIGHT], body.joints[Kinect.HAND_TIP_RIGHT]);
          renderBone(body.joints[Kinect.HAND_RIGHT], body.joints[Kinect.THUMB_RIGHT]);

          renderBone(body.joints[Kinect.SPINE_BASE], body.joints[Kinect.HIP_LEFT]);
          renderBone(body.joints[Kinect.HIP_LEFT], body.joints[Kinect.KNEE_LEFT]);
          renderBone(body.joints[Kinect.KNEE_LEFT], body.joints[Kinect.ANKLE_LEFT]);
          renderBone(body.joints[Kinect.ANKLE_LEFT], body.joints[Kinect.FOOT_LEFT]);

          renderBone(body.joints[Kinect.SPINE_BASE], body.joints[Kinect.HIP_RIGHT]);
          renderBone(body.joints[Kinect.HIP_RIGHT], body.joints[Kinect.KNEE_RIGHT]);
          renderBone(body.joints[Kinect.KNEE_RIGHT], body.joints[Kinect.ANKLE_RIGHT]);
          renderBone(body.joints[Kinect.ANKLE_RIGHT], body.joints[Kinect.FOOT_RIGHT]);
        }

        function renderJoint(joint) {
          var geometry = new THREE.Geometry();
          geometry.vertices.push(new THREE.Vector3(joint.position.x, joint.position.y, -joint.position.z));
          var material = joint.trackingState === Kinect.JOINT_TRACKED ? jointMaterial : inferredJointMaterial;
          scene.add(new THREE.Points(geometry, material));
        }

        function renderBone(joint0, joint1) {
          var geometry = new THREE.Geometry();
          geometry.vertices.push(new THREE.Vector3(joint0.position.x, joint0.position.y, -joint0.position.z));
          geometry.vertices.push(new THREE.Vector3(joint1.position.x, joint1.position.y, -joint1.position.z));
          var material = (joint0.trackingState === Kinect.JOINT_TRACKED && joint1.trackingState === KinectManager.JOINT_TRACKED) ? boneMaterial : inferredBoneMaterial;
          scene.add(new THREE.Line(geometry, material));
        }

        function clearScene() {
          for (var i = scene.children.length - 1; 0 <= i; i--) {
            scene.remove(scene.children[i]);
          }
        }
      },
      link: function postLink(scope, element, attrs, controller) {
        controller.init();
      }
    };
  });
