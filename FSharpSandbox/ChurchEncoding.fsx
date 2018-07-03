type Peano = Zero | Succ of Peano

let zero = Zero
let one = Succ zero
let two = Succ one
let three = Succ two
let four = Succ three
let five = Succ four


let rec isEven n =
    match n with
    | Zero -> true
    | Succ Zero -> false
    | Succ (Succ p) -> isEven p

let rec toInt n = 
    match n with 
    | Zero -> 0
    | Succ p -> 1 + (toInt p)

let rec add x y = 
    match x with
    | Zero -> y
    | Succ p -> Succ (add p y)