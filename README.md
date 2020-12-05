# FSharp.GrpcCodeGenerator

This project is aimed at providing complete integration of GRPC and Protobuf into the F# language.

First, a word of warning:
Currently, only a `protoc` plugin is implemented which enables the generation of F# sources from `.proto` files.
While this is enough to be able to use GRPC in pure F#, it is in no way a complete solution and takes effort to set up correctly.
This project should be considered a work in progress.
At least the following will have to be implemented to offer a good development experience:

* The code generator available as an installable dotnet tool
* Server and Client nuget packages
* A compile-time package to add a `Protobuf` MSBuild target which enables automatic code generation
* Integration with existing web servers (e.g. Saturn)

## How does it work?

Now that that's out of the way, let's see how it all works.

### Messages

Protobuf messages are transformed into F# records.
This means we get to use F#'s automatic implementations for `Equals`, `ToString` etc.
Now, you may be used to strictly immutable records, so be warned:
the records contain ***mutable*** fields.
This is because the .NET protobuf runtime is implemented with mutability in mind, and object are constructed and initialized in separate phases.
It'd be a much bigger task to adapt the runtime to strictly immutable messages.

### Fields

All primitive and message fields are transformed into `ValueOption` fields.
This is because any field can simply be missing from a protobuf message,
and we don't want to start guessing whether a 0 came in the actual message or was filled in as a default.

On the plus side, we no longer need `HasXXX` and `ClearXXX` calls.
Simply match the `ValueOption` to test for a field's presence and set it to `ValueNone` to clear it from the message.
Another benefit is that the deserializer **never** generates null fields;
if you see one, it's a bug that needs to be reported.

But why use value option, I hear you ask?
Performance!
I wouldn't want each field of each message to cause an allocation.
I know you wouldn't either.
Someone *could* implement a switch to override this behaviour, but I won't.
Allocation is evil.

