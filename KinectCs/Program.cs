using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectCs
{
    internal class Program
    {
        List<Body> bodies = new List<Body>();

        static void Main(string[] args)
        {
            KinectSensor sensor = KinectSensor.GetDefault();
            sensor.Open();

            Program p = new Program();

            BodyFrameReader reader = sensor.BodyFrameSource.OpenReader();

            reader.FrameArrived += p.Reader_FrameArrived;

            System.Threading.Thread.Sleep(-1);
        }

        private  void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {

            var frame = e.FrameReference.AcquireFrame();
            System.Console.WriteLine("Got frame");

            if (frame == null) return;
         
            System.Console.WriteLine(frame.BodyCount);

            if (frame.BodyCount == 0) return;

            var bodies = new Body[frame.BodyCount];

            frame.GetAndRefreshBodyData(bodies);

            var body = bodies[0];

            var left = body.Joints[JointType.HandTipLeft];
            var right = body.Joints[JointType.HandTipRight];

            var hand = (left == null) ? right : left;
            if (hand == null) return;

            System.Console.WriteLine($"{left.Position.X} {left.Position.Y}");
            System.Console.WriteLine($"{right.Position.X} {right.Position.Y}");
        }
    }
}
