type Functor<'a> = Functor of 'a
    with
    static member pure (Functor f) a = f a
    static member apply (Functor f) (Functor a) = Functor (f a)

let functorAdd1 = Functor ((+) 1)
Functor.pure functorAdd1 2 // Return 2

let functor2 = Functor 2
Functor.apply functorAdd1 functor2 // Return Functor 3