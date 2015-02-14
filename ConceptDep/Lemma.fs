module Lemma

open System
open System.IO
open System.Collections.Generic

let removeDuplicate () =
    let path = "mergenn.csv"
    let dict = Dictionary()
    for line in File.ReadLines path do
        let items = line.Split ','
        dict.[items.[0]] <- line
    File.WriteAllLines(path, dict.Values)

let shapeNN () =
    let path = "nn.csv"
    let dict = Dictionary()
    for line in File.ReadLines path do
        let items = line.Split ','
        if items.[0] = items.[2] then
            dict.[items.[0]] <- line

    let path = "nns.csv"
    for line in File.ReadLines path do
        let items = line.Split ','
        if dict.ContainsKey items.[2] then
            dict.[items.[2]] <- String.Format("{0},NN,{0},C/U", items.[2])
    File.WriteAllLines(path, dict.Values)

/// Merge nn csv files in NAIST EDic
let mergeNN () =
    let path = "other.csv"
    let dict = Dictionary()
    let ra = ResizeArray()
    for line in File.ReadLines path do
        let items = line.Split ','
        if dict.ContainsKey items.[0] = false then
            dict.[items.[0]] <- line // basically no problem
            ra.Add line

    let path = "nn.csv" // merge of regular.csv, irregular.csv, and noChange.csv
    for line in File.ReadLines path do
        let items = line.Split ','
        if dict.ContainsKey items.[0] = false then
            if items.[0] = items.[2] then
                let v = Array.append items [| "U"; "no" |]
                let v = String.Join(",", v)
                dict.Add(items.[0], line)
                ra.Add v
            else
                // currently ignores this case: ex. building - build, weighting - weight
                ()

    File.WriteAllLines("mergeNN.csv", ra)