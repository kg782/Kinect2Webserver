﻿// -----------------------------------------------------------------------
// <copyright file="PushRightSegment1.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The first part of the swipe right gesture</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The first part of the swipe right gesture
    /// </summary>
    public class PushRightSegment1 : GestureSegment, IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body, List<object> bodyHistory)
        {
            // Facing the sensors
            if (Math.Abs(body.Joints[JointType.ShoulderRight].Position.Z - body.Joints[JointType.ShoulderRight].Position.Z) > Properties.Settings.Default.SholderFacingDistance)
            {
                //Debug.WriteLine("GesturePart 0 - Facing sensor - Fail");
                return GesturePartResult.Fail;
            }

            // Body is in right distance
            if (body.Joints[JointType.Head].Position.Z < Properties.Settings.Default.GestureMinDistance || body.Joints[JointType.Head].Position.Z > Properties.Settings.Default.GestureMaxDistance)
            {
                //Debug.WriteLine("GesturePart 0 - Body is in right distance - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 0 - Body is in right distance - Pass");
            
            //Right hand in front of right shoulder
            if (body.Joints[JointType.HandTipRight].Position.Z < body.Joints[JointType.ShoulderRight].Position.Z)
            {
                // Pause if hand is forwarding is last moment
                var frameBefore = 20;
                if (bodyHistory.Count >= frameBefore)
                {
                    for (var i = 0; i < frameBefore - 1; i++)
                    {
                        var handZ = getPositionOfJoint(bodyHistory[i], JointType.HandRight, "z");
                        var previousHandZ = getPositionOfJoint(bodyHistory[i + 1], JointType.HandRight, "z");

                        if (handZ - previousHandZ < -0.01)
                        {
                            return GesturePartResult.Pausing;
                        }
                    }

                    //Debug.WriteLine("PushRightSegment1 - Succeed");
                    return GesturePartResult.Suceed;
                }
            }

            //Debug.WriteLine("GesturePart 0 - Right hand in front of right shoulder - FAIL");
            return GesturePartResult.Fail;
        }
    }
}