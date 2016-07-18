using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Samples.Kinect.Webserver;
using Microsoft.Samples.Kinect.Webserver.Properties;
using Microsoft.Samples.Kinect.Webserver.Sensor;
using Microsoft.Samples.Kinect.WebserverBasics.Sensor.Serialization;
using Microsoft.Kinect;
using KinectSkeltonTracker.Gestures;
using KinectSkeltonTracker.Gestures.GestureParts;
using System.Diagnostics;

namespace Microsoft.Samples.Kinect.WebserverBasics.Sensor
{
    class GestureStreamHandler : SensorStreamHandlerBase
    {
        /// <summary>
        /// JSON name of gesture stream.
        /// </summary>
        internal const string GestureStreamName = "gesture";

        /// <summary>
        /// Context that allows this stream handler to communicate with its owner.
        /// </summary>
        private readonly SensorStreamHandlerContext ownerContext;

        /// <summary>
        /// Serializable body stream message, reused as body frames arrive.
        /// </summary>
        private readonly GestureStreamMessage gestureStreamMessage = new GestureStreamMessage { stream = GestureStreamName };

        /// <summary>
        /// true if body stream is enabled. Body stream is disabled by default.
        /// </summary>
        private bool gestureIsEnabled;

        /// <summary>
        /// Keep track if we're in the middle of processing an body frame.
        /// </summary>
        private bool isProcessingGestureFrame;

        /// <summary>
        /// The dictionary of gesture controllers
        /// </summary>
        private Dictionary<ulong, GestureControler> gestureDictionary = new Dictionary<ulong, GestureControler>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureStreamHandler"/> class
        /// and associates it with a context that allows it to communicate with its owner.
        /// </summary>
        /// <param name="ownerContext">
        /// An instance of <see cref="SensorStreamHandlerContext"/> class.
        /// </param>
        internal GestureStreamHandler(SensorStreamHandlerContext ownerContext)
        {
            this.ownerContext = ownerContext;

            this.AddStreamConfiguration(GestureStreamName, new StreamConfiguration(this.GetProperties, this.SetProperty));

        }

        /// <summary>
        /// Process data from one Kinect body frame.
        /// </summary>
        /// <param name="bodies">
        /// Kinect body data.
        /// </param>
        /// <param name="bodyFrame">
        /// <see cref="bodyFrame"/> from which we obtained body data.
        /// </param>
        public override void ProcessBody(Body[] bodies, BodyFrame bodyFrame)
        {
            if (bodyFrame == null)
            {
                throw new ArgumentNullException("bodyFrame");
            }

            //this.ProcessBodyAsync(bodies, bodyFrame.Timestamp);

            // Update all bodies
            for (var i = 0; i < bodies.Length; i++)
            {
                var body = bodies[i];
                if (body.IsTracked)
                {
                    UpdateGestures(bodies[i]);
                }
            }

            // Clean up old bodies
            foreach (KeyValuePair<ulong, GestureControler> entry in this.gestureDictionary)
            {
                var i = Array.FindIndex(bodies, s => s.TrackingId == entry.Key);
                if (i < 0) {
                    this.gestureDictionary.Remove(entry.Key);
                    Debug.WriteLine("Body was removed: " + entry.Key);
                }
            }
        }

        private void UpdateGestures(Body body)
        {
            if (this.gestureDictionary.ContainsKey(body.TrackingId))
            {
                this.gestureDictionary[body.TrackingId].UpdateAllGestures(body);
            }
            else
            {
                GestureControler gestures = new GestureControler();

                DefineGestures(gestures);

                gestures.GestureRecognised += new EventHandler<KinectSkeltonTracker.GestureEventArgs>(this.onGestureRecognised);
                gestures.UpdateAllGestures(body);

                this.gestureDictionary.Add(body.TrackingId, gestures);
                Debug.WriteLine("Body was added: " + body.TrackingId);
            }
        }

