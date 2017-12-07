using System;
using System.Drawing;

namespace TagCloudGenerator
{
    public class ArchimedianSpiral : ISpiral
    {
        private readonly double angleShift;
        private readonly double size;
        private double currentAngle;
        public Point Center { get; set; }

        public ArchimedianSpiral(double size = 1, Point center = default(Point), double angleShift = Math.PI / 180)
        {
            this.angleShift = angleShift;
            this.size = size;
            this.Center = center;
        }

        public Point GetNextPoint()
        {
            var nextPoint = GetCartesianCoordinates(size * currentAngle, currentAngle);
            currentAngle += angleShift;
            nextPoint.Offset(Center);
            return nextPoint;
        }

        private static Point GetCartesianCoordinates(double polarRadius, double polarAngle)
        {
            return new Point(
                (int) (polarRadius * Math.Cos(polarRadius)),
                (int) (polarRadius * Math.Sin(polarAngle)));
        }
    }
}