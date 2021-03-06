﻿// -----------------------------------------------------------------------
// <copyright file="GestureControler.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The gesture controler</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker.Gestures
{
    #region using...

    using System;
    using System.Collections.Generic;
    using Microsoft.Kinect;
    using Microsoft.Samples.Kinect.Webserver;

    #endregion

    /// <summary>
    /// The gesture controler
    /// </summary>
    public class GestureControler
    {
        /// <summary>
        /// The list of all gestures we are currently looking for
        /// </summary>
        private List<Gesture> gestures = new List<Gesture>();

        /// <summary>
        /// History of bodies
        /// </summary>
        private List<object> bodyHistory = new List<object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureControler"/> class.
        /// </summary>
        public GestureControler()
        {
        }

        /// <summary>
        /// Occurs when [gesture recognised].
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognised;

        /// <summary>
        /// Updates all gestures.
        /// </summary>
        /// <param name="data">The body data.</param>
        public void UpdateAllGestures(Body data)
        {
            foreach (Gesture gesture in this.gestures)
            {
                gesture.UpdateGesture(data, bodyHistory);
            }

            // Add body to history
            var maxHistory = 100;
            if (bodyHistory.Count >= maxHistory)
            {
                bodyHistory.RemoveAt(maxHistory - 1);
            }

            // Serialize Body because the instance is shared across frames
            object serializedBody = JsonSerializationExtensions.ExtractSerializableJsonData(data);
            bodyHistory.Insert(0, serializedBody);
        }

        /// <summary>
        /// Adds the gesture.
        /// </summary>
        /// <param name="type">The gesture type.</param>
        /// <param name="gestureDefinition">The gesture definition.</param>
        public void AddGesture(GestureType type, IRelativeGestureSegment[] gestureDefinition)
        {
            Gesture gesture = new Gesture(type, gestureDefinition);
            gesture.GestureRecognised += new EventHandler<GestureEventArgs>(this.Gesture_GestureRecognised);
            this.gestures.Add(gesture);
        }

        /// <summary>
        /// Handles the GestureRecognised event of the g control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KinectSkeltonTracker.GestureEventArgs"/> instance containing the event data.</param>
        private void Gesture_GestureRecognised(object sender, GestureEventArgs e)
        {
            if (this.GestureRecognised != null)
            {
                this.GestureRecognised(this, e);
            }

            foreach (Gesture g in this.gestures)
            {
                g.Reset();
            }
        }
    }
}
