// https://github.com/thinkbeforecoding/applicatives/blob/master/applicatives-live.fsx

open System

type User = {
    FirstName: string
    LastName: string
    Age: int
}


let getAnUser firstName lastName age =
    {
        FirstName = firstName
        LastName = lastName
        Age = age
    }

module Options =
    let notEmpty input =
        match String.IsNullOrEmpty input with
        | true -> None
        | false -> Some input

    let getInt input =
        match Int32.TryParse input with
        | true, i -> Some i
        | false, _ -> None

    let map f ox =
        match ox with
        | Some x -> Some (f x)
        | None -> None

    let (<!>) = map

    let map2 f ox oy =
        match ox, oy with
        | Some x, Some y -> Some (f x y)
        | _ -> None

    let apply of' ox =
        map2 (fun f x -> f x) of' ox

    let (<*>) = apply

    getAnUser
        <!> notEmpty "John"
        <*> notEmpty "Doe"
        <*> getInt "42"

module Results =
    let notEmpty input =
        match String.IsNullOrEmpty input with
        | true -> Error "This is an empty string"
        | false -> Ok input

    let getInt input =
        match Int32.TryParse input with
        | true, i -> Ok i
        | false, _ -> Error "This string cannot be parse as an int"

    let map f rx =
        match rx with
        | Ok x -> Ok (f x)
        | Error s -> Error s

    let (<!>) = map

    let map2 f rx ry =
        match rx, ry with
        | Ok x, Ok y -> Ok (f x y)
        | Error x, Error y -> Error (sprintf "%s, %s" x y)        
        | Error x, _ -> Error x
        | _, Error y -> Error y

    let apply rf rx =
        map2 (fun f x -> f x) rf rx

    let (<*>) = apply

    getAnUser
        <!> notEmpty "John"
        <*> notEmpty "Doe"
        <*> getInt "42"
