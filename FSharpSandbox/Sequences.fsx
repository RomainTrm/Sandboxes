let rec nums =
    seq { yield 1
          for n in nums do yield n + 1 }

let strings = 
    nums 
    |> Seq.map (fun n -> sprintf "%d" n) 
    |> Seq.take 20 
    |> Seq.toList      