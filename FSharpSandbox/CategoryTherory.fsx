// Note : Rewrite code of the article https://medium.com/@lettier/your-easy-guide-to-monads-applicatives-functors-862048d61610

open System

module Functor =
    let substract2 x = x - 2
    let add = (+)
    let add1 = add 1
    let mapList funtion list = List.map funtion list 

    // Functor.``A functor is a list`` ()
    let ``A functor is a list`` () =
        mapList add1 [1..3]
        // Return [2; 3; 4]
  
    // Functor.``A functor is a function`` ()
    let ``A functor is a function`` () =
        let mapFunction funtion1 funtion2 = funtion1 >> funtion2
        let substract2_then_add1 = mapFunction substract2 add1
        substract2_then_add1 1 
        // Return 0

    // Functor.``A functor is a promise`` ()
    let ``A functor is a promise`` () =
        let mapPromise funtion promise =
            let return_promise () = 
                async {
                    let! promiseResult = promise ()
                    return funtion promiseResult
                }
            return_promise
        let promise () = async {
                do! Async.Sleep 1
                return 1 
            }      
        Async.RunSynchronously ((mapPromise add1 promise) ())
        // Return 2

    // Functor.``Functor law 1 : Identity`` ()
    let ``Functor law 1 : Identity`` () =
        let identity x = x
        let list = [1..3]
        list = mapList identity list
        // Return true

    // Functor.``Functor law 2 : Composition`` ()
    let ``Functor law 2 : Composition`` () =
        let multiply3 = (*) 3
        let compositionFunction = multiply3 >> substract2 >> add1
        // composition 1 = 2
        let compositionList = mapList multiply3 >> mapList substract2 >> mapList add1
        mapList compositionFunction [1..3] = compositionList [1..3]
        // Return true


module Applicative = 
    let substract2 x = x - 2
    let add = (+)
    let add1 = add 1
    let mapList funtion list = List.map funtion list 

    // Applicative.``An applicative functor is a list`` ()
    let ``An applicative functor is a list`` () =
        let pure = fun x -> [x]
        let apply wrappedFunction list = 
            List.collect (fun f -> mapList f list) wrappedFunction
        apply (pure add1) [1..3]
        // Return [2; 3; 4]       

        apply (apply (pure add) []) [4..6]
        // Return []
        
        apply (apply (pure add) [1..3]) [4..6]
        apply (mapList add [1..3]) [4..6]
        // Return [5; 6; 7; 6; 7; 8; 7; 8; 9]

        let b_if_a_else_c a b c = if a then b else c
        let y_if_x_else_z x y z = apply (apply (apply (pure b_if_a_else_c) x) y) z

        y_if_x_else_z [true] ["y result"] ["z result"]
        // Return ["y result"]
        y_if_x_else_z [true] ["y result"] []
        // Return []

        let y_if_x_else_z_lifted x y z = apply (pure (fun a -> if a then y else z)) x
        y_if_x_else_z_lifted [true] ["y result"] []
        // Return [["y result"]]

    // Applicative.``An applicative functor is a function`` ()
    let ``An applicative functor is a function`` () = 
        let mapFunction funtion1 funtion2 = funtion1 >> funtion2 // TODO : wrong definition (cf: apply)
        let pure x y = x // cons function, take de first of two parameters

        pure add1 None
        // Return int -> int
        pure add1 None 0
        // Return 1

        let apply wrappedFunction func = 
            fun x -> (wrappedFunction x) (func x)

        apply (pure add1) add1 1
        // Return 3
        apply (apply (pure add) add1) add1 1
        // Return 4

        let add_x_y_z x y z = add (add x y) z
        add_x_y_z 1 2 3
        // Return 6
        apply(apply(apply(pure add_x_y_z) add1) add1) add1 1
        // Return 6