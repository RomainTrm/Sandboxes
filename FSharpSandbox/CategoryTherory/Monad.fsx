// Note : Rewrite code of the article https://medium.com/@lettier/your-easy-guide-to-monads-applicatives-functors-862048d61610

module Monad = 

    let add (x:float) (y:float) = x + y
    let add1 = add 1.0
    let divide (x:float) (y:float)  = x / y

    // Monad.``A monad is a list`` ()
    let ``A monad is a list`` () =
        let join x = List.collect id x
        join [[1]; [2; 3]; [4; 5; 6]]
        // Return [1; 2; 3; 4; 5; 6]

        let pure = fun x -> [x]
        let apply wrappedFunction list = 
            List.collect (fun f -> List.map f list) wrappedFunction
        let (<*>) = apply        

        join (pure (fun x -> [x + 1; x]) <*> [1..3])
        // [1 + 1; 1; 2 + 1; 2; 3 + 1; 3]  
        // Return [2; 1; 3; 2; 4; 3]      

        join (pure (fun x -> [2; x * 3; x * 4]) <*> join (pure (fun x -> [x + 1; 1]) <*> [1; 2; 3]))
        // join (apply (pure (fun x -> [2; x * 3; x * 4])) [2; 1; 3; 2; 4; 3])
        // [2; 2 * 3; 2 * 4; 2; 1 * 3; 1 * 4; 2; ...]
        // Return [2; 6; 8; 2; 3; 4; 2; 9; 12; 2; 3; 4; 2; 12; 16; 2; 3; 4]

        let y_if_x_else_z x y z = join (pure (fun a -> if a then y else z) <*> x)
        y_if_x_else_z [true; false; false; true] ["y result 1"; "y result 2"] ["z result"]
        // Return ["y result 1"; "y result 2"; "z result"; "z result"; "y result 1"; "y result 2"]

    // Monad.``A monad is a function`` () 
    let ``A monad is a function`` () =    
        let mapFunction funtion1 funtion2 x = funtion1 (funtion2 x)
        let pure x y = x // cons function, take de first of two parameters
        let apply wrappedFunction func =  
            fun x -> mapFunction (wrappedFunction x) func x
        let (<*>) = apply

        let join x y = x y y

        join (pure add <*> add1) 1.0
        // add (add1 1) 1
        // add 2 1
        // (1 + 1) + 1
        // Return 3       

        join (pure divide <*> (join (pure add <*> add1))) 2.0
        // divide (add (add1 2) 2) 2
        // ((1 + 2) + 2) / 2
        // Return 2.5

    // A monad is an applicative functor that you lawfully define join for.
    //
    // let join nested_monad = flatten nested_monad
    //
    // let function thing = monad thing
    //
    // join (apply (pure function) monad)
    // Return a monad

    // Monad.``list monad with bind`` ()
    let ``list monad with bind`` () =
        let pure = fun x -> [x]
        let apply wrappedFunction list = 
            List.collect (fun f -> List.map f list) wrappedFunction
        let join x = List.collect id x

        let bind x y = join (apply (pure x) y)

        bind (fun x -> [x + 2]) [1..3]
        // [3; 4; 5]

        bind (fun x -> [x * 3]) (bind (fun x -> pure (x + 2)) [1..3])
        // [3; 12; 15]

        bind (fun x -> [x * 3; 0]) (bind (fun x -> pure (x + 2)) [1..3])
        // [9; 0; 12; 0; 15; 0]
    