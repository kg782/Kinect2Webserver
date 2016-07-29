// -----------------------------------------------------------------------
// <copyright file="SwipeRightSegment2.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The second part of the swipe right gesture</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;
    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The second part of the swipe right gesture
    /// </summary>
    public class SwipeRightSegment2 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Microsoft.Kinect.Body body, List<object> bodyHistory)
        {
            // Facing the sensors
            if (Math.Abs(body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.ShoulderRight].Position.Z) > Properties.Settings.Default.SholderFacingDistance)
            {
                //Debug.WriteLine("GesturePart 1 - Facing sensor - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 1 - Facing sensor - Pass");

            // Body is in right distance
            if (body.Joints[JointType.Head].Position.Z < Properties.Settings.Default.GestureMinDistance || body.Joints[JointType.Head].Position.Z > Properties.Settings.Default.GestureMaxDistance)
            {
                //Debug.WriteLine("GesturePart 1 - Body is in right distance - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 1 - Body is in right distance - Pass");


            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandTipLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z)
            {
                // Debug.WriteLine("GesturePart 1 - left hand in front of left Shoulder - PASS");
                // /left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandTipLeft].Position.Y < body.Joints[JointType.Head].Position.Y)
                {
                    // Debug.WriteLine("GesturePart 1 - left hand below shoulder height but above hip height - PASS");
                    // //left hand left of left Shoulder
                    if (body.Joints[JointType.HandTipLeft].Position.X < body.Joints[JointType.SpineShoulder].Position.X && body.Joints[JointType.HandTipLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        // Debug.WriteLine("GesturePart 1 - left hand left of left Shoulder - PASS");
                        return GesturePartResult.Suceed;
                    }

                    // Debug.WriteLine("GesturePart 1 - left hand left of left Shoulder - UNDETERMINED");
                    return GesturePartResult.Pausing;
                }

                // Debug.WriteLine("GesturePart 1 - left hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Fail;
            }

            // Debug.WriteLine("GesturePart 1 - left hand in front of left Shoulder - FAIL");
            return GesturePartResult.Fail;
        }
    }
}