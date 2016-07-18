// -----------------------------------------------------------------------
// <copyright file="WaveLeftSegment1.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>the first part of the wave left gesture</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;

    #endregion

    /// <summary>
    /// the first part of the wave left gesture
    /// </summary>
    public class WaveLeftSegment1 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body)
        {
            // hand above elbow
            if (body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y)
            {
                // hand right of elbow
                if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ElbowLeft].Position.X)
                {
                    return GesturePartResult.Suceed;
                }

                // hand has not dropped but is not quite where we expect it to be, pausing till next frame
                return GesturePartResult.Pausing;
            }

            // hand dropped - no gesture fails
            return GesturePartResult.Fail;
        }
    }
}
