# Fable.ZipJs

Fable bindings for the Core API of [zip.js](https://github.com/gildas-lormeau/zip.js) ([website](https://gildas-lormeau.github.io/zip.js/index.html), [NPM package](https://www.npmjs.com/package/@zip.js/zip.js)) version 2.4.7+.

## Nuget package
[![Nuget](https://img.shields.io/nuget/v/Fable.ZipJs.svg?colorB=green)](https://www.nuget.org/packages/Fable.ZipJs)

## Installation with [Femto](https://github.com/Zaid-Ajaj/Femto)

```
femto install Fable.ZipJs
```

## Standard installation

Nuget package

```
paket add Fable.ZipJs -p YourProject.fsproj
```

NPM package

```
npm install @zip.js/zip.js@2.4.7
```

## Usage

Firstly, you'll need to import zip.js in the Fable application. This library does not do it automatically because zip.js provides [bundles of various sizes based on usage scenarios](https://github.com/gildas-lormeau/zip.js/tree/master/dist), possibly because its tree-shakability is limited.

Here we'll be using `zip-no-worker.js`, which allows both compression and decompression, and does not support web workers. Simultaneously, we globally configure zip.js not to use web workers. Do this once for the lifetime of your application.

```fsharp
open Fable.Core.JsInterop
open Fable.ZipJs

let zip: Zip = importAll "@zip.js/zip.js/lib/zip-no-worker.js"
zip.configure (jsOptions<IConfiguration> (fun x -> x.useWebWorkers <- false))
```

The snippets also assume you're using [Fable.Promise](https://www.nuget.org/packages/Fable.Promise/) and its `promise` builder. In an Elmish application you would then pass the promises to `Cmd.OfPromise.either` or `perform`.

### Decompression

Given a file containing a zip (provided by the user via an HTML file input for instance), list its contents. For simplicity's sake, we're skipping error handling (file is not a zip), but you can easily use try-with to that end.

```fsharp
let listContents (file: Browser.Types.Blob) = promise {
    // Create a ZipReader capable of decompressing data in a blob.
    let zipReader = zip.createZipReader file

    // Get the entries in the zip.
    let! entries = zipReader.getEntries ()

    for e in entries do
        printfn "Name: %s, compressed size: %f, is a directory: %O" e.filename e.compressedSize e.directory

        // Get the decompressed file contents as a byte array.
        let! dataBytes = e.getDataBytes ()
        // or a blob
        let! dataBlob = e.getDataBlob ()
        // or a string
        let! dataString = e.getDataString ()

        // Do something with the data.
        ()

    // Close the ZipReader when done.
    do! zipReader.close () }
```

### Compression

Create a password-protected zip file with a single entry from some array of bytes.

```fsharp
let zipDataWithPassword (data: byte[]) fileName password = promise {
    // The writer type determines the type of the zipped data you get at the end.
    // If you want a byte array, use zip.createBytesWriter.
    let writer = zip.createBlobWriter ()

    // Create a ZipWriter for compressing files into the provided writer.
    // Also set a password using the optional IZipWriterOptions parameter.
    let zipWriter = zip.createZipWriter (writer, jsOptions<IZipWriterOptions> (fun x -> x.password <- password))

    // Add a file into the zip. Because the input is a byte array, wrap it in IBytesReader.
    // Additionally, you can indicate whether you're adding a directory, or set various metadata using the optional IAddOptions parameter.
    let! addedEntry = zipWriter.add (fileName, zip.createBytesReader data, jsOptions<IAddOptions> (fun x -> x.comment <- "blah"))

    // Close the ZipWriter, finalizing the zip file.
    do! zipWriter.close ()

    // From the original BlobWriter, return a blob containing the newly created zip.
    return writer.getData () }
```
