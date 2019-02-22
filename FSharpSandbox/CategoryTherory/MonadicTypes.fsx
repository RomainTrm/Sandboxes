// Inspired by : https://sadraskol.com/posts/simple-take-on-monadic-types-the-list

type Tree = unit
type Apple = unit
type Pie = unit

type Bake = Apple -> Pie
let bake : Bake = fun apple -> ()

module ListAsMonadicType =
    
    type Harvest = Tree -> Apple list
    let harvest : Harvest = fun tree -> [(); (); ()]

    let rec map (f: 'a -> 'b) (list: 'a list) : 'b list = 
        // List.map
        match list with
        | [] -> []
        | head::tail -> (f head)::(map f tail)

    let bakeAll (apples: Apple list) : Pie list = 
        map bake apples

    let harvestAndBake (tree: Tree) : Pie list = 
        map bake (harvest tree)

    let harvestAndBakeAll (trees: Tree list) : Pie list list = 
        map harvestAndBake trees
        
    let rec join (items: 'a list list) : 'a list =
        // List.join
        match items with
        | [] -> []
        | head::tail -> head@(join tail)

    let harvestAndBakeAll_Join (trees: Tree list) : Pie list = 
        join (map harvestAndBake trees)

    let bind (list: 'a list) (f: 'a -> 'b list) : 'b list =
        join (map f list)

    let harvestAndBakeAll_Bind (trees: Tree list) : Pie list = 
        bind trees harvestAndBake

    let (<.>) = map
    let (>>=) = bind

    let harvestAndBakeAll_Operators (trees: Tree list) : Pie list = 
        trees >>= (fun tree -> bake <.> (harvest tree))


module OptionAsMonadicType = 

    type Harvest = Tree -> Apple option
    let harvest : Harvest = fun tree -> Some ()

    [<Struct>]
    type OptionalBuilder =
      member __.Bind(opt, binder) =
        match opt with
        | Some value -> binder value
        | None -> None
      member __.Return(value) =
        Some value
        
    let optional = OptionalBuilder()

    let map (f: 'a -> 'b) (option: 'a option) : 'b option =
        match option with
        | None -> None
        | Some a -> Some (f a)

    let bakeAll (apples: Apple option) : Pie option = 
        map bake apples

    let harvestAndBake (tree: Tree) : Pie option = 
        map bake (harvest tree)

    let harvestAndBakeAll (trees: Tree option) : Pie option option = 
        map harvestAndBake trees
        
    let rec join (item: 'a option option) : 'a option =
        // match item with
        // | None -> None
        // | Some a -> match a with
        //             | None -> None
        //             | Some b -> Some b
        optional {
            let! level1 = item 
            let! level2 = level1
            return level2
        }            
            
    let harvestAndBakeAll_Join (trees: Tree option) : Pie option = 
        join (map harvestAndBake trees)

    let bind (option: 'a option) (f: 'a -> 'b option) : 'b option =
        join (map f option)

    let harvestAndBakeAll_Bind (trees: Tree option) : Pie option = 
        bind trees harvestAndBake

    [<Struct>]
    type OptionalBindBuilder =
      member __.Bind(opt, binder) = bind opt binder
      member __.Return(value) = Some value
        
    let optional2 = OptionalBindBuilder()

    let harvestAndBakeAll_Computation (trees: Tree option) : Pie option = 
        optional2 {
            let! tree = trees
            let! apple = harvest tree
            let pie = bake apple
            return pie
        }