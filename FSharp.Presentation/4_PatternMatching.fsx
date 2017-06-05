(* 4. Pattern Matching
    - Pattern matching is a way of executing code branches when
        the input matches a pattern.
    - Similar to switch statement, but much more flexible.
*)

// Using it with lists
let isListEmpty (list: list<_>) = 
    match list with
    | [] -> true
    | _ -> false

let rec quicksort list = 
    match list with
    | [] -> []
    | head::tail ->
        let (smaller, greater) = List.partition (fun i -> i < head) tail
        quicksort smaller @ [head] @ quicksort greater

// Using it with unions
type BinaryTree<'a> = 
    | Empty
    | Node of ('a * BinaryTree<'a> * BinaryTree<'a>) 

let rec insertValue tree x = 
    match tree with
    | Empty -> Node (x, Empty, Empty)
    | Node (elem, l, r) -> 
        if x = elem then tree 
        else if x < elem then insertValue l x 
        else insertValue r x



// Creating a Json serializer
type Json = 
    | Null
    | Bool of bool
    | Number of float
    | Value of string
    | Array of list<Json>
    | Object of Map<string, Json>


let rec serialize json = 
    match json with
    | Null -> "null"
    | Bool b -> sprintf "%b" b
    | Number n -> sprintf "%f" n
    | Value v -> sprintf "\"%s\"" v
    | Array values -> 
        values 
        |> List.map serialize 
        |> String.concat "," 
        |> sprintf "[%s]"
    | Object properties -> 
        properties 
        |> Seq.map (fun kv -> sprintf "\"%s\": %s" kv.Key (serialize kv.Value))
        |> String.concat ","
        |> sprintf "{%s}"

let jsonValue: Json = 
    [
    ("Age", Number 27.0);
    ("Name", Value "Alice");
    ("PreviousTitles", Array [Value "Junior"; Value "Mid"]);
    ("DoB", 
        [
        ("Year", Value "1987");
        ("Month", Value "Feb");
        ("Day", Value "12")
        ]
        |> Map.ofList
        |> Object
    )] 
    |> Map.ofList 
    |> Object

serialize jsonValue |> printf "%s"
(*
    {
      "DoB": {
        "Day": "12",
        "Month": "Feb",
        "Year": "1987"
      },
      "Id": "123",
      "Age": "27.000",
      "Name": "Alice",
      "PreviousTitles": [
        "Junior",
        "Mid"
      ]
    }
*)