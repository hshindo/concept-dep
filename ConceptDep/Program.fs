open System

[<EntryPoint>]
let main argv = 
    //printfn "%A" argv

    //MWE.run ()
    Lemma.shapeNN ()

    Console.ReadLine() |> ignore
    0 // return an integer exit code