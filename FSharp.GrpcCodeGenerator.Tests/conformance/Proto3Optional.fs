module Proto3Optional

open Xunit
open FSharp.GrpcCodeGenerator.TestProtos.FSharp
open FSharp.GrpcCodeGenerator.TestProtos
open Google.Protobuf

[<Fact>]
let ``Proto3 optional fields should serialize correctly and be consumable by csharp code`` () =
    let nestedMessage: FSharp.TestProto3Optional.Types.NestedMessage = {
        Bb = ValueSome(3)
        _UnknownFields = null
    }

    let fsharp: FSharp.TestProto3Optional = {
        SingularInt32 = ValueSome(1)
        SingularInt64 = ValueSome(2L)
        OptionalInt32 = ValueSome(3)
        OptionalInt64 = ValueSome(4L)
        OptionalUint32 = ValueSome(5u)
        OptionalUint64 = ValueSome(6uL)
        OptionalSint32 = ValueSome(7)
        OptionalSint64 = ValueSome(8L)
        OptionalFixed32 = ValueSome(9u)
        OptionalFixed64 = ValueSome(10uL)
        OptionalSfixed32 = ValueSome(11)
        OptionalSfixed64 = ValueSome(12L)
        OptionalFloat = ValueSome(13.0f)
        OptionalDouble = ValueSome(14.0)
        OptionalBool = ValueSome(true)
        OptionalString = ValueSome("str")
        OptionalBytes = ValueSome(ByteString.FromBase64("eW8="))
        _UnknownFields = null
        OptionalCord = ValueSome("cord")
        OptionalNestedMessage = ValueSome(nestedMessage)
        LazyNestedMessage = ValueSome(nestedMessage)
        OptionalNestedEnum = ValueSome(FSharp.TestProto3Optional.Types.NestedEnum.Foo)
    }

    let fsharpBytes = fsharp.ToByteArray()
    let csharp = CSharp.TestProto3Optional.Parser.ParseFrom(fsharpBytes)

    Assert.Equal(fsharp.SingularInt32.Value, csharp.SingularInt32)
    Assert.Equal(fsharp.SingularInt64.Value, csharp.SingularInt64)
    Assert.Equal(fsharp.OptionalInt32.Value, csharp.OptionalInt32)
    Assert.Equal(fsharp.OptionalInt64.Value, csharp.OptionalInt64)
    Assert.Equal(fsharp.OptionalUint32.Value, csharp.OptionalUint32)
    Assert.Equal(fsharp.OptionalUint64.Value, csharp.OptionalUint64)
    Assert.Equal(fsharp.OptionalSint32.Value, csharp.OptionalSint32)
    Assert.Equal(fsharp.OptionalSint64.Value, csharp.OptionalSint64)
    Assert.Equal(fsharp.OptionalFixed32.Value, csharp.OptionalFixed32)
    Assert.Equal(fsharp.OptionalFixed64.Value, csharp.OptionalFixed64)
    Assert.Equal(fsharp.OptionalSfixed32.Value, csharp.OptionalSfixed32)
    Assert.Equal(fsharp.OptionalSfixed64.Value, csharp.OptionalSfixed64)
    Assert.Equal(fsharp.OptionalFloat.Value, csharp.OptionalFloat)
    Assert.Equal(fsharp.OptionalDouble.Value, csharp.OptionalDouble)
    Assert.Equal(fsharp.OptionalBool.Value, csharp.OptionalBool)
    Assert.Equal(fsharp.OptionalString.Value, csharp.OptionalString)
    Assert.Equal(fsharp.OptionalBytes.Value.ToString(), csharp.OptionalBytes.ToString())
    Assert.Equal(fsharp.OptionalCord.Value, csharp.OptionalCord)
    Assert.Equal(fsharp.OptionalNestedMessage.Value.Bb.Value, csharp.OptionalNestedMessage.Bb)
    Assert.Equal(fsharp.LazyNestedMessage.Value.Bb.Value, csharp.LazyNestedMessage.Bb)
    Assert.Equal(int fsharp.OptionalNestedEnum.Value, int csharp.OptionalNestedEnum)