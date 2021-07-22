module Proto3Optional

open Xunit
open FSharp.GrpcCodeGenerator.TestProtos.FSharp
open FSharp.GrpcCodeGenerator.TestProtos
open Google.Protobuf

[<Fact>]
let ``Proto3 optional fields should serialize correctly and be consumable by csharp code`` () =
    let nestedMessage: FSharp.TestProto3Optional.Types.NestedMessage = {
        Bb = 3
        _UnknownFields = null
    }

    let fsharp: FSharp.TestProto3Optional = {
        SingularInt32 = 1
        SingularInt64 = 2L
        OptionalInt32 = 3
        OptionalInt64 = 4L
        OptionalUint32 = 5u
        OptionalUint64 = 6uL
        OptionalSint32 = 7
        OptionalSint64 = 8L
        OptionalFixed32 = 9u
        OptionalFixed64 = 10uL
        OptionalSfixed32 = 11
        OptionalSfixed64 = 12L
        OptionalFloat = 13.0f
        OptionalDouble = 14.0
        OptionalBool = true
        OptionalString = "str"
        OptionalBytes = ByteString.FromBase64("eW8=")
        _UnknownFields = null
        OptionalCord = "cord"
        OptionalNestedMessage = ValueSome(nestedMessage)
        LazyNestedMessage = ValueSome(nestedMessage)
        OptionalNestedEnum = FSharp.TestProto3Optional.Types.NestedEnum.Foo
    }

    let fsharpBytes = fsharp.ToByteArray()
    let csharp = CSharp.TestProto3Optional.Parser.ParseFrom(fsharpBytes)

    Assert.Equal(fsharp.SingularInt32, csharp.SingularInt32)
    Assert.Equal(fsharp.SingularInt64, csharp.SingularInt64)
    Assert.Equal(fsharp.OptionalInt32, csharp.OptionalInt32)
    Assert.Equal(fsharp.OptionalInt64, csharp.OptionalInt64)
    Assert.Equal(fsharp.OptionalUint32, csharp.OptionalUint32)
    Assert.Equal(fsharp.OptionalUint64, csharp.OptionalUint64)
    Assert.Equal(fsharp.OptionalSint32, csharp.OptionalSint32)
    Assert.Equal(fsharp.OptionalSint64, csharp.OptionalSint64)
    Assert.Equal(fsharp.OptionalFixed32, csharp.OptionalFixed32)
    Assert.Equal(fsharp.OptionalFixed64, csharp.OptionalFixed64)
    Assert.Equal(fsharp.OptionalSfixed32, csharp.OptionalSfixed32)
    Assert.Equal(fsharp.OptionalSfixed64, csharp.OptionalSfixed64)
    Assert.Equal(fsharp.OptionalFloat, csharp.OptionalFloat)
    Assert.Equal(fsharp.OptionalDouble, csharp.OptionalDouble)
    Assert.Equal(fsharp.OptionalBool, csharp.OptionalBool)
    Assert.Equal(fsharp.OptionalString, csharp.OptionalString)
    Assert.Equal(fsharp.OptionalBytes.ToString(), csharp.OptionalBytes.ToString())
    Assert.Equal(fsharp.OptionalCord, csharp.OptionalCord)
    Assert.Equal(fsharp.OptionalNestedMessage.Value.Bb, csharp.OptionalNestedMessage.Bb)
    Assert.Equal(fsharp.LazyNestedMessage.Value.Bb, csharp.LazyNestedMessage.Bb)
    Assert.Equal(int fsharp.OptionalNestedEnum, int csharp.OptionalNestedEnum)

[<Fact>]
let ``Proto3 optional fields should deserialize correctly and be consumable from csharp`` () =
    let nestedMessage = CSharp.TestProto3Optional.Types.NestedMessage(Bb = 3)

    let csharp = CSharp.TestProto3Optional(SingularInt32 = 1, SingularInt64 = 2L, OptionalInt32 = 3, OptionalInt64 = 4L, OptionalUint32 = 5u, OptionalUint64 = 6uL, OptionalSint32 = 7, OptionalSint64 = 8L, OptionalFixed32 = 9u, OptionalFixed64 = 10uL, OptionalSfixed32 = 11, OptionalSfixed64 = 12L, OptionalFloat = 13.0f, OptionalDouble = 14.0, OptionalBool = true, OptionalString = "str", OptionalBytes = ByteString.FromBase64("eW8="), OptionalCord = "cord", OptionalNestedMessage = nestedMessage, LazyNestedMessage = nestedMessage, OptionalNestedEnum = CSharp.TestProto3Optional.Types.NestedEnum.Foo)

    let csharpBytes = csharp.ToByteArray()
    let fsharp = FSharp.TestProto3Optional.Parser.ParseFrom(csharpBytes)

    Assert.Equal(csharp.SingularInt32, fsharp.SingularInt32)
    Assert.Equal(csharp.SingularInt64, fsharp.SingularInt64)
    Assert.Equal(csharp.OptionalInt32, fsharp.OptionalInt32)
    Assert.Equal(csharp.OptionalInt64, fsharp.OptionalInt64)
    Assert.Equal(csharp.OptionalUint32, fsharp.OptionalUint32)
    Assert.Equal(csharp.OptionalUint64, fsharp.OptionalUint64)
    Assert.Equal(csharp.OptionalSint32, fsharp.OptionalSint32)
    Assert.Equal(csharp.OptionalSint64, fsharp.OptionalSint64)
    Assert.Equal(csharp.OptionalFixed32, fsharp.OptionalFixed32)
    Assert.Equal(csharp.OptionalFixed64, fsharp.OptionalFixed64)
    Assert.Equal(csharp.OptionalSfixed32, fsharp.OptionalSfixed32)
    Assert.Equal(csharp.OptionalSfixed64, fsharp.OptionalSfixed64)
    Assert.Equal(csharp.OptionalFloat, fsharp.OptionalFloat)
    Assert.Equal(csharp.OptionalDouble, fsharp.OptionalDouble)
    Assert.Equal(csharp.OptionalBool, fsharp.OptionalBool)
    Assert.Equal(csharp.OptionalString, fsharp.OptionalString)
    Assert.Equal(csharp.OptionalBytes.ToString(), fsharp.OptionalBytes.ToString())
    Assert.Equal(csharp.OptionalCord, fsharp.OptionalCord)
    Assert.Equal(csharp.OptionalNestedMessage.Bb, fsharp.OptionalNestedMessage.Value.Bb)
    Assert.Equal(csharp.LazyNestedMessage.Bb, fsharp.LazyNestedMessage.Value.Bb)
    Assert.Equal(int csharp.OptionalNestedEnum, int fsharp.OptionalNestedEnum)
