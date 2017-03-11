namespace Mandelbrot.Lib.CSharp
{
    public class Mandelbrot
    {
        private readonly int _maxIterations;
        private readonly float _limit;

        public Mandelbrot(int maxIterations, float limit)
        {
            _maxIterations = maxIterations;
            _limit = limit;
        }

        /// <summary>
        /// Whether the recursive calculation Z = (Z * Z) + C remains within bounds 
        ///  during iterations starting with Z = 0.
        /// </summary>
        /// <param name="c">ComplexNumber value of C.</param>
        /// <returns>Result type instance.</returns>
        public Result IsDivergent(ComplexNumber c)
        {
            // Iteration
            var i = 0;
            // Accumulator
            var z = ComplexNumber.Zero;

            while (i < _maxIterations)
            {
                if (ComplexNumber.Abs(z) > _limit)
                {
                    return new Result(true, i);
                }

                z = ComplexNumber.Square(z) + c;
            }

            return new Result(false, -1);
        }
    }

    public class Result
    {
        public bool IsDivergent { get; }
        public int DivergedAt { get; }

        public Result(bool isDivergent, int divergedAt)
        {
            IsDivergent = isDivergent;
            DivergedAt = divergedAt;
        }
    }
}
