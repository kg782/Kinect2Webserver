// -----------------------------------------------------------------------
// <copyright file="BodyViewModel.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The body view model</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.ComponentModel;
    using System.Windows.Media;
    using System.Windows.Threading;
    using KinectSkeltonTracker.Gestures;
    using KinectSkeltonTracker.Gestures.GestureParts;
    using Microsoft.Kinect;

    #endregion

    /// <summary>
    /// The body view model
    /// </summary>
    public class BodyViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The gesture controler for this body
        /// </summary>
        private GestureControler gestures = new GestureControler();

        #region fields

        /// <summary>
        /// backing field for the gesture text
        /// </summary>
        private string gestureText;

        /// <summary>
        /// backing field for the gesture property
        /// </summary>
        private bool gestureDetected;

        /// <summary>
        /// The body data
        /// This is used as the backing field for all properties
        /// </summary>
        private Body body;

        /// <summary>
        /// backing field for the joint color
        /// </summary>
        private Color jointColor;

        /// <summary>
        /// the timer for the on screen gesture text
        /// </summary>
        private DispatcherTimer textTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(3) };

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyViewModel"/> class.
        /// </summary>
        public BodyViewModel()
        {
            this.ChangeTimer = new DispatcherTimer();
            this.ChangeTimer.Interval = TimeSpan.FromSeconds(0.5);
            this.ChangeTimer.Tick += new EventHandler(this.ChangeTimer_Tick);
            this.textTimer.Tick += new EventHandler(this.TextTimer_Tick);
            this.ChangeTimer.Start();
            this.DefineGestures();
            this.gestures.GestureRecognised += new EventHandler<GestureEventArgs>(this.Gestures_GestureRecognised);
        }

        #endregion

        #region events

        /// <summary>
        /// Occurs when [delete body].
        /// </summary>
        public event EventHandler DeleteBody;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the change timer.
        /// </summary>
        /// <value>
        /// The change timer.
        /// </value>
        public DispatcherTimer ChangeTimer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gesture text.
        /// </summary>
        /// <value>
        /// The gesture text.
        /// </value>
        public string GestureText
        {
            get
            {
                return this.gestureText;
            }

            set
            {
                if (this.gestureText != value)
                {
                    this.gestureText = value;
                    this.NotifyPropertyChanged("GestureText");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BodyViewModel"/> is waved.
        /// </summary>
        /// <value>
        ///   <c>true</c> if waved; otherwise, <c>false</c>.
        /// </value>
        public bool GestureDetected
        {
            get
            {
                return this.gestureDetected;
            }

            set
            {
                if (this.gestureDetected != value)
                {
                    this.gestureDetected = value;
                    this.NotifyPropertyChanged("GestureDetected");
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the joint.
        /// </summary>
        /// <value>
        /// The color of the joint.
        /// </value>
        public Color JointColor
        {
            get
            {
                return this.jointColor;
            }

            set
            {
                if (this.jointColor != value)
                {
                    this.jointColor = value;
                    this.NotifyPropertyChanged("JointColor");
                }
            }
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public Body Body
        {
            get
            {
                return this.body;
            }

            set
            {
                if (this.body != value)
                {
                    this.body = value;
                    this.NotifyPropertyChanged("Body");
                    this.NotifyAllChange();
                    this.ResetTimer();
                    this.gestures.UpdateAllGestures(this.body);
                }
            }
        }

        #region KinectBodyParts

        /// <summary>
        /// Gets the head.
        /// </summary>
        public Joint Head
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.Head];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left hand.
        /// </summary>
        public Joint LeftHand
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.HandLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right hand.
        /// </summary>
        public Joint RightHand
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.HandRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left wrist.
        /// </summary>
        public Joint LeftWrist
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.WristLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right wrist.
        /// </summary>
        public Joint RightWrist
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.WristRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left elbow.
        /// </summary>
        public Joint LeftElbow
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.ElbowLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right elbow.
        /// </summary>
        public Joint RightElbow
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.ElbowRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the shoulder center.
        /// </summary>
        public Joint ShoulderCenter
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.SpineShoulder];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the spine.
        /// </summary>
        public Joint Spine
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.SpineMid];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the hip center.
        /// </summary>
        public Joint HipCenter
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.SpineBase];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left knee.
        /// </summary>
        public Joint LeftKnee
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.KneeLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right knee.
        /// </summary>
        public Joint RightKnee
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.KneeRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left ankle.
        /// </summary>
        public Joint LeftAnkle
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.AnkleLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right ankle.
        /// </summary>
        public Joint RightAnkle
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.AnkleRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the left foot.
        /// </summary>
        public Joint LeftFoot
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.FootLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the right foot.
        /// </summary>
        public Joint RightFoot
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.FootRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the Left Hip
        /// </summary>
        public Joint RightHip
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.HipRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the Left Hip
        /// </summary>
        public Joint LeftHip
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.HipLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the Right Shoulder
        /// </summary>
        public Joint RightShoulder
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.ShoulderRight];
                }
                else
                {
                    return new Joint();
                }
            }
        }

        /// <summary>
        /// Gets the Left Shoulder
        /// </summary>
        public Joint LeftShoulder
        {
            get
            {
                if (this.Body != null)
                {
                    return this.Body.Joints[JointType.ShoulderLeft];
                }
                else
                {
                    return new Joint();
                }
            }
        }
        #endregion

        #endregion

        #region methods

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void ResetTimer()
        {
            this.ChangeTimer.Stop();
            this.ChangeTimer.Start();
        }

        /// <summary>
        /// Handles the Tick event of the textTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TextTimer_Tick(object sender, EventArgs e)
        {
            this.GestureDetected = false;
            this.textTimer.Stop();
        }

        /// <summary>
        /// Handles the GestureRecognised event of the Gestures control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KinectSkeltonTracker.GestureEventArgs"/> instance containing the event data.</param>
        private void Gestures_GestureRecognised(object sender, GestureEventArgs e)
        {
            if (e.GestureType == GestureType.WaveRight)
            {
                this.GestureDetected = true;
                this.GestureText = "Waved with right hand";
                this.textTimer.Start();
            }
            else if (e.GestureType == GestureType.WaveLeft)
            {
                this.GestureDetected = true;
                this.GestureText = "Waved with left hand";
                this.textTimer.Start();
            }
            else if (e.GestureType == GestureType.LeftSwipe)
            {
                this.GestureDetected = true;
                this.GestureText = "Swiped left";
                this.textTimer.Start();
            }
            else if (e.GestureType == GestureType.RightSwipe)
            {
                this.GestureDetected = true;
                this.GestureText = "Swiped right";
                this.textTimer.Start();
            }
            else if (e.GestureType == GestureType.Menu)
            {
                this.GestureDetected = true;
                this.GestureText = "Menu";
                this.textTimer.Start();
            }
        }

        /// <summary>
        /// Defines the gestures.
        /// </summary>
        private void DefineGestures()
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
            this.gestures.AddGesture(GestureType.WaveRight, waveRightSegments);

            IRelativeGestureSegment[] waveLeftSegments = new IRelativeGestureSegment[6];
            WaveLeftSegment1 waveLeftSegment1 = new WaveLeftSegment1();
            WaveLeftSegment2 waveLeftSegment2 = new WaveLeftSegment2();
            waveLeftSegments[0] = waveLeftSegment1;
            waveLeftSegments[1] = waveLeftSegment2;
            waveLeftSegments[2] = waveLeftSegment1;
            waveLeftSegments[3] = waveLeftSegment2;
            waveLeftSegments[4] = waveLeftSegment1;
            waveLeftSegments[5] = waveLeftSegment2;
            this.gestures.AddGesture(GestureType.WaveLeft, waveLeftSegments);

            IRelativeGestureSegment[] swipeleftSegments = new IRelativeGestureSegment[3];
            swipeleftSegments[0] = new SwipeLeftSegment1();
            swipeleftSegments[1] = new SwipeLeftSegment2();
            swipeleftSegments[2] = new SwipeLeftSegment3();
            this.gestures.AddGesture(GestureType.LeftSwipe, swipeleftSegments);

            IRelativeGestureSegment[] swiperightSegments = new IRelativeGestureSegment[3];
            swiperightSegments[0] = new SwipeRightSegment1();
            swiperightSegments[1] = new SwipeRightSegment2();
            swiperightSegments[2] = new SwipeRightSegment3();
            this.gestures.AddGesture(GestureType.RightSwipe, swiperightSegments);

            IRelativeGestureSegment[] menuSegments = new IRelativeGestureSegment[20];
            MenuSegments1 menuSegment = new MenuSegments1();
            for (int i = 0; i < 20; i++)
            {
                // gesture consists of the same thing 20 times 
                menuSegments[i] = menuSegment;
            }

            this.gestures.AddGesture(GestureType.Menu, menuSegments);
        }

        /// <summary>
        /// Handles the Tick event of the ChangeTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ChangeTimer_Tick(object sender, EventArgs e)
        {
            if (this.DeleteBody != null)
            {
                this.DeleteBody(this, null);
            }
        }

        /// <summary>
        /// Notifies all change.
        /// </summary>
        private void NotifyAllChange()
        {
            this.NotifyPropertyChanged("Head");
            this.NotifyPropertyChanged("LeftHand");
            this.NotifyPropertyChanged("LeftHandX");
            this.NotifyPropertyChanged("LeftHandY");
            this.NotifyPropertyChanged("RightHand");
            this.NotifyPropertyChanged("LeftWrist");
            this.NotifyPropertyChanged("RightWrist");
            this.NotifyPropertyChanged("LeftElbow");
            this.NotifyPropertyChanged("RightElbow");
            this.NotifyPropertyChanged("ShoulderCenter");
            this.NotifyPropertyChanged("HipCenter");
            this.NotifyPropertyChanged("LeftKnee");
            this.NotifyPropertyChanged("RightKnee");
            this.NotifyPropertyChanged("LeftAnkle");
            this.NotifyPropertyChanged("RightAnkle");
            this.NotifyPropertyChanged("LeftFoot");
            this.NotifyPropertyChanged("RightFoot");
            this.NotifyPropertyChanged("Spine");
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
