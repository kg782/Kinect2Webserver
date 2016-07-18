// -----------------------------------------------------------------------
// <copyright file="UserActivityMeter.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.Webserver.Sensor
{
    using System.Collections.Generic;
    using Microsoft.Kinect;

    /// <summary>
    /// Helper class used to measure user activity.
    /// </summary>
    internal class UserActivityMeter
    {
        private readonly Dictionary<ulong, UserActivityRecord> activityRecords = new Dictionary<ulong, UserActivityRecord>();
        private int totalUpdatesSoFar;

        /// <summary>
        /// Clears all user activity metrics.
        /// </summary>
        public void Clear()
        {
            this.activityRecords.Clear();
        }

        /// <summary>
        /// Update user activity metrics with data from a collection of bodies.
        /// </summary>
        /// <param name="bodies">
        /// Collection of bodies to use to update activity metrics.
        /// </param>
        /// <param name="timestamp">
        /// Time when body array was received for processing.
        /// </param>
        /// <remarks>
        /// UserActivityMeter assumes that this method is called regularly, e.g.: once
        /// per body frame received by application, so if a user whose activity was
        /// previously measured is now absent, activity record will be removed.
        /// </remarks>
        public void Update(ICollection<Body> bodies, long timestamp)
        {
            foreach (var body in bodies)
            {
                UserActivityRecord record;

                if (this.activityRecords.TryGetValue(body.TrackingId, out record))
                {
                    record.Update(body.Joints[JointType.SpineMid].Position, this.totalUpdatesSoFar, timestamp);
                }
                else
                {
                    record = new UserActivityRecord(body.Joints[JointType.SpineMid].Position, this.totalUpdatesSoFar, timestamp);
                    this.activityRecords[body.TrackingId] = record;
                }
            }

            // Remove activity records corresponding to users that are no longer being tracked
            var idsToRemove = new List<ulong>();
            foreach (var record in this.activityRecords)
            {
                if (record.Value.LastUpdateId != this.totalUpdatesSoFar)
                {
                    idsToRemove.Add(record.Key);
                }
            }

            foreach (var id in idsToRemove)
            {
                this.activityRecords.Remove(id);
            }

            ++this.totalUpdatesSoFar;
        }

        /// <summary>
        /// Gets the activity record associated with the specified user.
        /// </summary>
        /// <param name="userTrackingId">
        /// Body tracking Id of user associated with the activity record to
        /// retrieve.
        /// </param>
        /// <param name="record">
        /// [out] When this method returns, contains the record associated with the
        /// specified user tracking Id, if the appropriate activity record is found.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <code>true</code> if the UserActivityMeter contains an activity record
        /// for the specified user tracking Id; otherwise, <code>false</code>.
        /// </returns>
        public bool TryGetActivityRecord(ulong userTrackingId, out UserActivityRecord record)
        {
            return this.activityRecords.TryGetValue(userTrackingId, out record);
        }
    }
}
