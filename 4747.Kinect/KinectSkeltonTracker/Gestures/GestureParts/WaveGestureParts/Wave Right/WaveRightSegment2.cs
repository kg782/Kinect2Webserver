// -----------------------------------------------------------------------
// <copyright file="WaveRightSegment2.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The second part in the wave right gesture</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures.GestureParts
{
    #region using...

    using Microsoft.Kinect;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The second part in the wave right gesture
    /// </summary>
    public class WaveRightSegment2 : IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        public GesturePartResult CheckGesture(Body body, List<object> bodyHistory)
        {
            // hand above elbow
            if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y)
            {
                // hand right of elbow
                if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.ElbowRight].Position.X)
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
