// -----------------------------------------------------------------------
// <copyright file="BodyFrameEventArgs.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The body frame event args</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The body frame event args
    /// </summary>
    public class BodyFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodyFrameEventArgs"/> class.
        /// </summary>
        /// <param name="bodyIDValues">The body ID values.</param>
        /// <param name="timeStamp">The time stamp.</param>
        public BodyFrameEventArgs(List<ulong> bodyIDValues, double timeStamp)
        {
            this.BodyIDValues = bodyIDValues;
            this.TimeStamp = timeStamp;
        }

        /// <summary>
        /// Gets or sets the body ID values.
        /// </summary>
        /// <value>
        /// The body ID values.
        /// </value>
        public List<ulong> BodyIDValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public double TimeStamp
        {
            get;
            set;
        }
    }
}
