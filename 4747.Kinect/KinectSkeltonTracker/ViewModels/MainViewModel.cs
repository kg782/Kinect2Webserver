// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>The main view model</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Media;

    #endregion

    /// <summary>
    /// The main view model
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The available colors for the body
        /// </summary>
        private static readonly Color[] availableColors = { Colors.Red, Colors.Blue, Colors.Green, Colors.Orange, Colors.Purple };

        /// <summary>
        /// The current color id
        /// </summary>
        private static int colorID = 0;

        /// <summary>
        /// the dictionary of bodys
        /// </summary>
        private Dictionary<ulong, BodyViewModel> bodyDictionary = new Dictionary<ulong, BodyViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.Kinect = new KinectConnection();
            this.Bodys = new ObservableCollection<BodyViewModel>();
            this.Kinect.BodyReady += new EventHandler<BodyEventArgs>(this.Kinect_BodyReady);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the kinect connection.
        /// </summary>
        /// <value>
        /// The kinect connection.
        /// </value>
        public KinectConnection Kinect
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bodys.
        /// </summary>
        /// <value>
        /// The bodys.
        /// </value>
        public ObservableCollection<BodyViewModel> Bodys
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the BodyReady event of the kinect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KinectSkeltonTracker.BodyEventArgs"/> instance containing the event data.</param>
        private void Kinect_BodyReady(object sender, BodyEventArgs e)
        {
            if (this.bodyDictionary.ContainsKey(e.Body.TrackingId))
            {
                this.bodyDictionary[e.Body.TrackingId].Body = e.Body;
                this.bodyDictionary[e.Body.TrackingId].ResetTimer();
            }
            else
            {
                BodyViewModel viewModel = new BodyViewModel();
                viewModel.Body = e.Body;
                viewModel.JointColor = availableColors[colorID];
                if (colorID == availableColors.Length - 1)
                {
                    colorID = 0;
                }
                else
                {
                    colorID += 1;
                }

                this.bodyDictionary.Add(e.Body.TrackingId, viewModel);
                this.Bodys.Add(viewModel);
                viewModel.DeleteBody += new EventHandler(this.ViewModel_DeleteBody);
            }
        }

        /// <summary>
        /// Handles the DeleteBody event of the ViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ViewModel_DeleteBody(object sender, EventArgs e)
        {
            BodyViewModel model = sender as BodyViewModel;
            if (model != null)
            {
                this.Bodys.Remove(model);
                this.bodyDictionary.Remove(model.Body.TrackingId);
            }
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
    }
}
