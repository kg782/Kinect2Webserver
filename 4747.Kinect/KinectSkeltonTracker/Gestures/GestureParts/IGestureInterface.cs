// -----------------------------------------------------------------------
// <copyright file="IGestureInterface.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>Description</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures
{
    #region using...

    using Microsoft.Kinect;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Defines a single gesture segment which uses relative positioning 
    /// of body parts to detect a gesture
    /// </summary>
    public interface IRelativeGestureSegment
    {
        /// <summary>
        /// Checks the gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>GesturePartResult based on if the gesture part has been completed</returns>
        GesturePartResult CheckGesture(Body body, List<Body> bodyHistory);
    }
}
