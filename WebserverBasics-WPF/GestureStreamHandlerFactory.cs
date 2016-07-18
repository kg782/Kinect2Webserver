using Microsoft.Samples.Kinect.Webserver.Sensor;
using Microsoft.Samples.Kinect.WebserverBasics.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.WebserverBasics
{
    class GestureStreamHandlerFactory : ISensorStreamHandlerFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SensorStreamHandlerFactory"/> class.
        /// </summary>
        /// <param name="streamType">The stream type.</param>
        public GestureStreamHandlerFactory()
        {
        }

        /// <summary>
        /// Creates a sensor stream handler object and associates it with a context that
        /// allows it to communicate with its owner.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="SensorStreamHandlerContext"/> class.
        /// </param>
        /// <returns>
        /// A new <see cref="ISensorStreamHandler"/> instance.
        /// </returns>
        public ISensorStreamHandler CreateHandler(SensorStreamHandlerContext context)
        {
            return new GestureStreamHandler(context);
        }
    }
}
