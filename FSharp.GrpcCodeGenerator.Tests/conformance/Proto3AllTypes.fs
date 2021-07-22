module Proto3AllTypes

open Xunit
open FSharp.GrpcCodeGenerator.TestProtos.FSharp
open FSharp.GrpcCodeGenerator.TestProtos
open Google.Protobuf
open Google.Protobuf.Collections
open System.Collections

let repeatedField<'a> a =
    let field = RepeatedField<'a>()
    field.AddRange(a)
    field

[<Fact>]
let ``Proto3 optional fields should serialize correctly and be consumable by csharp code`` () =
    let nestedMessage : FSharp.TestAllTypes.Types.NestedMessage = { Bb = 3; _UnknownFields = null }

    let foreignMessage : FSharp.ForeignMessage = { _UnknownFields = null; C = 123 }

    let importMessage : FSharp.ImportMessage = { _UnknownFields = null; D = 456 }

    let publicImportMessage : FSharp.PublicImportMessage = { _UnknownFields = null; E = 789 }

    let fsharp : FSharp.TestAllTypes =
        { _UnknownFields = null
          SingleInt32 = 1
          SingleInt64 = 2L
          SingleUint32 = 3u
          SingleUint64 = 4uL
          SingleSint32 = 5
          SingleSint64 = 6L
          SingleFixed32 = 7u
          SingleFixed64 = 8uL
          SingleSfixed32 = 9
          SingleSfixed64 = 10L
          SingleFloat = 11.0f
          SingleDouble = 12.0
          SingleBool = true
          SingleString = "Yo"
          SingleBytes = ByteString.FromBase64("eW8=")
          SingleNestedMessage = ValueSome(nestedMessage)
          SingleForeignMessage = ValueSome(foreignMessage)
          SingleImportMessage = ValueSome(importMessage)
          SingleNestedEnum = FSharp.TestAllTypes.Types.NestedEnum.Foo
          SingleForeignEnum = FSharp.ForeignEnum.ForeignBaz
          SingleImportEnum = FSharp.ImportEnum.ImportBaz
          SinglePublicImportMessage = ValueSome(publicImportMessage)
          RepeatedInt32 = repeatedField [ 1; 2; 3 ]
          RepeatedInt64 = repeatedField [ 4L; 5L; 6L ]
          RepeatedUint32 = repeatedField [ 7u; 8u; 9u ]
          RepeatedUint64 = repeatedField [ 10uL; 11uL; 12uL ]
          RepeatedSint32 = repeatedField [ 13; 14; 15 ]
          RepeatedSint64 = repeatedField [ 16L; 17L; 18L ]
          RepeatedFixed32 = repeatedField [ 19u; 20u; 21u ]
          RepeatedFixed64 = repeatedField [ 22uL; 23uL; 24uL ]
          RepeatedSfixed32 = repeatedField [ 25; 26; 27 ]
          RepeatedSfixed64 = repeatedField [ 28L; 29L; 30L ]
          RepeatedFloat = repeatedField [ 31.0f; 32.0f; 33.0f ]
          RepeatedDouble = repeatedField [ 34.0; 35.0; 36.0 ]
          RepeatedBool = repeatedField [ true; false; true ]
          RepeatedString = repeatedField [ "Yo"; "Yo"; "Yo" ]
          RepeatedBytes =
              repeatedField [ ByteString.FromBase64("eW8=")
                              ByteString.FromBase64("eW8=")
                              ByteString.FromBase64("eW8=") ]
          RepeatedNestedMessage =
              repeatedField [ nestedMessage
                              nestedMessage
                              nestedMessage ]
          RepeatedForeignMessage =
              repeatedField [ foreignMessage
                              foreignMessage
                              foreignMessage ]
          RepeatedImportMessage =
              repeatedField [ importMessage
                              importMessage
                              importMessage ]
          RepeatedNestedEnum =
              repeatedField [ FSharp.TestAllTypes.Types.NestedEnum.Foo
                              FSharp.TestAllTypes.Types.NestedEnum.Foo
                              FSharp.TestAllTypes.Types.NestedEnum.Foo ]
          RepeatedForeignEnum =
              repeatedField [ FSharp.ForeignEnum.ForeignBaz
                              FSharp.ForeignEnum.ForeignBaz
                              FSharp.ForeignEnum.ForeignBaz ]
          RepeatedImportEnum =
              repeatedField [ FSharp.ImportEnum.ImportBaz
                              FSharp.ImportEnum.ImportBaz
                              FSharp.ImportEnum.ImportBaz ]
          RepeatedPublicImportMessage =
              repeatedField [ publicImportMessage
                              publicImportMessage
                              publicImportMessage ]
          OneofField = ValueSome(TestAllTypes.Types.OneofUint32(777u)) }

    let fsharpBytes = fsharp.ToByteArray()

    let csharp =
        CSharp.TestAllTypes.Parser.ParseFrom(fsharpBytes)

    Assert.Equal(fsharp.SingleInt32, csharp.SingleInt32)
    Assert.Equal(fsharp.SingleInt64, csharp.SingleInt64)
    Assert.Equal(fsharp.SingleUint32, csharp.SingleUint32)
    Assert.Equal(fsharp.SingleUint64, csharp.SingleUint64)
    Assert.Equal(fsharp.SingleSint32, csharp.SingleSint32)
    Assert.Equal(fsharp.SingleSint64, csharp.SingleSint64)
    Assert.Equal(fsharp.SingleFixed32, csharp.SingleFixed32)
    Assert.Equal(fsharp.SingleFixed64, csharp.SingleFixed64)
    Assert.Equal(fsharp.SingleSfixed32, csharp.SingleSfixed32)
    Assert.Equal(fsharp.SingleSfixed64, csharp.SingleSfixed64)
    Assert.Equal(fsharp.SingleFloat, csharp.SingleFloat)
    Assert.Equal(fsharp.SingleDouble, csharp.SingleDouble)
    Assert.Equal(fsharp.SingleBool, csharp.SingleBool)
    Assert.Equal(fsharp.SingleString, csharp.SingleString)
    Assert.Equal(fsharp.SingleBytes.ToString(), csharp.SingleBytes.ToString())
    Assert.Equal(fsharp.SingleNestedMessage.Value.Bb, csharp.SingleNestedMessage.Bb)
    Assert.Equal(fsharp.SingleForeignMessage.Value.C, csharp.SingleForeignMessage.C)
    Assert.Equal(fsharp.SingleImportMessage.Value.D, csharp.SingleImportMessage.D)
    Assert.Equal(int fsharp.SingleNestedEnum, int csharp.SingleNestedEnum)
    Assert.Equal(int fsharp.SingleForeignEnum, int csharp.SingleForeignEnum)
    Assert.Equal(int fsharp.SingleImportEnum, int csharp.SingleImportEnum)
    Assert.Equal(fsharp.SinglePublicImportMessage.Value.E, csharp.SinglePublicImportMessage.E)
    Assert.Equal(fsharp.RepeatedInt32 :> IEnumerable, csharp.RepeatedInt32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedInt64 :> IEnumerable, csharp.RepeatedInt64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint32 :> IEnumerable, csharp.RepeatedUint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint64 :> IEnumerable, csharp.RepeatedUint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint32 :> IEnumerable, csharp.RepeatedSint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint64 :> IEnumerable, csharp.RepeatedSint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed32 :> IEnumerable, csharp.RepeatedFixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed64 :> IEnumerable, csharp.RepeatedFixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed32 :> IEnumerable, csharp.RepeatedSfixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed64 :> IEnumerable, csharp.RepeatedSfixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFloat :> IEnumerable, csharp.RepeatedFloat :> IEnumerable)
    Assert.Equal(fsharp.RepeatedDouble :> IEnumerable, csharp.RepeatedDouble :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBool :> IEnumerable, csharp.RepeatedBool :> IEnumerable)
    Assert.Equal(fsharp.RepeatedString :> IEnumerable, csharp.RepeatedString :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBytes :> IEnumerable, csharp.RepeatedBytes :> IEnumerable)
    Assert.Equal(fsharp.RepeatedNestedMessage.[0].Bb, csharp.RepeatedNestedMessage.[0].Bb)
    Assert.Equal(fsharp.RepeatedNestedMessage.[1].Bb, csharp.RepeatedNestedMessage.[1].Bb)
    Assert.Equal(fsharp.RepeatedNestedMessage.[2].Bb, csharp.RepeatedNestedMessage.[2].Bb)
    Assert.Equal(fsharp.RepeatedForeignMessage.[0].C, csharp.RepeatedForeignMessage.[0].C)
    Assert.Equal(fsharp.RepeatedForeignMessage.[1].C, csharp.RepeatedForeignMessage.[1].C)
    Assert.Equal(fsharp.RepeatedForeignMessage.[2].C, csharp.RepeatedForeignMessage.[2].C)
    Assert.Equal(fsharp.RepeatedImportMessage.[0].D, csharp.RepeatedImportMessage.[0].D)
    Assert.Equal(fsharp.RepeatedImportMessage.[1].D, csharp.RepeatedImportMessage.[1].D)
    Assert.Equal(fsharp.RepeatedImportMessage.[2].D, csharp.RepeatedImportMessage.[2].D)
    Assert.Equal(int fsharp.RepeatedNestedEnum.[0], int csharp.RepeatedNestedEnum.[0])
    Assert.Equal(int fsharp.RepeatedNestedEnum.[1], int csharp.RepeatedNestedEnum.[1])
    Assert.Equal(int fsharp.RepeatedNestedEnum.[2], int csharp.RepeatedNestedEnum.[2])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[0], int csharp.RepeatedForeignEnum.[0])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[1], int csharp.RepeatedForeignEnum.[1])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[2], int csharp.RepeatedForeignEnum.[2])
    Assert.Equal(int fsharp.RepeatedImportEnum.[0], int csharp.RepeatedImportEnum.[0])
    Assert.Equal(int fsharp.RepeatedImportEnum.[1], int csharp.RepeatedImportEnum.[1])
    Assert.Equal(int fsharp.RepeatedImportEnum.[2], int csharp.RepeatedImportEnum.[2])
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[0].E, csharp.RepeatedPublicImportMessage.[0].E)
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[1].E, csharp.RepeatedPublicImportMessage.[1].E)
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[2].E, csharp.RepeatedPublicImportMessage.[2].E)
    Assert.Equal(777u, csharp.OneofUint32)

