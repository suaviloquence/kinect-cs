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

            /* while (true)
            {
                var frame = reader.AcquireLatestFrame();

        //        p.handleFrame(frame);

                System.Threading.Thread.Sleep(100);
            } */
            System.Threading.Thread.Sleep(-1);
        }

    private  void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
       // private void handleFrame(BodyFrame frame)
        {

             var frame = e.FrameReference.AcquireFrame();
            // System.Console.WriteLine("Got frame");

            if (frame == null) return;
         
            // System.Console.WriteLine(frame.BodyCount);

            if (frame.BodyCount == 0) return;

            var bodies = new Body[frame.BodyCount];

            frame.GetAndRefreshBodyData(bodies);

            foreach (var body in bodies)
            {
                if (!body.IsTracked)
                {
                    // System.Console.WriteLine("Skipping untracked body.");
                    continue;
                }

                // System.Console.WriteLine($"Is body tracked? {body.IsTracked}");
                // System.Console.WriteLine($"Body # {body.TrackingId}");
                var left = body.Joints[JointType.HandTipLeft];
                var right = body.Joints[JointType.HandTipRight];

                // System.Console.WriteLine($"- Left Hand state: {left.TrackingState}");

                var hand = (left == null) ? right : left;
                // System.Console.WriteLine("- No hands found, skipping...");
                if (hand == null) continue;

                System.Console.WriteLine($"- Clip: {frame.FloorClipPlane.X} {frame.FloorClipPlane.Y}, {frame.FloorClipPlane.Z}");

                System.Console.WriteLine($"- Left Hand Tip: ({left.Position.X}, {left.Position.Y}, {left.Position.Z})");
                // System.Console.WriteLine($" - Right Hand Tip: ({right.Position.X}, {right.Position.Y})");
            }
            frame.Dispose();
        }
    }
}
