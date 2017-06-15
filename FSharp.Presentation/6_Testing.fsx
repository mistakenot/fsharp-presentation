(* Testing support 
    - Interops with main .NET testing frameworks: NUnit, XUnit
    - Has support for property based testing
*)

// Use with XUnit
#r "../packages/NUnit.3.6.1/lib/net45/nunit.framework.dll"
#r "../packages/FsUnit.3.0.0/lib/net45/FsUnit.NUnit.dll"
open FsUnit
open NUnit.Framework

[<Test>]
let ``Reversing a list twice maintains the original order`` () = 
    let expected = [1;2;3]
    let actual = expected |> List.rev |> List.rev
    actual |> should equal expected
  




// FsCheck is inspired by Haskells QuickCheck library.
// Test cases are auto-generated to provide better coverage
#r "../packages/FsCheck.2.9.0/lib/net452/FsCheck.dll"
open FsCheck

// Define a test property for any instance of list<int>
let reverseOfReverseIsOriginal (list: list<int>) = list |> List.rev |> List.rev |> (=) list

// Framework auto-generates test cases and checks them against the property
Check.Quick reverseOfReverseIsOriginal

let headOfListReturnsFirstValue (list: list<int>) = list.Head = list.Item 0
Check.Quick headOfListReturnsFirstValue



// Monoid example
type Monoid<'a> = 
    abstract member Zero: 'a
    abstract member Combine: 'a -> 'a -> 'a



// There exists an element e in S such that for every element 
//  a in S, the equations e • a = a • e = a hold.
let combineZero (m: Monoid<int>) (x: int) = 
    (m.Combine x m.Zero) = x && (m.Combine m.Zero x) = x

// For all a, b and c in S, the equation (a • b) • c = a • (b • c) holds.
let isAssociative (m: Monoid<int>) (x: int) (y: int) (z: int) = 
    ((m.Combine x y) |> m.Combine z) = (m.Combine x <| (m.Combine y z))


// Two implementations
type AdditionMonoid () = 
    interface Monoid<int> with
        member this.Zero = 0
        member this.Combine x y = x + y

type SubtractionMonoid () = 
    interface Monoid<int> with
        member this.Zero = 0
        member this.Combine x y = x - y


let additionMonoid = new AdditionMonoid()

combineZero additionMonoid |> Check.Quick
isAssociative additionMonoid |> Check.Quick

let subtractionMonoid = new SubtractionMonoid()

combineZero subtractionMonoid |> Check.Quick
isAssociative subtractionMonoid |> Check.Quick