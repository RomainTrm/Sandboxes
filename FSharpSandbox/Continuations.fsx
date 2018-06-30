type Tree =
| Leaf of int
| Node of Tree * Tree

let numbers = [1..100000]
let unbalancedTree = numbers |> List.fold (fun state value ->  Node (Leaf value, state)) (Leaf 0)

let rec naiveSum tree =
    match tree with
    | Leaf n -> n
    | Node (l, r) -> naiveSum l + naiveSum r

let fail = naiveSum unbalancedTree // return stack overflow

let rec sum tree cont =
    match tree with
    | Leaf n -> cont n
    | Node (l, r) -> sum l (fun sumL -> sum r (fun sumR -> cont (sumL + sumR)))

let success = sum unbalancedTree id