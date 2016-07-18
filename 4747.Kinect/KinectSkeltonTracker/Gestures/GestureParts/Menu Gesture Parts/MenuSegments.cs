// -----------------------------------------------------------------------
// <copyright file="MenuSegments.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The menu gesture segment</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;

    #endregion

    /// <summary>
    /// The menu gesture segment
    /// </summary>
    public class MenuSegments1 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // Left and right hands below hip
            if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineBase].Position.Y && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // left hand 0.3 to left of center hip
                if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.SpineBase].Position.X - 0.3)
                {
                    // left hand 0.2 to left of left elbow
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X - 0.2)
                    {
                        return GesturePartResult.Suceed;
                    }
                }

                return GesturePartResult.Pausing;
            }

            return GesturePartResult.Fail;
        }
    }
}
