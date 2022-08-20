### 2.0.1 - August 20th, 2022
- Fix `getData` APIs

### 2.0.0 - August 18th, 2022
- Make the library compatible with ES imports. This introduces a small breaking change in how it is initialized and called:

```fsharp
// in v1
importAll "@zip.js/zip.js/dist/zip-no-worker.min.js"
Zip.configure (jsOptions<IConfiguration> (fun x -> x.useWebWorkers <- false))

let writer = Zip.createBlobWriter ()

// in v2
// Assign the imports to a variable and use it as the library entry point throughout the code
// We're also no longer using the dist version of zip.js
let zip: Zip = importAll "@zip.js/zip.js/lib/zip-no-worker.js"
zip.configure (jsOptions<IConfiguration> (fun x -> x.useWebWorkers <- false))

let writer = zip.createBlobWriter ()
```

### 1.0.1 - March 22nd, 2022
- Allow IReader to be null to make it easier to add directory with IZipWriter.add
- Add IZipWriter.addDirectory as a shorthand for adding directory entries without metadata

### 1.0.0 - March 21st, 2022
- Initial version