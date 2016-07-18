//-----------------------------------------------------------------------
// <copyright file="KinectConnection.cs" company="Microsoft Limited">
// Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The kinect connection class</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.Collections.Generic;
    using System.Windows.Documents;
    using Microsoft.Kinect;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// The kinect connection class
    /// </summary>
    public class KinectConnection
    {
        /// <summary>
        /// The list of approved camera angles
        /// </summary>
        private static readonly int[] cameraAngles = { -25, -23, -21, -19, -17, -15, -13, -11, -9, -7, -5, -3, -1, 0, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25 };

        /// <summary>
        /// The Kinect run time
        /// </summary>
        private KinectSensor kinectSensor = KinectSensor.GetDefault();

        /// <summary>
        /// The current index of the angle that the sensor is at
        /// </summary>
        //private int currentAngle;
        
        /// <summary>
        /// Reader for body frames
        /// </summary>
        public BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Reader for color frames
        /// </summary>
        public ColorFrameReader colorFrameReader = null;

        /// <summary>
        /// Reader for depth frames
        /// </summary>
        public DepthFrameReader depthFrameReader = null;

        /// <summary>
        /// Reader for depth frames
        /// </summary>
        public FrameDescription colorFrameDescription = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="KinectConnection"/> class.
        /// </summary>
        public KinectConnection()
        {
            if (this.InitializeNui())
            {
                this.bodyFrameReader.FrameArrived += this.KinectRunTime_BodyFrameReady;
                this.colorFrameReader.FrameArrived += this.KinectRunTime_VideoFrameReady;


                //if (this.kinectSensor.ElevationAngle != cameraAngles[13])
                //{
                //    this.kinectSensor.ElevationAngle = cameraAngles[13];
                //    this.currentAngle = 12;
                //}
            }
            else
            {
                throw new Exception("Error initialising Kinect sensor");
            }
        }

        /// <summary>
        /// Occurs when [body ready].
        /// </summary>
        public event EventHandler<BodyEventArgs> BodyReady;

        /// <summary>
        /// Occurs when [image frame ready].
        /// </summary>
        public event EventHandler<ColorFrameArrivedEventArgs> ImageFrameReady;

        /// <summary>
        /// Occurs when [body frame complete].
        /// </summary>
        public event EventHandler<BodyFrameEventArgs> BodyFrameComplete;

        /// <summary>
        /// Tilts the camera up.
        /// </summary>
        /// <returns> bool value true if the sensore moved</returns>
        public bool TiltUp()
        {
            return false;
            //if (this.currentAngle >= cameraAngles.Length)
            //{
            //    return false;
            //}

            //try
            //{
            //    this.currentAngle += 1;
            //    this.kinectSensor.ElevationAngle = cameraAngles[this.currentAngle];
            //    return true;
            //}
            //catch (InvalidOperationException)
            //{
            //    this.currentAngle -= 1;
            //    return false;
            //}
        }

        /// <summary>
        /// Tilts the camera down.
        /// </summary>
        /// <returns>bool value true if the sensore moved</returns>
        public bool TiltDown()
        {
            return false;
            //if (this.currentAngle <= 0)
            //{
            //    return false;
            //}

            //try
            //{
            //    this.currentAngle -= 1;
            //    this.kinectSensor.ElevationAngle = cameraAngles[this.currentAngle];
            //    return true;
            //}
            //catch (InvalidOperationException)
            //{
            //    this.currentAngle += 1;
            //    return false;
            //}
        }

        /// <summary>
        /// Handles the ColorFrameReady event of the kinectSensor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Kinect.ImageFrameReadyEventArgs"/> instance containing the event data.</param>
        private void KinectRunTime_VideoFrameReady(object sender, ColorFrameArrivedEventArgs e)
        {
            if (this.ImageFrameReady != null)
            {
                this.ImageFrameReady(this, e);
            }
        }

        /// <summary>
        /// Handles the BodyFrameReady event of the kinectSensor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Kinect.BodyFrameReadyEventArgs"/> instance containing the event data.</param>
        private void KinectRunTime_BodyFrameReady(object sender, BodyFrameArrivedEventArgs e)
        {
            List<ulong> idValues = new List<ulong>();

            bool dataReceived = false;
            Body[] bodys = null;
            double timestamp = 0.0;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodys = new Body[bodyFrame.BodyCount];

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(bodys);
                    timestamp = bodyFrame.RelativeTime.TotalMilliseconds;
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                int trackingCount = 0;

                while (trackingCount < bodys.Length)
                {
                    if (bodys[trackingCount].IsTracked)
                    {
                        if (this.BodyReady != null)
                        {
                            this.BodyReady(this, new BodyEventArgs(bodys[trackingCount]));
                        }

                        idValues.Add(bodys[trackingCount].TrackingId);
                    }

                    trackingCount++;
                    
                    if (this.BodyFrameComplete != null)
                    {
                        this.BodyFrameComplete(this, new BodyFrameEventArgs(idValues, timestamp));
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the Kinect sensor.
        /// </summary>
        /// <returns>bool value true if the sensor initialised correctly</returns>
        private bool InitializeNui()
        {
            if (this.kinectSensor == null)
            {
                return false;
            }

            try
            {
                this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

                this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();
                this.colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

                this.depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();

                this.kinectSensor.Open();

                //this.kinectSensor.DepthStream.Enable();

                //var parameters = new TransformSmoothParameters
                //    {
                //        Smoothing = 0.75f,
                //        Correction = 0.0f,
                //        Prediction = 0.0f,
                //        JitterRadius = 0.05f,
                //        MaxDeviationRadius = 0.04f
                //    };
                //this.kinectSensor.BodyStream.Enable(parameters);
                //this.kinectSensor.ColorStream.Enable();
                //this.kinectSensor.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return false;
            }

            return true;
        }
    }
}
