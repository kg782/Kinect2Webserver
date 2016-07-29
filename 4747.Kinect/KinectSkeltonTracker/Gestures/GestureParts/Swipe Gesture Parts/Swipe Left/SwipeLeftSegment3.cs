// -----------------------------------------------------------------------
// <copyright file="SwipeLeftSegment3.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The third part of the swipe left gesture</summary>
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
    /// The third part of the swipe left gesture
    /// </summary>
    public class SwipeLeftSegment3 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body, List<Body> bodyHistory)
        {
            // Facing the sensors
            if (Math.Abs(body.Joints[JointType.ShoulderLeft].Position.Z - body.Joints[JointType.ShoulderRight].Position.Z) > Properties.Settings.Default.SholderFacingDistance)
            {
                //Debug.WriteLine("GesturePart 2 - Facing sensor - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 2 - Facing sensor - Pass");

            // Body is in right distance
            if (body.Joints[JointType.Head].Position.Z < Properties.Settings.Default.GestureMinDistance || body.Joints[JointType.Head].Position.Z > Properties.Settings.Default.GestureMaxDistance)
            {
                //Debug.WriteLine("GesturePart 2 - Body is in right distance - Fail");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 2 - Body is in right distance - Pass");


            // //Right hand in front of right Shoulder
            if (body.Joints[JointType.HandTipRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //Debug.WriteLine("GesturePart 2 - Right hand in front of right shoulder - PASS");
                // //right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandTipRight].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandTipRight].Position.Y > body.Joints[JointType.HipRight].Position.Y)
                {
                    //Debug.WriteLine("GesturePart 2 - right hand below shoulder height but above hip height - PASS");
                    // //right hand left of left Shoulder
                    if (body.Joints[JointType.HandTipRight].Position.X < body.Joints[JointType.SpineShoulder].Position.X)
                    {
                        //Debug.WriteLine("GesturePart 2 - right hand left of left Shoulder - PASS");
                        return GesturePartResult.Suceed;
                    }

                    //Debug.WriteLine("GesturePart 2 - right hand left of right Shoulder - UNDETERMINED");
                    return GesturePartResult.Pausing;
                }

                //Debug.WriteLine("GesturePart 2 - right hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 2 - Right hand in front of right Shoulder - FAIL");
            return GesturePartResult.Fail;
        }
    }
}