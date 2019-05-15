// https://blog.ploeh.dk/2019/05/13/peano-catamorphism/

type BoolF = TrueF | FalseF

let cataBool falseF trueF boolF =
    match boolF with
    | FalseF -> falseF 
    | TrueF -> trueF

let andF x y = cataBool FalseF y x
let orF x y = cataBool y TrueF x
let notF = cataBool TrueF FalseF
let toBool = cataBool false true

type NatF = Zero | Succ of NatF

let zero = Zero
let one = Succ zero
let two = Succ one
let three = Succ two
let four = Succ three
let five = Succ four
let six = Succ five

let rec cataNat zero succ nat =
    match nat with
    | Zero -> zero
    | Succ n -> cataNat (succ zero) succ n

let count = cataNat 0 ((+) 1)
let add x y = cataNat x Succ y
let multiply x y = cataNat zero (add x) y

let isZero = cataNat TrueF (fun _ -> FalseF)
let isEven = cataNat TrueF notF
let isOdd = isEven >> notF