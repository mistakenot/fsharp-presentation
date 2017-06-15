(* 4. Pattern Matching
    - Pattern matching is a way of executing code branches when
        the input matches a pattern.
    - Similar to switch statement, but much more flexible.
*)

// Using it with lists
let isListEmpty list = 
    match list with
    | [] -> true
    | head::tail -> false

let rec quicksort list = 
    match list with
    | [] -> []
    | head::tail ->
        let (smaller, greater) = List.partition ((>) head) tail
        quicksort smaller @ [head] @ quicksort greater











// Using it with unions
type BinaryTree<'a> = 
    | Empty
    | Node of ('a * BinaryTree<'a> * BinaryTree<'a>) 

let rec insertValue x tree = 
    match tree with
    | Empty -> Node(x, Empty, Empty)
    | Node(elem, left, right) -> 
        if x = elem then tree 
        else if x < elem then Node(elem, insertValue x left, right)
        else Node(elem, left, insertValue x right)











// Creating a Json serializer
type Json = 
    | Null
    | Bool of bool
    | Number of float
    | String of string
    | Array of list<Json>
    | Object of Map<string, Json>


let rec serialize json = 
    match json with
    | Null -> "null"
    | Bool b -> sprintf "%b" b
    | Number n -> sprintf "%f" n
    | String v -> sprintf "\"%s\"" v
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







// Convience method
let Object = Map.ofList >> Object

let jsonValue: Json = 
    Object [
        ("Age", Number 27.0);
        ("Name", String "Alice");
        ("PreviousTitles", Array [
            String "Junior"; 
            String "Mid"]);
        ("DoB", Object [
            ("Year", String "1987");
            ("Month", String "Feb");
            ("Day", String "12")]
        )]

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