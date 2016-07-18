// -----------------------------------------------------------------------
// <copyright file="SwipeLeftSegment2.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The second part of the swipe left gesture</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;
    using System;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The second part of the swipe left gesture
    /// </summary>
    public class SwipeLeftSegment2 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body)
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

            // //Right hand in front of right shoulder
            if (body.Joints[JointType.HandTipRight].Position.Z < body.Joints[JointType.ElbowRight].Position.Z)
            {
                //Debug.WriteLine("GesturePart 1 - Right hand in front of right shoulder - PASS");
                // //right hand below shoulder height but above hip height
                if (body.Joints[JointType.HandTipRight].Position.Y < body.Joints[JointType.Head].Position.Y)
                {
                    //Debug.WriteLine("GesturePart 1 - right hand below shoulder height but above hip height - PASS");
                    // //right hand left of right shoulder & right of left shoulder
                    if (body.Joints[JointType.HandTipRight].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandTipRight].Position.X > body.Joints[JointType.SpineShoulder].Position.X)
                    {
                        //Debug.WriteLine("GesturePart 1 - right hand left of right shoulder & right of left shoulder - PASS");
                        return GesturePartResult.Suceed;
                    }

                    //Debug.WriteLine("GesturePart 1 - right hand left of right shoulder & right of left shoulder - UNDETERMINED");
                    return GesturePartResult.Pausing;
                }

                //Debug.WriteLine("GesturePart 1 - right hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Fail;
            }

            //Debug.WriteLine("GesturePart 1 - Right hand in front of right shoulder - FAIL");
            return GesturePartResult.Fail;
        }
    }
}