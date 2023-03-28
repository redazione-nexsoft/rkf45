using Microsoft.Research.Oslo;
using static RungeKutta.RKF45Solver;

namespace RungeKutta.UnitTests
{
    [TestClass]
    public class TestF1
    {
        [TestMethod]
        public void Test1_Tan2X()
        {
            var f = (double x, double y) => (1 + Math.Pow(Math.Tan(x), 2));
            double A = -1.4;
            double B = 1.4;
            double Y0 = -5.797884;
            double tolmin = 1.000000E-04;
            int NMAX = 40;

            var solver = new RKF45Solver();
            var points = solver.RKF45(f, A, B, Y0, tolmin, NMAX);

            var expectedPoints = Ode.RK45(
                    0,
                    new[] { A, Y0 }, 
                    (t, x) => new Vector(1, 1 + Math.Pow(Math.Tan(x[0]), 2) ),
                    new  Options
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
            foreach(var p in points.Take(points.Length - 1))
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