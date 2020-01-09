module Lib

open System
open System.IO
open System.Text.RegularExpressions

let parseRules str =
    let rulePattern = @"\.\w+"
    let matches = Regex.Matches(str, rulePattern)
    if matches.Count > 0 then [for m in matches -> m.Value] else []

let printWithColor (color: ConsoleColor) (message: string) =
    Console.ForegroundColor <- color
    Console.WriteLine(message)
    Console.ResetColor()

let printErrorWithColor (color: ConsoleColor) (message: string) =
    Console.ForegroundColor <- color
    Console.Error.WriteLine(message)
    Console.ResetColor()

let removeDot (rule: string) =
    rule.Substring(1)

let typeDefOfRule rule =
    "export const " + rule + ": string;"

let toTypeDefFile content typeDef =
    content + typeDef + "\n"

let saveFile path content =
   printWithColor ConsoleColor.Magenta ("Created/Updated " + path)
   File.WriteAllText(path, content)

let handleOnChange path =
    let rec handler retries =
        if retries > 10_000
        then
             "Could not open file" |> printErrorWithColor ConsoleColor.DarkRed 
        else
            try
                File.ReadAllText path
                |> parseRules
                |> List.map removeDot
                |> List.map typeDefOfRule
                |> List.reduce toTypeDefFile
                |> saveFile (path + ".d.ts")
            with
                | :? System.IO.IOException -> 
                    ("Retrying, attempt: " + (retries + 1).ToString()) |> printWithColor ConsoleColor.Yellow
                    handler (retries + 1)
    handler 0

