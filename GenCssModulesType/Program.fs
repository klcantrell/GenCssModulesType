open System
open System.IO
open Lib

[<EntryPoint>]
let main argv =
    printWithColor ConsoleColor.White "Edit a CSS file in this directory or subdirectories..."
    printWithColor ConsoleColor.White "TypeScript type defs shall be generated"
    Console.WriteLine()
    printWithColor ConsoleColor.White "Enter quit() to exit"
    Console.WriteLine()

    let fileSystemWatcher = new FileSystemWatcher()
    fileSystemWatcher.Path <- "."
    fileSystemWatcher.Filter <- "*.css"
    fileSystemWatcher.IncludeSubdirectories <- true
    fileSystemWatcher.Changed.Add(fun e -> handleOnChange e.FullPath)
    fileSystemWatcher.EnableRaisingEvents <- true

    while (Console.ReadLine().Trim() <> "quit()") do
        ()
    0
