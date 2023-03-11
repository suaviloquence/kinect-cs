using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace KinectCs {

internal class Program {
	List<Body> bodies = new List<Body>();

	static void Main(string[] args) {
		KinectSensor sensor = KinectSensor.GetDefault();
		sensor.Open();

		Program p = new Program();

		BodyFrameReader reader = sensor.BodyFrameSource.OpenReader();

		reader.FrameArrived += p.Reader_FrameArrived;

		System.Threading.Thread.Sleep(-1);
	}

	private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e) {
		var frame = e.FrameReference.AcquireFrame();

		if (frame == null) {
			return;
		}

		var bodies = new Body[frame.BodyCount];

		frame.GetAndRefreshBodyData(bodies);

		foreach (var body in bodies) {
			if (!body.IsTracked) {
				continue;
			}

			var left = body.Joints[JointType.HandTipLeft];
			var right = body.Joints[JointType.HandTipRight];

			System.Console.WriteLine($"- Clip: {frame.FloorClipPlane.X} {frame.FloorClipPlane.Y}, {frame.FloorClipPlane.Z}");

			System.Console.WriteLine($"- Left Hand Tip: ({left.Position.X}, {left.Position.Y}, {left.Position.Z})");
			System.Console.WriteLine($" - Right Hand Tip: ({right.Position.X}, {right.Position.Y}, {right.Position.Z})");
		}

		frame.Dispose();
	}
}

}
