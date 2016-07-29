using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectSkeltonTracker.Gestures.GestureParts
{
    public class GestureSegment
    {
        protected float getPositionOfJoint(object body, JointType jointType, string coodinate)
        {
            var b = (Dictionary<string, object>)body;
            var joints = (Dictionary<string, object>)b["joints"];
            var joint = (Dictionary<string, object>)joints[((int)jointType).ToString()];
            var position = (Dictionary<string, object>)joint["position"];
            return (float)position[coodinate];
        }
    }
}
