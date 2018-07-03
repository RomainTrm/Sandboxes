let foo n =
    printfn "foo %d" n
    n <= 10

let n = lazy foo 5
let run = 
    n.Value // print "foo 5"
    n.Value // print nothing