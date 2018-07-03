let rec simpleSum list =
    match list with
    | [] -> 0
    | head::tail -> head + simpleSum tail

let sum list =
    let rec sumUtil total list =
        match list with
        | [] -> total
        | head::tail -> sumUtil (total + head) tail
    sumUtil 0 list

let prepend el list = el::list

let rec append el list =
    match list with
    | [] -> [el]
    | head::tail -> head::(append el tail)

let numbers = [1..100000]
let fail = simpleSum numbers // return stack overflow
let success = sum numbers // return a result