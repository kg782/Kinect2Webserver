// -----------------------------------------------------------------------
// <copyright file="PushLeftSegment1.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The first part of the swipe left gesture</summary>
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
    /// The first part of the swipe left gesture
    /// </summary>
    public class PushLeftSegment1 : GestureSegment, IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body, List<object> bodyHistory)
        {
            // Facing the sensors
            if (Math.Abs(body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.ShoulderLeft].Position.Z) > Properties.Settings.Default.SholderFacingDistance)
            {
                //Debug.WriteLine("GesturePart 0 - Facing sensor - Fail");
                return GesturePartResult.Fail;
            }

            // Body is in left distance
            if (body.Joints[JointType.Head].Position.Z < Properties.Settings.Default.GestureMinDistance || body.Joints[JointType.Head].Position.Z > Properties.Settings.Default.GestureMaxDistance)
            {
                //Debug.WriteLine("GesturePart 0 - Body is in left distance - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 0 - Body is in left distance - Pass");
            
            //Left hand in front of left shoulder
            if (body.Joints[JointType.HandTipLeft].Position.Z < body.Joints[JointType.ShoulderLeft].Position.Z)
            {
                // Pause if hand is forwarding is last moment
                var frameBefore = 20;
                if (bodyHistory.Count >= frameBefore)
                {
                    for (var i = 0; i < frameBefore - 1; i++)
                    {
                        var handZ = getPositionOfJoint(bodyHistory[i], JointType.HandLeft, "z");
                        var previousHandZ = getPositionOfJoint(bodyHistory[i + 1], JointType.HandLeft, "z");

                        if (handZ - previousHandZ < -0.01)
                        {
                            return GesturePartResult.Pausing;
                        }
                    }

                    //Debug.WriteLine("PushLeftSegment1 - Succeed");
                    return GesturePartResult.Suceed;
                }
            }

            //Debug.WriteLine("GesturePart 0 - Left hand in front of left shoulder - FAIL");
            return GesturePartResult.Fail;
        }
    }
}