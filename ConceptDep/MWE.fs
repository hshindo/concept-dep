module MWE

open System
open System.IO
open System.Collections.Generic

let mutable count = 0

let inline split (pred: _ -> bool) (source: seq<_>) =
    seq {
        let buf = ResizeArray()
        for item in source do
            if pred item && buf.Count > 0 then
                yield buf.ToArray()
                buf.Clear()
            else buf.Add item
        }

/// continuous (BIO-style) MWE
let cont (lines: string[]) =
    let mwes = ResizeArray()
    let mutable mwe = HashSet()
    for i = 0 to lines.Length - 1 do
        let items = lines.[i].Split '\t'
        let id = items.[0] |> int

        // continuous
        let item = items.[5]
        if item.StartsWith "B-" then
            if mwe.Count > 0 then mwe |> mwes.Add
            mwe <- HashSet()
            mwe.Add id |> ignore
        else if item.StartsWith "I-" then mwe.Add id |> ignore
    if mwe.Count > 0 then mwe |> mwes.Add
    mwes.ToArray()

/// discontinuous MWE
let discont (lines: string[]) =
    let dict = Dictionary()
    for i = 0 to lines.Length - 1 do
        let items = lines.[i].Split '\t'
        let id = items.[0] |> int
        let item = items.[11]
        if item <> "-" then
            let key = int item
            if dict.ContainsKey(key) = false then
                dict.Add(key, HashSet())
                dict.[key].Add key |> ignore
            dict.[key].Add id |> ignore
    dict.Values |> Seq.toArray

/// Check whether continuous and discontinuous MWE is overlapped or not
let isOverlap (lines: string[]) =
    let conts = cont lines
    let disconts = discont lines
    let mutable b = false
    count <- count + disconts.Length
    for i = 0 to conts.Length - 1 do
        for j = 0 to disconts.Length - 1 do
            if conts.[i].Overlaps disconts.[j] then b <- true
    b

let run () =
    let path = @"C:\Users\Hiroyuki\Desktop\"
    let path = path + "wsj_mod-utf_00-24_POSUPDATE_with_PhrasalVerbs.conll"
    let data = File.ReadLines path |> split (fun s -> s.Length = 0) |> Seq.toArray
    let conflicts = ResizeArray()
    for i = 0 to data.Length - 1 do
        if isOverlap data.[i] then
            String.Join("\n", data.[i]) + "\n" |> conflicts.Add

    //for id in conflicts do id |> Console.WriteLine
    File.WriteAllLines("conflict.conll", conflicts)