And BTW, if you think handling all those options all over your code is troublesome,
you need to stop passing network DTO's into your business logic.
You may want to take a look at [Onion architecture](https://www.google.com/search?q=onion+architecture).

### Collections

Repeated and map fields use the built-in `RepeatedField` and `MapField` types from protobuf,
which include special support for the binary protocol.

### Enums

Enums are transformed into CLR enums, not F# unions.
This is due to the fact that an enum field is, in the end, a number field,
which is a perfect match for CLR enums, including the possibility of encountering numbers with no assigned name.

### One-of

One-of fields, however, are transformed into F# union types.
Contrary to the C# plugin which generates separate properties for each one-of entry,
you'll get only one record field of type `ValueOption<UnionType>`.
A `ValueNone` indicates none of the fields were present in the message.
If one was present, you'll get a `ValueSome(UnionCase)`.
If more than one is present (and you have a malformed message), only the last value will be preserved.

### Helpers

Each message gets an accompanying module. It contains:

* A `Parser` you can use to read binary messages.
* An `empty()` function which returns an empty record with all fields set to none or empty collections.
You can use this to start constructing new messages.
* Field number and codec definitions.

### Reflection

Each file also gets a `Reflection` module.
This module contains the reflection descriptor for the whole file.
It also contains descriptors for each message type, should you need to access them by name.

### Services

Services are transformed into classes.
I plan to change this behaviour, but it will take effort to work around the very OO runtime,
so for now, you get a client class and an abstract server base class.
The one divergence from the C# code generator is that the server base class contains no default logic to return a "not implemented" response;
you get abstract methods instead.

### What else?

* You may need to be aware of the fact that any type inside the `Google.Protobuf.*` namespace will have its namespace rewritten to `Google.Protobuf.FSharp.*`.
This is to keep the types from clashing with the ones provided inside the `Google.Protobuf` package, which you always need to reference.
So, for example, you get the `any` type at `Google.Protobuf.FSharp.WellKnownTypes.Any`.

* The code generator respects the `csharp_namespace` option.
There is currently no separate `fsharp_namespace` option.
I don't know whether this behaviour needs to be changed.

* Contrary to the C# version, this code generator has no special handling for the types in `wrappers.proto`,
since a `ValueOption<uint64>` is completely adequate for use in place of a `Nullable<uint64>`.
See [here](https://docs.microsoft.com/en-us/aspnet/core/grpc/protobuf?view=aspnetcore-5.0#nullable-types) for more info.

## How do I use it?

This project is in an early stage; so this is going to be a bit harder than necessary.

* Clone this repo.
* Build the `FSharp.GrpcCodeGenerator` project into an executable for your OS.
A .NET DLL without an accompanying OS-specific executable can't be used.
* Rename the executable to `protoc-gen-fsharp` and place it somewhere inside your `PATH`.
* Invoke it by running `protoc` with the `--fsharp_out` flag: `protoc my-proto-file.proto --fsharp_out=./generated-sources`
  * Bear in mind that for now, you will have to generate sources for all built-in types (e.g. `any`) yourself.
  You don't need to do this if you don't use the built-in types.
  * You can use the `--fsharp_opt=no-server` and `--fsharp_opt=no-client` flags to control GRPC service code generation.
* Place the generated files inside your project.
* Add a reference to the `Google.Protobuf` and, if you use GRPC services, `Grpc.Core` packages.
  * To create a GRPC server, you need the `Grpc.AspNetCore` package.
  * To create a client, you need `Grpc.Net.Client`.

## Show me some code

Here you are.
First, reading and writing messages directly:

```fsharp
// Read a message
use stdIn = Console.OpenStandardInput()
let req = Compiler.CodeGeneratorRequest.Parser.ParseFrom(stdIn)

// Do something with it
let files = ... // left out

// Create a response message
let resp = { Compiler.CodeGeneratorResponse.empty() with SupportedFeatures = ValueSome <| uint64 Compiler.CodeGeneratorResponse.Types.Feature.Proto3Optional }
resp.File.AddRange files

// Write it somewhere
use stdOut = Console.OpenStandardOutput()
Google.Protobuf.MessageExtensions.WriteTo(resp, stdOut)
```

Here's a service client:

```fsharp
use channel = GrpcChannel.ForAddress("https://localhost:5001/")
let client = Greet.GreeterService.GreeterServiceClient(channel)
let req = { Greet.HelloRequest.empty() with Name = ValueSome "World" }
let resp = client.SayHello(req).ResponseAsync.Result
printfn "%s" resp.Message
```

And a service implementation:

```fsharp
type GreeterService() =
    inherit Greet.GreeterService.GreeterServiceBase()

    override _.SayHello req ctx =
        let resp =
            { Greet.HelloReply.empty() with
                // Notice how we're immediately forced to handle missing fields.
                // The language itself protects you from the binary protocol's quirks.
                // How cool is THAT?
                Message = req.Name |> ValueOption.map (sprintf "Hello, %s!")
            }
        Threading.Tasks.Task.FromResult(resp)
```

## A note on protoc plugins

It took me considerable effort (much more than the ~5 minutes it takes to read the following paragraphs) to figure out how to implement a `protoc` plugin.
If there is adequate documentation anywhere, I must have missed it;
so I'll document what I've learned here.

The protocol buffers compiler supports plugins in the shape of executables named `protoc-gen-XXXX`, where `XXXX` is the name of the plugin.
You enable a plugin by specifying `--XXXX_out=some_directory` and optionally pass it arguments by specifying `--XXXX_opt=opt1=val1,opt2=val2`.
`protoc` attempts to execute `protoc-gen-XXXX` and write a serialized code generation request to the process's `stdin`.
It then waits for the process to write a code generation response back to its `stdout`.
The request and response are serialized as (you guessed it!) a protobuf message.
The contract for these types is available from `google/protobuf/compiler/plugin.proto`, found inside the `protoc` download archive.

(Fascinatingly, the C# implementation inside `protoc` does not handle GRPC services.
Those are generated by a separate plugin.
This is why in C# you get two source files per `.proto` file instead of one.)

If you're implementing a plugin in C++, you can use a bunch of utilities the protobuf team have made available,
but if you're implementing a plugin in another language,
you'll have to be able to understand protobuf in order to understand protobuf, so to speak.
This chicken-and-egg situation is the same as with any language which has its primary compiler implemented in the language itself.
You'd basically have to implement the initial version in another language and then use *that* to implement the language again, this time in itself.
I was lucky enough to have the C# protobuf plugin available,
which means I implemented the original code in F#,
and only had to account for the change from classes to records and such minor cases.
It may not be as easy for another language where no support is available.
