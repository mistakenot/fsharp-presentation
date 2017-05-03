namespace Mandelbrot.Lib.FSharp

open System

module Complex = 
    /// Number that can be expressed in form (A + Bi) where A is the 
    ///  'Real' part and B is the 'Imaginary' part and i represents 
    ///  the square root of -1.
    /// See https://en.wikipedia.org/wiki/Complex_number for more.
    type Number = {Re: float; Im: float}

    /// Zero value
    let zero = {Re = 0.0; Im = 0.0}

    /// Adds together two complex numbers.
    let add a b = {Re = a.Re + b.Re; Im = a.Im + b.Im}

    /// Multiples together two complex numbers.
    let multiply a b = 
        let realPart = (a.Re * b.Re) - (a.Im * b.Im)
        let imagPart = (a.Im * b.Re) + (a.Re * b.Im)
        {Re = realPart; Im = imagPart}

    /// Squares a Complex Number.
    let square a = multiply a a

    /// Absolute value the Complex Number.
    let abs a = Math.Sqrt((a.Re * a.Re) + (a.Im * a.Im))