module Lemma

open System
open System.IO
open System.Collections.Generic

/// Merge nn csv files in NAIST EDic
let mergeNN () =
    let path = "nn.csv"
    let dict = Dictionary()
    let ra = ResizeArray()
    for line in File.ReadLines path do
        let items = line.Split ','
        dict.[items.[0]] <- line
        //dict.Add(items.[0], line)
        ra.Add line

    let path = "other.csv"
    for line in File.ReadLines path do
        let items = line.Split ','
        if dict.ContainsKey items.[0] = false then
            if items.[0] = items.[2] then
                let v = Array.append items [| "U"; "no" |]
                let v = String.Join(",", v)
                ra.Add v
            else
                // currently ignores this case: ex. building - build, weighting - weight
                ()

    File.WriteAllLines("mergeNN.csv", ra)
