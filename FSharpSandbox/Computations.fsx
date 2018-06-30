type Maybe<'T> = 
    | Just of 'T 
    | None  

type MaybeBuilder() =
    member x.Bind(maybe, f) = 
        match maybe with
        | Just value -> f value
        | None -> None

    member x.Return(value) = Just value    

let maybe = MaybeBuilder()

let add maybeX maybeY = maybe {
        let! x = maybeX
        let! y = maybeY
        return x + y
    }

let none = add (Just 5) (None) = None

let just = add (Just 5) (Just 3) = Just 8