[<Fact>]
let ``Proto3 optional fields should deserialize correctly and be consumable from csharp`` () =
    let nestedMessage : CSharp.TestAllTypes.Types.NestedMessage =
        CSharp.TestAllTypes.Types.NestedMessage(Bb = 1)

    let foreignMessage : CSharp.ForeignMessage = CSharp.ForeignMessage(C = 2)

    let importMessage : CSharp.ImportMessage = CSharp.ImportMessage(D = 3)

    let publicImportMessage : CSharp.PublicImportMessage = CSharp.PublicImportMessage(E = 4)

    let csharp : CSharp.TestAllTypes =
        CSharp.TestAllTypes(
            SingleInt32 = 1,
            SingleInt64 = 2L,
            SingleUint32 = 3u,
            SingleUint64 = 4uL,
            SingleSint32 = -5,
            SingleSint64 = -6L,
            SingleFixed32 = 7u,
            SingleFixed64 = 8uL,
            SingleSfixed32 = -9,
            SingleSfixed64 = -10L,
            SingleFloat = 11f,
            SingleDouble = 12.0,
            SingleBool = true,
            SingleString = "13",
            SingleBytes = ByteString.FromBase64("eW8="),
            SingleNestedMessage = nestedMessage,
            SingleForeignMessage = foreignMessage,
            SingleImportMessage = importMessage,
            SinglePublicImportMessage = publicImportMessage,
            SingleNestedEnum = CSharp.TestAllTypes.Types.NestedEnum.Foo,
            SingleForeignEnum = CSharp.ForeignEnum.ForeignFoo,
            SingleImportEnum = CSharp.ImportEnum.ImportFoo)
    csharp.RepeatedInt32.AddRange([1;2;3])
    csharp.RepeatedInt64.AddRange([1L;2L;3L])
    csharp.RepeatedUint32.AddRange([1u;2u;3u])
    csharp.RepeatedUint64.AddRange([1uL;2uL;3uL])
    csharp.RepeatedSint32.AddRange([-1;-2;-3])
    csharp.RepeatedSint64.AddRange([-1L;-2L;-3L])
    csharp.RepeatedFixed32.AddRange([1u;2u;3u])
    csharp.RepeatedFixed64.AddRange([1uL;2uL;3uL])
    csharp.RepeatedSfixed32.AddRange([-1;-2;-3])
    csharp.RepeatedSfixed64.AddRange([-1L;-2L;-3L])
    csharp.RepeatedFloat.AddRange([1f;2f;3f])
    csharp.RepeatedDouble.AddRange([1.0;2.0;3.0])
    csharp.RepeatedBool.AddRange([true;false;true])
    csharp.RepeatedString.AddRange(["1";"2";"3"])
    csharp.RepeatedBytes.AddRange([ByteString.FromBase64("dGhpcyBpcyBzb21lIHRlc3Q=");ByteString.FromBase64("dGhpcyBpcyBzb21lIHRlc3Q=");ByteString.FromBase64("dGhpcyBpcyBzb21lIHRlc3Q=")])
    csharp.RepeatedNestedMessage.Add([nestedMessage; nestedMessage; nestedMessage])
    csharp.RepeatedForeignMessage.Add([foreignMessage; foreignMessage; foreignMessage])
    csharp.RepeatedImportMessage.Add([importMessage; importMessage; importMessage])
    csharp.RepeatedPublicImportMessage.Add([publicImportMessage; publicImportMessage; publicImportMessage])
    csharp.RepeatedNestedEnum.AddRange([CSharp.TestAllTypes.Types.NestedEnum.Foo;CSharp.TestAllTypes.Types.NestedEnum.Foo;CSharp.TestAllTypes.Types.NestedEnum.Foo])
    csharp.RepeatedForeignEnum.AddRange([CSharp.ForeignEnum.ForeignFoo;CSharp.ForeignEnum.ForeignFoo;CSharp.ForeignEnum.ForeignFoo])
    csharp.RepeatedImportEnum.AddRange([CSharp.ImportEnum.ImportFoo;CSharp.ImportEnum.ImportFoo;CSharp.ImportEnum.ImportFoo])
    csharp.OneofUint32 <- 777u

    let csharpBytes = csharp.ToByteArray()

    let fsharp =
        FSharp.TestAllTypes.Parser.ParseFrom(csharpBytes)

    Assert.Equal(fsharp.SingleInt32, csharp.SingleInt32)
    Assert.Equal(fsharp.SingleInt64, csharp.SingleInt64)
    Assert.Equal(fsharp.SingleUint32, csharp.SingleUint32)
    Assert.Equal(fsharp.SingleUint64, csharp.SingleUint64)
    Assert.Equal(fsharp.SingleSint32, csharp.SingleSint32)
    Assert.Equal(fsharp.SingleSint64, csharp.SingleSint64)
    Assert.Equal(fsharp.SingleFixed32, csharp.SingleFixed32)
    Assert.Equal(fsharp.SingleFixed64, csharp.SingleFixed64)
    Assert.Equal(fsharp.SingleSfixed32, csharp.SingleSfixed32)
    Assert.Equal(fsharp.SingleSfixed64, csharp.SingleSfixed64)
    Assert.Equal(fsharp.SingleFloat, csharp.SingleFloat)
    Assert.Equal(fsharp.SingleDouble, csharp.SingleDouble)
    Assert.Equal(fsharp.SingleBool, csharp.SingleBool)
    Assert.Equal(fsharp.SingleString, csharp.SingleString)
    Assert.Equal(fsharp.SingleBytes.ToString(), csharp.SingleBytes.ToString())
    Assert.Equal(fsharp.SingleNestedMessage.Value.Bb, csharp.SingleNestedMessage.Bb)
    Assert.Equal(fsharp.SingleForeignMessage.Value.C, csharp.SingleForeignMessage.C)
    Assert.Equal(fsharp.SingleImportMessage.Value.D, csharp.SingleImportMessage.D)
    Assert.Equal(int fsharp.SingleNestedEnum, int csharp.SingleNestedEnum)
    Assert.Equal(int fsharp.SingleForeignEnum, int csharp.SingleForeignEnum)
    Assert.Equal(int fsharp.SingleImportEnum, int csharp.SingleImportEnum)
    Assert.Equal(fsharp.SinglePublicImportMessage.Value.E, csharp.SinglePublicImportMessage.E)
    Assert.Equal(fsharp.RepeatedInt32 :> IEnumerable, csharp.RepeatedInt32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedInt64 :> IEnumerable, csharp.RepeatedInt64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint32 :> IEnumerable, csharp.RepeatedUint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint64 :> IEnumerable, csharp.RepeatedUint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint32 :> IEnumerable, csharp.RepeatedSint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint64 :> IEnumerable, csharp.RepeatedSint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed32 :> IEnumerable, csharp.RepeatedFixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed64 :> IEnumerable, csharp.RepeatedFixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed32 :> IEnumerable, csharp.RepeatedSfixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed64 :> IEnumerable, csharp.RepeatedSfixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFloat :> IEnumerable, csharp.RepeatedFloat :> IEnumerable)
    Assert.Equal(fsharp.RepeatedDouble :> IEnumerable, csharp.RepeatedDouble :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBool :> IEnumerable, csharp.RepeatedBool :> IEnumerable)
    Assert.Equal(fsharp.RepeatedString :> IEnumerable, csharp.RepeatedString :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBytes :> IEnumerable, csharp.RepeatedBytes :> IEnumerable)
    Assert.Equal(fsharp.RepeatedNestedMessage.[0].Bb, csharp.RepeatedNestedMessage.[0].Bb)
    Assert.Equal(fsharp.RepeatedNestedMessage.[1].Bb, csharp.RepeatedNestedMessage.[1].Bb)
    Assert.Equal(fsharp.RepeatedNestedMessage.[2].Bb, csharp.RepeatedNestedMessage.[2].Bb)
    Assert.Equal(fsharp.RepeatedForeignMessage.[0].C, csharp.RepeatedForeignMessage.[0].C)
    Assert.Equal(fsharp.RepeatedForeignMessage.[1].C, csharp.RepeatedForeignMessage.[1].C)
    Assert.Equal(fsharp.RepeatedForeignMessage.[2].C, csharp.RepeatedForeignMessage.[2].C)
    Assert.Equal(fsharp.RepeatedImportMessage.[0].D, csharp.RepeatedImportMessage.[0].D)
    Assert.Equal(fsharp.RepeatedImportMessage.[1].D, csharp.RepeatedImportMessage.[1].D)
    Assert.Equal(fsharp.RepeatedImportMessage.[2].D, csharp.RepeatedImportMessage.[2].D)
    Assert.Equal(int fsharp.RepeatedNestedEnum.[0], int csharp.RepeatedNestedEnum.[0])
    Assert.Equal(int fsharp.RepeatedNestedEnum.[1], int csharp.RepeatedNestedEnum.[1])
    Assert.Equal(int fsharp.RepeatedNestedEnum.[2], int csharp.RepeatedNestedEnum.[2])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[0], int csharp.RepeatedForeignEnum.[0])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[1], int csharp.RepeatedForeignEnum.[1])
    Assert.Equal(int fsharp.RepeatedForeignEnum.[2], int csharp.RepeatedForeignEnum.[2])
    Assert.Equal(int fsharp.RepeatedImportEnum.[0], int csharp.RepeatedImportEnum.[0])
    Assert.Equal(int fsharp.RepeatedImportEnum.[1], int csharp.RepeatedImportEnum.[1])
    Assert.Equal(int fsharp.RepeatedImportEnum.[2], int csharp.RepeatedImportEnum.[2])
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[0].E, csharp.RepeatedPublicImportMessage.[0].E)
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[1].E, csharp.RepeatedPublicImportMessage.[1].E)
    Assert.Equal(fsharp.RepeatedPublicImportMessage.[2].E, csharp.RepeatedPublicImportMessage.[2].E)
    Assert.Equal(777u, csharp.OneofUint32)

