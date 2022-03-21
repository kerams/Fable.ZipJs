namespace Fable.ZipJs

open System
open Browser.Types
open Fable.Core

type IConfiguration =
    /// Enables the use of Web workers to compress and uncompress data in non-blocking background processes. The default value is true.
    abstract useWebWorkers: bool with get, set

    /// Delay before terminating a web worker in ms. The default value is 5000. You can call zip.terminateWorkers() to terminate idle workers.
    abstract terminateWorkerTimeout: int with get, set

    /// Number of workers used simultaneously to process data. The default value is the number of logical cores returned by the attribute navigator.hardwareConcurrency or 2 if navigator is undefined.
    abstract maxWorkers: int with get, set

type IReader =
    /// The size of data to read in bytes
    abstract member size: float

    /// Initialize the zip.Reader object
    abstract member init: unit -> JS.Promise<unit>

type IBlobReader =
    inherit IReader

type IStringReader =
    inherit IReader

type IBytesReader =
    inherit IReader

type IWriter =
    /// The size of written data in bytes
    abstract member size: float

    /// Initialize the zip.Writer object
    abstract member init: unit -> JS.Promise<unit>

type IBlobWriter =
    inherit IWriter

    abstract member getData: unit -> Blob

type IStringWriter =
    inherit IWriter

    abstract member getData: unit -> JS.Promise<string>

type IBytesWriter =
    inherit IWriter

    abstract member getData: unit -> byte[]

type IGetDataOptions =
    /// function tracking the task progress and having as parameters an index (number) value and a max (number) value (undefined by default)
    abstract onprogress: Action<int, int> with get, set

    /// boolean value specifying if the signature of the file should be verified (false by default)
    abstract checkSignature: bool with get, set

    /// password used to decrypt the file encrypted with AES or ZipCrypto (undefined by default)
    abstract password: string with get, set

    /// boolean value specifying if web workers should be used (undefined by default)
    abstract useWebWorkers: bool with get, set

type IEntry =
    /// file name
    abstract filename: string

    /// file name in binary format
    abstract rawFilename: byte[]

    /// true if the filename is encoded in UTF-8
    abstract filenameUTF8: bool

    /// true if the entry is a directory
    abstract directory: bool

    /// true if the entry is encrypted
    abstract encrypted: bool

    /// compressed data size
    abstract compressedSize: float

    /// uncompressed data size
    abstract uncompressedSize: float

    /// last modification date
    abstract lastModDate: DateTime

    /// last access date (current date by default)
    abstract lastAccessDate: DateTime

    /// creation date (current date by default)
    abstract creationDate: DateTime

    /// last modification date in binary format (MS-DOS)
    abstract rawLastModDate: float

    /// file comment
    abstract comment: string

    /// file comment in binary format
    abstract rawComment: byte[]

    /// true if the comment is encoded in UTF-8
    abstract commentUTF8: bool

    /// Map of extra field data where the key (number) is the type of the extra field and the value is a UInt8Array
    abstract extraField: System.Collections.Generic.IDictionary<int, obj>

    /// extra field in binary format
    abstract rawExtraField: byte[]

    /// signature (crc checksum or AES authentication code) of the file
    abstract signature: U2<float, byte[]>

    /// true if the file is formatted in Zip64
    abstract zip64: bool

    /// compression method
    abstract compressionMethod: int

    /// Get the data of a zip entry
    /// Returns a promise with the output data as resolved value
    abstract getData: writer: IWriter * ?options: IGetDataOptions -> JS.Promise<obj>

    /// Get the data of a zip entry
    /// Returns a promise with the output data as resolved value
    [<Emit("$0.getData(new zip.BlobWriter(), $1)")>]
    abstract getDataBlob: ?options: IGetDataOptions -> JS.Promise<Blob>

    /// Get the data of a zip entry
    /// Returns a promise with the output data as resolved value
    [<Emit("$0.getData(new zip.Uint8ArrayWriter(), $1)")>]
    abstract getDataBytes: ?options: IGetDataOptions -> JS.Promise<byte[]>

    /// Get the data of a zip entry
    /// Returns a promise with the output data as resolved value
    [<Emit("$0.getData(new zip.TextWriter(), $1)")>]
    abstract getDataString: ?options: IGetDataOptions -> JS.Promise<string>

