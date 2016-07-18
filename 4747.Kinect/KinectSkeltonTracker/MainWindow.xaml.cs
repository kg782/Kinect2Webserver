// -----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft Limited">
//  Copyright (c) Microsoft Limited, Microsoft Consulting Services, UK. All rights reserved.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>Interaction logic for MainWindow.xaml</summary>
//-----------------------------------------------------------------------
namespace KinectSkeltonTracker
{
    #region using...

    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Bitmap to display
        /// </summary>
        private WriteableBitmap colorBitmap = null;

        /// <summary>
        /// Intermediate storage for receiving frame data from the sensor
        /// </summary>
        private byte[] colorPixels = null;

        /// <summary>
        /// Size of the RGB pixel in the bitmap
        /// </summary>
        private readonly uint bytesPerPixel = 0;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel model = new MainViewModel();
            this.DataContext = model;
            this.BodyControl.ItemsSource = model.Bodys;

            FrameDescription colorFrameDescription = model.Kinect.colorFrameDescription;

            this.bytesPerPixel = colorFrameDescription.BytesPerPixel;

            this.colorPixels = new byte[colorFrameDescription.Width * colorFrameDescription.Height * this.bytesPerPixel];

            this.colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            model.Kinect.ImageFrameReady += new EventHandler<ColorFrameArrivedEventArgs>(this.Kinect_ImageFrameReady);
        }

        /// <summary>
        /// Handles the ImageFrameReady event of the kinect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Kinect.ImageFrameReadyEventArgs"/> instance containing the event data.</param>
        private void Kinect_ImageFrameReady(object sender, ColorFrameArrivedEventArgs e)
        {
            bool colorFrameProcessed = false;

            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    // verify data and write the new color frame data to the display bitmap
                    if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                    {
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            colorFrame.CopyRawFrameDataToArray(this.colorPixels);
                        }
                        else
                        {
                            colorFrame.CopyConvertedFrameDataToArray(this.colorPixels, ColorImageFormat.Bgra);
                        }

                        colorFrameProcessed = true;
                    }
                }
            }

            // we got a frame, render
            if (colorFrameProcessed)
            {
                this.RenderColorPixels();
            }

            //if (image != null)
            //{
            //    byte[] pixeldata = new byte[image.PixelDataLength];
            //    image.CopyPixelDataTo(pixeldata);
            //    cameraFeed.Source = BitmapSource.Create(
            //     image.Width,
            //     image.Height,
            //     96, 96,
            //     PixelFormats.Bgr32,
            //     null,
            //     pixeldata,
            //     image.Width * image.BytesPerPixel);
            //}


            //ColorImageFrame image = e.OpenColorImageFrame();
            //cameraFeed.Source = BitmapSource.Create(image.Width, image.Height, 96, 96, PixelFormats.Bgr32, null, image.Bits, image.Width * image.BytesPerPixel);
        }

        private void RenderColorPixels()
        {
            this.colorBitmap.WritePixels(
                new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                this.colorPixels,
                this.colorBitmap.PixelWidth * (int)this.bytesPerPixel,
                0);

            cameraFeed.Source = this.colorBitmap;
        }
    }
}
