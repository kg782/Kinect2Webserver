// -----------------------------------------------------------------------
// <copyright file="BodyEventArgs.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The body event args</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using Microsoft.Kinect;

    #endregion

    /// <summary>
    /// The body event args
    /// </summary>
    public class BodyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodyEventArgs"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public BodyEventArgs(Body body)
        {
            this.Body = body;
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public Body Body
        { 
            get; 
            set; 
        }
    }
}
