//cf : https://blog.ploeh.dk/2019/09/16/picture-archivist-in-f/

// type Folder = Folder of string
// type Picture = Picture of int32
// type Tree = Node of Folder * Tree list | Leaf of Picture

type Tree<'a, 'b> = Node of 'a * Tree<'a, 'b> list | Leaf of 'b

let node x xs = Node (x, xs)
let leaf x = Leaf x

let rec cata fd ff = function 
    | Leaf x -> ff x
    | Node (x, xs) -> xs |> List.map (cata fd ff) |> fd x

let choose f = cata (fun x -> List.choose id >> node x >> Some) (f >> Option.map Leaf)
let bimap f g = cata (f >> node) (g >> leaf)
let map f = bimap id f

let bifold f g z t =
    let flip f x y = f y x
    cata (fun x xs -> flip f x >> List.fold (>>) id xs) (flip g) t z

let bifoldBack f g t z = 
    cata (fun x xs -> List.foldBack (<<) xs id >> f x) g t z

let fold f = bifold (fun x _ -> x) f
let foldBack f = bifoldBack (fun _ x -> x) f

let iter f = fold (fun () x -> f x) ()

let myTree = Node ("Dir 1", 
                [
                    Leaf "File 1.A"; 
                    Leaf "File 1.B"; 
                    Leaf "File 1.C"; 
                    Node ("Dir 1.1", [Leaf "File 1.1.A"]);
                    Node ("Dir 1.2", []);
                    Node ("Dir 1.3", [Leaf "File 1.3.A"; Leaf "File 1.3.B"])  
                ])

// Exemples 
let countLeafs = fold (fun nbLeaf _ -> nbLeaf + 1) 0 myTree

let reduceOnSingleNode = fold (fun leafs leaf -> leafs@[leaf]) [] myTree

