using static RungeKutta.RKF45Solver;

namespace RungeKutta.UnitTests
{
    [TestClass]
    public class TestF3
    {
        [TestMethod]
        public void Test3_cosX()
        {
            var f = (double x, double y) => Math.Cos(x);
            double A = 0;
            double B = 2 * Math.PI;
            double Y0 = 1;
            double tolmin = 1.000000E-03;
            int NMAX = 40;

            var solver = new RKF45Solver();
            var points = solver.RKF45(f, A, B, Y0, tolmin, NMAX);
            var expectedPoints = points.Select(p => new PointD { X = p.X, Y = Y0 + Math.Sin(p.X) }).ToArray();

            CheckCurves(points, expectedPoints, tolmin);
        }

        private void CheckCurves(PointD[] points, PointD[] solPoints, double tol)
        {
            for(int i = 0; i < points.Length; i++)
            {
                Assert.AreEqual(points[i].X, solPoints[i].X, tol);
                Assert.AreEqual(points[i].Y, solPoints[i].Y, tol);
            }
        }
    }
}