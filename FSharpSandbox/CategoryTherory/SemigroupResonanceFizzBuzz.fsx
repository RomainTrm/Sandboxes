// See article : https://blog.ploeh.dk/2019/12/30/semigroup-resonance-fizzbuzz/

let rec cycle xs = seq { yield! xs; yield! cycle xs }

let fizzes = cycle [ None; None; Some "Fizz" ]
let buzzes = cycle [ None; None; None; None; Some "Buzz" ]

let mergeOptions = function
    | Some fizz, Some buzz -> Some (sprintf "%s%s" fizz buzz)
    | Some fizz, None -> Some fizz
    | None, Some buzz -> Some buzz
    | None, None -> None

let fizzBuzzes = Seq.zip fizzes buzzes |> Seq.map mergeOptions

let fromOption = function
    | Some x, _ -> x
    | None, x -> x
 
let numbers =  [1..100] |> Seq.map (sprintf "%d")

numbers
|> Seq.zip fizzBuzzes
|> Seq.map fromOption
|> Seq.take 100
|> Seq.toList