type IGetEntriesOptions =
    /// function tracking the task progress and having as parameters an index (number) value, a max (number) value (undefined by default) and the file entry
    abstract onprogress: Action<int, int> with get, set

    /// encoding of filenames stored in another encoding than UTF-8 (cp437 by default)
    abstract filenameEncoding: string with get, set

    /// encoding of comments stored in another encoding than UTF-8 (cp437 by default)
    abstract commentEncoding: string with get, set

type IZipReader =
    /// Get all entries from a zip.
    /// Returns a promise with the Array of Entry objects as resolved value
    abstract getEntries: ?options: IGetEntriesOptions -> JS.Promise<IEntry[]>

    /// Close the opened zip file.
    /// Returns a promise with undefined as resolved value
    abstract close: unit -> JS.Promise<unit>

type IZipReaderOptions =
    /// boolean value specifying if the signature of all the files should be verified (false by default)
    abstract checkSignature: bool with get, set

    /// password used to decrypt all the files (undefined by default)
    abstract password: string with get, set

    /// encoding of filenames stored in another encoding than UTF-8 (cp437 by default)
    abstract filenameEncoding: string with get, set

    /// encoding of comments stored in another encoding than UTF-8 (cp437 by default)
    abstract commentEncoding: string with get, set

    /// boolean value specifying if web workers should be used (undefined by default)
    abstract useWebWorkers: bool with get, set

type IAddOptions =
    /// function tracking the task progress and having as parameters an index (number) value and a max (number) value (undefined by default)
    abstract onprogress: Action<int, int> with get, set

    /// true if the entry is a directory (undefined by default)
    abstract directory: bool with get, set

    /// compression level from 0 (no compression) to 9 (max. compression) (5 by default)
    abstract level: int with get, set

    /// true to explicitely allow calling ZipWriter#add multiple times in parallel (false by default)
    abstract bufferedWrite: bool with get, set

    /// true to keep the files in order when calling ZipWriter#add multiple times in parallel (true by default)
    abstract keepOrder: bool with get, set

    /// file comment (undefined by default)
    abstract comment: string with get, set

    /// last modification date (current date by default)
    abstract lastModDate: DateTime with get, set

    /// use extended timestamp and ntfs extra fields (true by default)
    abstract extendedTimestamp: DateTime with get, set

    /// last access date (current date by default)
    abstract lastAccessDate: DateTime with get, set

    /// creation date (current date by default)
    abstract creationDate: DateTime with get, set

    /// zip version (undefined by default)
    abstract version: float with get, set

    /// "version made by" field (29 by default)
    abstract versionMadeBy: float with get, set

    /// password used to encrypt the file with AES or ZipCrypto (undefined by default)
    abstract password: string with get, set

    /// strength of the encryption algorithm: 1 for AES-128, 2 for AES-192, 3 for AES-256 (3 by default)
    abstract encryptionStrength: int with get, set

    /// use ZipCrypto instead of AES (false by default)
    abstract zipCrypto: bool with get, set

    /// force the file entry to be written in Zip64 (undefined by default)
    abstract zip64: bool with get, set

    /// Map of extra field data where the key (number) is the type of the extra field and the value is a UInt8Array (undefined by default)
    abstract extraField: System.Collections.Generic.IDictionary<int, obj> with get, set

    /// boolean value specifying if web workers should be used (undefined by default)
    abstract useWebWorkers: bool with get, set

    /// boolean value specifying if the data descriptor should be included (true by default)
    abstract dataDescriptor: bool with get, set

    /// boolean value specifying if the data descriptor signature should be included (true by default)
    abstract dataDescriptorSignature: bool with get, set

    /// internal file attribute (undefined by default)
    abstract internalFileAttribute: float with get, set

    /// external file attribute (undefined by default)
    abstract externalFileAttribute: float with get, set

type ICloseOptions =
    /// function tracking the task progress and having as parameters an index (number) value, a max (number) value (undefined by default) and the file entry
    abstract onprogress: Action<int, int> with get, set

    /// force the file entry to be written in Zip64 (undefined by default)
    abstract zip64: bool with get, set

type IZipWriter =
    /// Add a new entry into the zip
    abstract add: name: string * reader: IReader * ?options: IAddOptions -> JS.Promise<IEntry>

    /// Close the zip file
    abstract close: ?comment: byte[] * ?options: ICloseOptions -> JS.Promise<unit>

