// 1. Syntaxt and semantics:
// - Description of computation
// - Execution of computation

// 2. Syntax / algebra
// - Primitive operations for your monad
// - Combinators do describe computations

// 3. Ideas
    // Library monad :
    // addBook :: Book -> Library BookId
    // lend :: BookId -> Librabry NbrBooks

    // Packman monad :
    // move :: Direction -> Pacman R
    // data R = OK | HitWall | Dead | AteDot

    // Database monad :
    // query, update, ...

// 4. Styles
// - Final style = type class
// - Initial style = data type with continuation

// Initial style
type Position = Position
type Player = Player1 | Player2
type Result = Result

type TicTacToe<'t> = 
| Info of (Position * (Player option -> TicTacToe<'t>))
| Take of (Position * (Result -> TicTacToe<'t>))
| Done of 't

let rec bind ttt f = 
    match ttt with
    | Done d -> f d
    | Info (p, i) -> Info (p, fun x -> bind (i x) f)
    | Take (p, t) -> Take (p, fun x -> bind (t x) f)

type TicTacToeBuilder() =
    member x.Return(value) = Done value
    member x.Bind(ttt, f) = bind ttt f

let ticTacToe = TicTacToeBuilder()

let test = ticTacToe {
    let! a = Info (Position, fun _ -> Done 5)
    return a
}

