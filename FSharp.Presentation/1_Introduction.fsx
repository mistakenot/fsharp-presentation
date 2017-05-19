(* 1.1 What is F#?
    - Created by Microsoft Research ~12 years ago.
    - Open source, cross platform.
    - Syntax inspired by ML family of languages.
    - Historically used in quantitive environments; finance, research etc.
*)

let creator = "Don Syme @ Microsoft"

let platforms = [".NET Framework"; ".NET Core"; "Mono"; "JavaScript"]

let inspiredBy = ["OCaml"; "Scala"; "ML"]








(* 1.2 Main Features?
    - Multi-paradym, but encourages a 'functional first' approach.
    - Strong, infered typing system.
    - Good IDE support:
        Visual Studio (Windows)
        Xamarin Studio (Windows / *nix)
        Iodine project (VS Code, Atom)
    - Seamless interop with any .NET binary (C#, Nuget, etc.)
*)

let x = 1 + "two" // Inferred, strong types
let y = 2

let mutable y = 0 // Imperative programming support
y <- 1

type MyObject(name: string) = // Object-orientated support
   member this.SayHello() = printf "Hi, my name is %s" name






(* 1.3. What is functional programming?
    - Programs are built by composing together 'pure' functions into larger components.
    - Pure Functions: a function that returns a value without reassignment to existing variables.
    - Code looks more like mathmatical expressions instead of step-by-step commands.
    - Business logic is often encoded into the type system.
    - Concurrency is much simpler when you copy data instead of updating it.
*)
// Imperative, C style implementation
let imperativeFactorial x = 
    let mutable y = 1
    if x > 0 then 
        for i in [1..x] do
            y <- y * i
    y

// Functional implementation
let rec functionalFactorial x =
    if x < 1 then 1
    else x * functionalFactorial (x - 1)






(* 1.4. Why bother?
    - Pros:
        Conciseness (Code better reflects the programmers intentions)
        Correctness (Better use of the type system)
        Productivity (Eliminates whole classes of bugs)
        Testability (Pure functions are easier to test)
    - Cons:
        Learning curve
        Performance
*)

// Quicksort implementation in csharp
(*
public static List<T> QuickSort<T>(List<T> values) 
      where T : IComparable
   {
      if (values.Count == 0)
      {
         return new List<T>();
      }

      var firstElement = values[0];
      var smallerElements = new List<T>();
      var largerElements = new List<T>();

      for (int i = 1; i < values.Count; i++) 
      {
         var elem = values[i];
         if (elem.CompareTo(firstElement) < 0)
         {
            smallerElements.Add(elem);
         }
         else
         {
            largerElements.Add(elem);
         }
      }

      var result = new List<T>();

      result.AddRange(QuickSort(smallerElements.ToList()));
      result.Add(firstElement);
      result.AddRange(QuickSort(largerElements.ToList()));

      return result;
   }
*)

// Quicksort implementation in fsharp
let rec qsort list = 
    match list with
    | [] -> []
    | x::xs -> let smaller = [for a in xs do if a <= x then yield a]
               let larger =  [for b in xs do if b > x then yield b]
               qsort smaller @ [x] @ qsort larger