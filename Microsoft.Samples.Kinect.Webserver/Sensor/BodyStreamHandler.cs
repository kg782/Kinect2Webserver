// -----------------------------------------------------------------------
// <copyright file="BodyStreamHandler.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.Webserver.Sensor
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Kinect;
    using Microsoft.Samples.Kinect.Webserver.Sensor.Serialization;

    /// <summary>
    /// Implementation of ISensorStreamHandler that exposes body streams.
    /// </summary>
    public class BodyStreamHandler : SensorStreamHandlerBase
    {
        /// <summary>
        /// JSON name of body stream.
        /// </summary>
        internal const string BodyStreamName = "body";

        /// <summary>
        /// Context that allows this stream handler to communicate with its owner.
        /// </summary>
        private readonly SensorStreamHandlerContext ownerContext;

        /// <summary>
        /// Serializable body stream message, reused as body frames arrive.
        /// </summary>
        private readonly BodyStreamMessage skeletonStreamMessage = new BodyStreamMessage { stream = BodyStreamName };

        /// <summary>
        /// true if body stream is enabled. Body stream is disabled by default.
        /// </summary>
        private bool skeletonIsEnabled;

        /// <summary>
        /// Keep track if we're in the middle of processing an body frame.
        /// </summary>
        private bool isProcessingbodyFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyStreamHandler"/> class
        /// and associates it with a context that allows it to communicate with its owner.
        /// </summary>
        /// <param name="ownerContext">
        /// An instance of <see cref="SensorStreamHandlerContext"/> class.
        /// </param>
        internal BodyStreamHandler(SensorStreamHandlerContext ownerContext)
        {
            this.ownerContext = ownerContext;

            this.AddStreamConfiguration(BodyStreamName, new StreamConfiguration(this.GetProperties, this.SetProperty));
        }

        /// <summary>
        /// Process data from one Kinect body frame.
        /// </summary>
        /// <param name="bodies">
        /// Kinect body data.
        /// </param>
        /// <param name="bodyFrame">
        /// <see cref="BodyFrame"/> from which we obtained body data.
        /// </param>
        public override void ProcessBody(Body[] bodies, BodyFrame bodyFrame)
        {
            if (bodyFrame == null)
            {
                throw new ArgumentNullException("bodyFrame");
            }

            this.ProcessBodyAsync(bodies, bodyFrame.RelativeTime.TotalSeconds);
        }

        /// <summary>
        /// Process bodies in async mode.
        /// </summary>
        /// <param name="bodies">
        /// Kinect body data.
        /// </param>
        /// <param name="timestamp">
        /// Timestamp of <see cref="BodyFrame"/> from which we obtained body data.
        /// </param>
        internal async void ProcessBodyAsync(Body[] bodies, double timestamp)
        {
            if (!this.skeletonIsEnabled)
            {
                return;
            }

            if (this.isProcessingbodyFrame)
            {
                // Re-entered bodyFrameReadyAsync while a previous frame is already being processed.
                // Just ignore new frames until the current one finishes processing.
                return;
            }

            this.isProcessingbodyFrame = true;

            try
            {
                if (bodies != null)
                {
                    this.skeletonStreamMessage.timestamp = timestamp;
                    this.skeletonStreamMessage.UpdateBodies(bodies);

                    await this.ownerContext.SendStreamMessageAsync(this.skeletonStreamMessage);
                }
            }
            finally
            {
                this.isProcessingbodyFrame = false;
            }
        }

        /// <summary>
        /// Gets a body stream property value.
        /// </summary>
        /// <param name="propertyMap">
        /// Property name->value map where property values should be set.
        /// </param>
        private void GetProperties(Dictionary<string, object> propertyMap)
        {
            propertyMap.Add(KinectRequestHandler.EnabledPropertyName, this.skeletonIsEnabled);
        }

        /// <summary>
        /// Set a body stream property value.
        /// </summary>
        /// <param name="propertyName">
        /// Name of property to set.
        /// </param>
        /// <param name="propertyValue">
        /// Property value to set.
        /// </param>
        /// <returns>
        /// null if property setting was successful, error message otherwise.
        /// </returns>
        private string SetProperty(string propertyName, object propertyValue)
        {
            bool recognized = true;

            if (propertyValue == null)
            {
                // None of the body stream properties accept a null value
                return Properties.Resources.PropertyValueInvalidFormat;
            }

            try
            {
                switch (propertyName)
                {
                    case KinectRequestHandler.EnabledPropertyName:
                        this.skeletonIsEnabled = (bool)propertyValue;
                        break;

                    default:
                        recognized = false;
                        break;
                }
            }
            catch (InvalidCastException)
            {
                return Properties.Resources.PropertyValueInvalidFormat;
            }

            if (!recognized)
            {
                return Properties.Resources.PropertyNameUnrecognized;
            }

            return null;
        }
    }
}
