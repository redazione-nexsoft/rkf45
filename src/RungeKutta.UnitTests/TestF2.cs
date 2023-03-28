using Microsoft.Research.Oslo;
using static RungeKutta.RKF45Solver;

namespace RungeKutta.UnitTests
{
    [TestClass]
    public class TestF2
    {
        [TestMethod]
        public void Test2_sinX_y()
        {
            var f = (double x, double y) => (Math.Sin(x) - y);
            double A = -3;
            double B = 5;
            double Y0 = -3.57;
            double tolmin = 1.000000E-03;
            int NMAX = 20;

            var solver = new RKF45Solver();
            var points = solver.RKF45(f, A, B, Y0, tolmin, NMAX);
            var expectedPoints = Ode.RK45(
                    0,
                    new[] { A, Y0 },
                    (t, x) => new Vector(1, Math.Sin(x[0]) - x[1]),
                    new Options
                    {
                        InitialStep = (B - A) / NMAX
                    }
                    )
                    .TakeWhile(p => p.X[0] >= A && p.X[0] <= B)
                    .ToArray();

            CheckCurves(points, expectedPoints, 0.5);
        }

        private void CheckCurves(PointD[] points, SolPoint[] solPoints, double tol)
        {
            foreach (var p in points.Take(points.Length - 1))
            {
                var sL = solPoints.LastOrDefault(s => s.X[0] <= p.X);
                var sR = solPoints.FirstOrDefault(s => s.X[0] > p.X);
                if (sL.X[0] < p.X)
                {
                    sL = solPoints.First();
                }
                if (sR.T == 0)
                {
                    sR = solPoints.Last();
                }

                var dx = p.X - sL.X[0];
                var slope = (sR.X[1] - sL.X[1]) / (sR.X[0] - sL.X[0]);
                var dy = dx * slope;
                var y = sL.X[1] + dy;
                Assert.AreEqual(p.Y, y, tol);
            }
        }
    }
}