let rec factorial n =
    if n <= 1
    then 1
    else n * factorial (n - 1)

let rec multiplyByHead list =
    match list with
    | [] -> 1
    | head::tail -> head * (multiplyByHead tail)