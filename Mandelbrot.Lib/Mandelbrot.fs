namespace Mandelbrot.Lib.FSharp

module Mandelbrot =

    type Result = 
        | Bounded
        | Diverged of int

    /// Whether the recursive calculation Z = (Z * Z) + C remains less than a limit
    ///  during iterations starting with Z = 0.
    let isDivergent (maxIterations: int) (limit: float) (c: Complex.Number) = 
        let rec loop i z = 
            if i > maxIterations then Bounded
            else if Complex.abs z > limit then Diverged i
            else loop (i + 1) (Complex.square z |> Complex.add c)

        loop 0 Complex.zero