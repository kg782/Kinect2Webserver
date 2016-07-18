// -----------------------------------------------------------------------
// <copyright file="BodyStreamMessage.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.Webserver.Sensor.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Kinect;

    /// <summary>
    /// Serializable representation of an body stream message to send to client.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
        Justification = "Lower case names allowed for JSON serialization.")]
    public class BodyStreamMessage : StreamMessage
    {
        /// <summary>
        /// Serializable body array.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "bodies", Justification = "Lower case names allowed for JSON serialization.")]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Array properties allowed for JSON serialization.")]
        public object[] bodies { get; set; }

        /// <summary>
        /// Update hand pointers from specified user info data.
        /// </summary>
        /// <param name="bodies">
        /// Enumeration of UserInfo structures.
        /// </param>
        public void UpdateBodies(Body[] bodies)
        {
            if (bodies == null)
            {
                throw new ArgumentNullException("bodies");
            }

            if (this.bodies == null || this.bodies.Length != bodies.Length)
            {
                this.bodies = new object[bodies.Length];
            }

            for (int i = 0; i < this.bodies.Length; i ++)
            {
                this.bodies[i] = JsonSerializationExtensions.ExtractSerializableJsonData(bodies[i]);
            }
        }
    }
}