using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Samples.Kinect.Webserver.Sensor.Serialization;
using Microsoft.Kinect;
using Microsoft.Samples.Kinect.Webserver;
using KinectSkeltonTracker.Gestures;

namespace Microsoft.Samples.Kinect.WebserverBasics.Sensor.Serialization
{
    /// <summary>
    /// Serializable representation of an body stream message to send to client.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
        Justification = "Lower case names allowed for JSON serialization.")]
    class GestureStreamMessage : StreamMessage
    {
        /// <summary>
        /// Gesture type.
        /// </summary>
        public GestureType gestureType { get; set; }

        /// <summary>
        /// Tracking ID of the body.
        /// </summary>
        public ulong trackingId { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public object body { get; set; }

        public void UpdateBody(Body body)
        {
            if (body != null)
            {
                this.body = JsonSerializationExtensions.ExtractSerializableJsonData(body);
            }
        }
    }
}