type IZipWriterOptions =
    /// force all the files to be written in Zip64 (false by default)
    abstract zip64: bool with get, set

    /// compression level applied on all files (5 by default)
    abstract level: int with get, set

    /// true to explicitely allow calling ZipWriter#add multiple times in parallel (false by default)
    abstract bufferedWrite: bool with get, set

    /// true to keep the files in order when calling ZipWriter#add multiple times in parallel (true by default)
    abstract keepOrder: bool with get, set

    /// last modification date of all files (current date by default)
    abstract lastModDate: DateTime with get, set

    /// use extended timestamp and ntfs extra fields (true by default)
    abstract extendedTimestamp: DateTime with get, set

    /// last access date of all files (current date by default)
    abstract lastAccessDate: DateTime with get, set

    /// creation date of all files (current date by default)
    abstract creationDate: DateTime with get, set

    /// zip version of all files (undefined by default)
    abstract version: float with get, set

    /// "version made by" field of all files (29 by default)
    abstract versionMadeBy: float with get, set

    /// password used to encrypt all the files (undefined by default)
    abstract password: string with get, set

    /// strength of the encryption algorithm: 1 for AES-128, 2 for AES-192, 3 for AES-256 (3 by default)
    abstract encryptionStrength: int with get, set

    /// use ZipCrypto instead of AES (false by default)
    abstract zipCrypto: bool with get, set

    /// boolean value specifying if web workers should be used (undefined by default)
    abstract useWebWorkers: bool with get, set

    /// boolean value specifying if the data descriptor should be included (true by default)
    abstract dataDescriptor: bool with get, set

    /// boolean value specifying if the data descriptor signature should be included (true by default)
    abstract dataDescriptorSignature: bool with get, set

    /// internal file attribute of all files (undefined by default)
    abstract internalFileAttribute: float with get, set

    /// external file attribute of all files (undefined by default)
    abstract externalFileAttribute: float with get, set

type Zip =
    /// Create a ZipReader object. A ZipReader object helps to read the zipped content.
    [<Emit("new zip.ZipReader($0, $1)")>]
    static member inline createZipReader (reader: IReader, ?options: IZipReaderOptions) = jsNative<IZipReader>

    /// Create a ZipReader object. A ZipReader object helps to read the zipped content.
    [<Emit("new zip.ZipReader(new zip.BlobReader($0), $1)")>]
    static member inline createZipReader (blob: Browser.Types.Blob, ?options: IZipReaderOptions) = jsNative<IZipReader>

    /// Create a ZipReader object. A ZipReader object helps to read the zipped content.
    [<Emit("new zip.ZipReader(new zip.TextReader($0), $1)")>]
    static member inline createZipReader (text: string, ?options: IZipReaderOptions) = jsNative<IZipReader>

    /// Create a ZipReader object. A ZipReader object helps to read the zipped content.
    [<Emit("new zip.ZipReader(new zip.Uint8ArrayReader($0), $1)")>]
    static member inline createZipReader (bytes: byte[], ?options: IZipReaderOptions) = jsNative<IZipReader>

    /// Create a ZipWriter object
    [<Emit("new zip.ZipWriter($0, $1)")>]
    static member inline createZipWriter (writer: IWriter, ?options: IZipWriterOptions) = jsNative<IZipWriter>

    /// Blob object writer constructor
    [<Emit("new zip.BlobWriter($0)")>]
    static member inline createBlobWriter (?mimeType: string) = jsNative<IBlobWriter>

    /// string writer constructor
    [<Emit("new zip.TextWriter()")>]
    static member inline createStringWriter () = jsNative<IStringWriter>

    /// Uint8Array object writer constructor
    [<Emit("new zip.Uint8ArrayWriter()")>]
    static member inline createBytesWriter () = jsNative<IBytesWriter>

    /// Blob object reader constructor
    [<Emit("new zip.BlobReader($0)")>]
    static member inline createBlobReader (blob: Blob) = jsNative<IBlobReader>

    /// string reader constructor
    [<Emit("new zip.TextReader($0)")>]
    static member inline createStringReader (text: string) = jsNative<IStringReader>

    /// Uint8Array object reader constructor
    [<Emit("new zip.Uint8ArrayReader($0)")>]
    static member inline createBytesReader (bytes: byte[]) = jsNative<IBytesReader>

    [<Emit("zip.configure($0)")>]
    static member inline configure (configuration: IConfiguration) = jsNative<unit>

    [<Emit("zip.terminateWorkers()")>]
    static member inline terminateWorkers () = jsNative<unit>