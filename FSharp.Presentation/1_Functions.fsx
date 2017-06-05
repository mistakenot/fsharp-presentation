(* 1.1 What is F#?
    - Created by Microsoft Research.
    - Open source, cross platform.
    - Multi paradigm.
    - Strongly, statically typed with inference.
*)

let creator = "Don Syme @ Microsoft"

let platforms = [".NET Framework"; ".NET Core"; "Mono"; "JavaScript"]

let inspiredBy = ["OCaml"; "Scala"; "ML"]








(* 1.2 Declaring functions and variables
    - 'let' keyword is immutable declaration of a function or variable.
    - Functions can be partially applied.
*)
// Type of (float)
let pi: float = 3.14

// Inferred type of (float)
let sqrRootOfTwo = 1.41

// Type of (float -> float -> float)
let multiply x y = x * y

// Multi-line function
let kilometersToMiles k = 
    let x = multiply k 6.0
    x / 8.0                 //  Last line is return value

// Type of (float -> float)
let square i = multiply i i

// Partial application
// Type of (float -> float)
let multiplyByPi = multiply pi

// Recursive functions can call themselves
let rec factorial x = 
    if x < 2
    then 1
    else x * factorial x-1 

// Anonomous functions are created with the fun keyword
// Type of (int -> bool)
let isValueEven = fun i -> (i%2) = 0





(* 1.3 Using functions together *)

// Using brackets to specify order of precedence
let areaOfCircle r = multiplyByPi (square r)

// Or, using the pipe operator 
let areaOfCircleUsingPipe r = 
    square r 
    |> multiplyByPi

// Or, using composition
// Type of (float -> float)
let areaOfCircleUsingArrows = square >> multiplyByPi

// sprintf is a compiler-checked string formatting function
let toString a = sprintf "%f metres squared" a

// printfn is like sprintf, but writes to the console.
let printAreaofCircle r = 
    square
    >> multiplyByPi
    >> toString
    >> printfn "The area of this circle is %s."
    