[<Fact>]
let ``FSharp default message should contain same values as csharp default message`` =
    let fsharp = FSharp.TestAllTypes.empty()
    let csharp = CSharp.TestAllTypes()
    
    Assert.Equal(fsharp.SingleInt32, csharp.SingleInt32)
    Assert.Equal(fsharp.SingleInt64, csharp.SingleInt64)
    Assert.Equal(fsharp.SingleUint32, csharp.SingleUint32)
    Assert.Equal(fsharp.SingleUint64, csharp.SingleUint64)
    Assert.Equal(fsharp.SingleSint32, csharp.SingleSint32)
    Assert.Equal(fsharp.SingleSint64, csharp.SingleSint64)
    Assert.Equal(fsharp.SingleFixed32, csharp.SingleFixed32)
    Assert.Equal(fsharp.SingleFixed64, csharp.SingleFixed64)
    Assert.Equal(fsharp.SingleSfixed32, csharp.SingleSfixed32)
    Assert.Equal(fsharp.SingleSfixed64, csharp.SingleSfixed64)
    Assert.Equal(fsharp.SingleFloat, csharp.SingleFloat)
    Assert.Equal(fsharp.SingleDouble, csharp.SingleDouble)
    Assert.Equal(fsharp.SingleBool, csharp.SingleBool)
    Assert.Equal(fsharp.SingleString, csharp.SingleString)
    Assert.Equal(fsharp.SingleBytes.ToString(), csharp.SingleBytes.ToString())
    Assert.Equal(fsharp.SingleNestedMessage, ValueNone)
    Assert.Equal(fsharp.SingleForeignMessage, ValueNone)
    Assert.Equal(fsharp.SingleImportMessage, ValueNone)
    Assert.Equal(int fsharp.SingleNestedEnum, int csharp.SingleNestedEnum)
    Assert.Equal(int fsharp.SingleForeignEnum, int csharp.SingleForeignEnum)
    Assert.Equal(int fsharp.SingleImportEnum, int csharp.SingleImportEnum)
    Assert.Equal(fsharp.SinglePublicImportMessage.Value.E, csharp.SinglePublicImportMessage.E)
    Assert.Equal(fsharp.RepeatedInt32 :> IEnumerable, csharp.RepeatedInt32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedInt64 :> IEnumerable, csharp.RepeatedInt64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint32 :> IEnumerable, csharp.RepeatedUint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedUint64 :> IEnumerable, csharp.RepeatedUint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint32 :> IEnumerable, csharp.RepeatedSint32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSint64 :> IEnumerable, csharp.RepeatedSint64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed32 :> IEnumerable, csharp.RepeatedFixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFixed64 :> IEnumerable, csharp.RepeatedFixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed32 :> IEnumerable, csharp.RepeatedSfixed32 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedSfixed64 :> IEnumerable, csharp.RepeatedSfixed64 :> IEnumerable)
    Assert.Equal(fsharp.RepeatedFloat :> IEnumerable, csharp.RepeatedFloat :> IEnumerable)
    Assert.Equal(fsharp.RepeatedDouble :> IEnumerable, csharp.RepeatedDouble :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBool :> IEnumerable, csharp.RepeatedBool :> IEnumerable)
    Assert.Equal(fsharp.RepeatedString :> IEnumerable, csharp.RepeatedString :> IEnumerable)
    Assert.Equal(fsharp.RepeatedBytes :> IEnumerable, csharp.RepeatedBytes :> IEnumerable)
    Assert.Equal(fsharp.RepeatedNestedMessage.Count, csharp.RepeatedNestedMessage.Count)
    Assert.Equal(fsharp.RepeatedForeignMessage.Count, csharp.RepeatedForeignMessage.Count)
    Assert.Equal(fsharp.RepeatedImportMessage.Count, csharp.RepeatedImportMessage.Count)
    Assert.Equal(fsharp.RepeatedNestedEnum.Count, csharp.RepeatedNestedEnum.Count)
    Assert.Equal(fsharp.RepeatedForeignEnum.Count, csharp.RepeatedForeignEnum.Count)
    Assert.Equal(fsharp.RepeatedImportEnum.Count, csharp.RepeatedImportEnum.Count)
    Assert.Equal(fsharp.RepeatedPublicImportMessage.Count, csharp.RepeatedPublicImportMessage.Count)
    Assert.Equal(fsharp.OneofField, ValueNone)
    