(* 3. Collections
    - F# can use any .NET collection type
    - Has its own implementations of lists, maps, arrays and sets.
*)

// Linked list

let emptyList = []
let twoAndThree = [2; 3;]
// Prepending
let oneToThree = 1 :: twoAndThree
// Range of values
let fourToSeven = [4..7]
// Concatinating two lists
let oneToSeven = oneToThree @ fourToSeven
// Generators
let evensBetweenZeroAndHundred = [for i in [0..100] do if i%2 = 0 then yield i]



// Maps can be constructed from lists of (key, value) tuples.
let map = [(1, "one"); (2, "two"); (3, "three")] |> Map.ofList


// Arrays can be constructed in many ways
// Building an array of primes less than 100:
let emptyArray = [||]
let array = [|4..7|]
let isPrime n = [|2..(n-1)|] |> Seq.forall (fun i -> n % i <> 0)
let primesLessThenOneHundred = 
    [| for i in 0..100 do
        if isPrime i then
            yield i|]

primesLessThenOneHundred.[0] <- 123  // Can be mutated