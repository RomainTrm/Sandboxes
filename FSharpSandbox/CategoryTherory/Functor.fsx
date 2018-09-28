// Note : Rewrite code of the article https://medium.com/@lettier/your-easy-guide-to-monads-applicatives-functors-862048d61610

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

    // A functor is define by map, map definition must obey the functor laws.
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