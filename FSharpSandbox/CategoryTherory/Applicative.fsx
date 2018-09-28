// Note : Rewrite code of the article https://medium.com/@lettier/your-easy-guide-to-monads-applicatives-functors-862048d61610

module Applicative = 
    open System.Collections.Generic
    
    let substract x y = x - y
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
        let mapFunction funtion1 funtion2 = funtion1 >> funtion2
        let pure x y = x // cons function, take de first of two parameters

        pure add1 None
        // Return int -> int
        pure add1 None 0
        // Return 1

        let apply wrappedFunction func =  
            fun x -> mapFunction func (wrappedFunction x) x
            // fun x -> mapFunction (wrappedFunction x) func x
            // Note: on his article, the author write: lambda x: \map(wrapped_fun(x), fun)(x)
            // Article will give: fun x -> mapFunction (wrappedFunction x) func x     ('a -> 'a -> 'b) -> ('b -> 'c) -> 'a -> 'c  
            // Here we have:      fun x -> mapFunction func (wrappedFunction x) x     ('a -> 'b -> 'c) -> ('a -> 'b) -> 'a -> 'c   
            // They both return 3 for:  apply (pure add1) add1 1
            // For: apply (apply (pure add) add1) add1 1  (and following examples)
            // Mine: compose well and return 4
            // Article: won't compose :(

        apply (pure add1) add1 1
        // Return 3
        apply (apply (pure add) add1) add1 1
        // Return 4

        let add_x_y_z x y z = add (add x y) z
        add_x_y_z 1 2 3
        // Return 6
        apply(apply(apply(pure add_x_y_z) add1) add1) add1 1
        // Equivalent to: add_x_y_z (add1 1) (add1 1) (add1 1) = (1 + 1) + (1 + 1) + (1 + 1)
        // Return 6

        apply (pure add_x_y_z) add1 1 2 3
        // Equivalent to: add_x_y_z (add1 1) 2 3 = (add1 1) + 2 + 3 = (1 + 1) + 2 + 3
        // Return 7

        apply (apply (apply (pure add_x_y_z) add1) (substract 1)) (substract 2) 2
        // Equivalent to: add_x_y_z (add1 2) (substract 1 2) (substract 2 2) = (add1 2) + (substract 1 2) + (substract 2 2) = (1 + 2) + (1 - 2) + (2 - 2)
        // Return 2
        

        let pluck_one (x:IDictionary<string, int>) = x.["one"]
        let pluck_two (x:IDictionary<string, int>) = x.["two"]
        let pluck_three (x:IDictionary<string, int>) = x.["three"]

        let new_add_x_y_z = apply (apply (apply (pure add_x_y_z) pluck_one) pluck_two) pluck_three
        new_add_x_y_z (dict ["one", 1; "two", 2; "three", 3])
        // Return 6
        let extract_then_add_x_y_z x = add_x_y_z (pluck_one x) (pluck_two x) (pluck_three x)
        extract_then_add_x_y_z (dict ["one", 1; "two", 2; "three", 3])
        // Return 6

    // An applicative functor is a functor that you lawfully define pure and apply for.
    // 
    // let pure thing = wrap thing
    //
    // let apply wrapped_function applicative =
    //     let unwrapped_function = unwrapp wrapped_function
    //     map unwrapped_function applicative
    //
    // apply (pure function) applicative
    // return an applicative