        private void DefineGestures(GestureControler gestures)
        {
            IRelativeGestureSegment[] waveRightSegments = new IRelativeGestureSegment[6];
            WaveRightSegment1 waveRightSegment1 = new WaveRightSegment1();
            WaveRightSegment2 waveRightSegment2 = new WaveRightSegment2();
            waveRightSegments[0] = waveRightSegment1;
            waveRightSegments[1] = waveRightSegment2;
            waveRightSegments[2] = waveRightSegment1;
            waveRightSegments[3] = waveRightSegment2;
            waveRightSegments[4] = waveRightSegment1;
            waveRightSegments[5] = waveRightSegment2;
            gestures.AddGesture(GestureType.WaveRight, waveRightSegments);

            IRelativeGestureSegment[] waveLeftSegments = new IRelativeGestureSegment[6];
            WaveLeftSegment1 waveLeftSegment1 = new WaveLeftSegment1();
            WaveLeftSegment2 waveLeftSegment2 = new WaveLeftSegment2();
            waveLeftSegments[0] = waveLeftSegment1;
            waveLeftSegments[1] = waveLeftSegment2;
            waveLeftSegments[2] = waveLeftSegment1;
            waveLeftSegments[3] = waveLeftSegment2;
            waveLeftSegments[4] = waveLeftSegment1;
            waveLeftSegments[5] = waveLeftSegment2;
            gestures.AddGesture(GestureType.WaveLeft, waveLeftSegments);

            IRelativeGestureSegment[] swipeleftSegments = new IRelativeGestureSegment[3];
            swipeleftSegments[0] = new SwipeLeftSegment1();
            swipeleftSegments[1] = new SwipeLeftSegment2();
            swipeleftSegments[2] = new SwipeLeftSegment3();
            gestures.AddGesture(GestureType.LeftSwipe, swipeleftSegments);

            IRelativeGestureSegment[] swiperightSegments = new IRelativeGestureSegment[3];
            swiperightSegments[0] = new SwipeRightSegment1();
            swiperightSegments[1] = new SwipeRightSegment2();
            swiperightSegments[2] = new SwipeRightSegment3();
            gestures.AddGesture(GestureType.RightSwipe, swiperightSegments);

            IRelativeGestureSegment[] menuSegments = new IRelativeGestureSegment[20];
            MenuSegments1 menuSegment = new MenuSegments1();
            for (int i = 0; i < 20; i++)
            {
                // gesture consists of the same thing 20 times 
                menuSegments[i] = menuSegment;
            }

            gestures.AddGesture(GestureType.Menu, menuSegments);
        }


        private void onGestureRecognised(object sender, KinectSkeltonTracker.GestureEventArgs e)
        {
            Debug.WriteLine("Gesture was recognized: " + e.GestureType);

            ProcessGestureAsync(e.GestureType, e.TrackingID);
        }

        /// <summary>
        /// Process bodies in async mode.
        /// </summary>
        /// <param name="bodies">
        /// Kinect body data.
        /// </param>
        /// <param name="timestamp">
        /// Timestamp of <see cref="bodyFrame"/> from which we obtained body data.
        /// </param>
        internal async void ProcessGestureAsync(GestureType gestureType, ulong trackingId)
        {
            if (!this.gestureIsEnabled)
            {
                return;
            }

            if (this.isProcessingGestureFrame)
            {
                // Re-entered bodyFrameReadyAsync while a previous frame is already being processed.
                // Just ignore new frames until the current one finishes processing.
                return;
            }

            this.isProcessingGestureFrame = true;

            try
            {
                //if (gestureType != null && trackingId != null)
                //{
                    this.gestureStreamMessage.trackingId = trackingId;
                    this.gestureStreamMessage.gestureType = gestureType;

                    await this.ownerContext.SendStreamMessageAsync(this.gestureStreamMessage);
                //}
            }
            finally
            {
                this.isProcessingGestureFrame = false;
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
            propertyMap.Add(KinectRequestHandler.EnabledPropertyName, this.gestureIsEnabled);
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
                return Webserver.Properties.Resources.PropertyValueInvalidFormat;
            }

            try
            {
                switch (propertyName)
                {
                    case KinectRequestHandler.EnabledPropertyName:
                        this.gestureIsEnabled = (bool)propertyValue;
                        break;

                    default:
                        recognized = false;
                        break;
                }
            }
            catch (InvalidCastException)
            {
                return Webserver.Properties.Resources.PropertyValueInvalidFormat;
            }

            if (!recognized)
            {
                return Webserver.Properties.Resources.PropertyNameUnrecognized;
            }

            return null;
        }

    }
}
