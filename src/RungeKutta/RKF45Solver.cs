namespace RungeKutta
{
    public class RKF45Solver
    {

        /// <summary>
        /// CALCOLA LE SOLUZIONI DISCRETE DI UNA ODE DEFINITA IN[A, B]
        /// A VALORE INIZIALE Y0 SU N INTERVALLI
        /// </summary>
        public PointD[] RKF45(Func<double, double, double> f, double a, double b, double y0, double tolMin, int numMaxIterazioni)
        {
            if (a > b)
            {
                throw new ArgumentException("Gli estermi dell'intervallo A e B sono invertiti");
            }

            if (tolMin <= EPSILON(1f))
            {
                throw new ArgumentException("La tolleranza non può essere inferiore all'epsilon macchina");
            }

            if (numMaxIterazioni < 1)
            {
                throw new ArgumentException("Il numero di intervallo di integrazione non può essere inferiore ad 1");
            }

            //INIZIALIZZAZIONE
            double H = (b - a) / (numMaxIterazioni - 1);
            double HMAX = 64 * H;
            double HMIN = H / 64;

            List<PointD> ret = new List<PointD>(numMaxIterazioni);
            PointD lastPoint = new PointD { X = a, Y = y0 };
            ret.Add(lastPoint);

            // INIZIO CICLO DI CALCOLO
            while (lastPoint.X < b)
            {
                //10       CONTINUE
                double R, D;
                double[] K = null;
                R = tolMin;
                while (R >= tolMin)
                {
                    K = COMPUTEK(f, lastPoint, H);

                    //CALCOLO DELL'ERRORE LOCALE DI TRONCAMENTO            
                    R = Math.Abs(YK1(lastPoint.Y, K) - ZK1(lastPoint.Y, K));

                    //LA DISTANZA H DEVE ESSERE MODIFICATA SE R E' MAGGIORE DELLA TOLLERANZA
                    if (R >= tolMin)
                    {
                        //CALCOLO DEL COEFFICIENTE PER LA MODIFICA DI H
                        D = .84 * Math.Pow((tolMin * H / R), 0.25);
                        // MANTIENI D NELL'INTERVALLO [.1, 4]
                        if (D < 0.1)
                        {
                            D = 0.1;
                        }
                        else if (D > 4)
                        {
                            D = 4;
                        }
                        //MODIFICA DI H
                        H = D * H;
                        //CONTROLLA CHE H APPARTENGA A[HMIN, HMAX]
                        if (H > HMAX)
                        {
                            H = HMAX;
                        }
                        else if (H < HMIN)
                        {
                            throw new DivideByZeroException();
                        }

                        //CONTROLLA CHE X + H APPARTENGA AD [A, B]
                        if (lastPoint.X + H > b)
                        {
                            H = b - lastPoint.X;
                        }
                    }
                }

                // MEMORIZZO I PUNTI
                if (ret.Count < numMaxIterazioni - 1)
                {
                    var newPoint = new PointD
                    {
                        X = lastPoint.X + H,
                        Y = YK1(lastPoint.Y, K)  
                    };
                    ret.Add(newPoint);
                    lastPoint = newPoint;
                } 
                else 
                {
                    var newPoint = new PointD
                    {
                        X = b,
                        Y = YK1(lastPoint.Y, K)  
                    };
                    ret.Add(newPoint);
                    lastPoint = newPoint;
                    if (Math.Abs(b - lastPoint.X) >= tolMin)
                    {
                        throw new InsufficientMemoryException("Numero di iterazioni insufficiente");
                    }
                }
                 
                //FINE DEL CICLO DI CALCOLO
            }

            return ret.ToArray();
        }

        public class PointD
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        private double EPSILON(double x)
        {
            return Double.Epsilon;
        }

        private double[] COMPUTEK(Func<double, double, double> F, PointD lastPoint, double H)  
        {
            double X = lastPoint.X;
            double Y = lastPoint.Y;
            double K1 = H * F(X, Y);
            double K2 = H * F(X + H / 4, Y + K1 / 4);
            double K3 = H * F(X + 3 * H / 8, Y + 3 * K1 / 32 + 9 * K2 / 32);
            double K4 = H * F(X + 12 * H / 13, Y + (1932 * K1 - 7200 * K2 + 7296 * K3) / 2197);
            double K5 = H * F(X + H, Y + 439 * K1 / 216 - 8 * K2 + 3680 * K3 / 513 - 845 * K4 / 4104);
            double K6 = H * F(X + H / 2, Y - 8 * K1 / 27 + 2 * K2 - 3544 * K3 / 2565 + 1859 * K4 / 4104 - 11 * K5 / 40);
            return new[] { K1, K2, K3, K4, K5, K6 }; 
        }

        // QUESTA FUNCTION CALCOLA LE ORDINATE DEI PUNTI APPROSSIMANTI C LA CURVA SOLUZIONE DELL' ODE ,ADOPERANDO IL METODO RK4
        private double YK1(double Y, double[] K)  
        {
            return Y + (25 * K[0] / 216 + 1408 * K[2] / 2565 + 2197 * K[3] / 4104 - K[4] / 5);
        }

        // QUESTA FUNCTION CALCOLA LE ORDINATE DEI PUNTI APPROSSIMANTI C LA CURVA SOLUZIONE DELLA ODE, ADOPERANDO IL METODO RK5
        private double ZK1(double Y, double[] K)  
        {
            return Y + (16 * K[0] / 135 + 6656 * K[2] / 12825 + 28561 * K[3] / 56430 - 9 * K[4] / 50 + 2 * K[5] / 55);
        }
    }
}