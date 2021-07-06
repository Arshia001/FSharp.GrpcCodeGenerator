#! /usr/bin/env bash

set -eoux pipefail

function cleanup {
    chmod -x ./Protoc/linux_x64/protoc
    find ./test-protos -type f -name "*.proto" -print0 | xargs -0 sed -i 's/.*csharp_namespace.*/option csharp_namespace = "REPLACE_ME";/' 
}
trap cleanup EXIT

dotnet build ./FSharp.GrpcCodeGenerator/FSharp.GrpcCodeGenerator.fsproj

find ./test-protos -type f -name "*.proto" -print0 | xargs -0 sed -i 's/.*csharp_namespace.*/option csharp_namespace = "FSharp.GrpcCodeGenerator.TestProtos.FSharp";/' 

chmod +x ./Protoc/linux_x64/protoc && \
./Protoc/linux_x64/protoc \
--plugin=protoc-gen-fsharp=./FSharp.GrpcCodeGenerator/bin/Debug/net5.0/FSharp.GrpcCodeGenerator \
--fsharp_out=./FSharp.GrpcCodeGenerator.TestProtos.FSharp \
-I ./test-protos \
-I ./Proto \
./test-protos/map_unittest_proto3.proto \
./test-protos/unittest_custom_options_proto3.proto \
./test-protos/unittest_import_proto3.proto \
./test-protos/unittest_import_public_proto3.proto \
./test-protos/unittest_proto3_optional.proto \
./test-protos/unittest_proto3.proto \
./test-protos/well_known_protos.proto \
./test-protos/google/api/annotations.proto \
./test-protos/google/api/http.proto \

find ./test-protos -type f -name "*.proto" -print0 | xargs -0 sed -i 's/.*csharp_namespace.*/option csharp_namespace = "FSharp.GrpcCodeGenerator.TestProtos.CSharp";/' 

chmod +x ./Protoc/linux_x64/protoc && \
./Protoc/linux_x64/protoc \
--csharp_out=./FSharp.GrpcCodeGenerator.TestProtos.CSharp \
-I ./test-protos \
-I ./Proto \
./test-protos/map_unittest_proto3.proto \
./test-protos/unittest_custom_options_proto3.proto \
./test-protos/unittest_import_proto3.proto \
./test-protos/unittest_import_public_proto3.proto \
./test-protos/unittest_proto3_optional.proto \
./test-protos/unittest_proto3.proto \
./test-protos/well_known_protos.proto

# We skip these protos for C# since we use the Google.Api.CommonProtos NuGet package instead
#./test-protos/google/api/annotations.proto \
#./test-protos/google/api/http.proto

dotnet build ./FSharp.GrpcCodeGenerator.TestProtos.FSharp/FSharp.GrpcCodeGenerator.TestProtos.FSharp.fsproj
dotnet build ./FSharp.GrpcCodeGenerator.TestProtos.CSharp/FSharp.GrpcCodeGenerator.TestProtos.CSharp.csproj
dotnet test ./FSharp.GrpcCodeGenerator.Tests/FSharp.GrpcCodeGenerator.Tests.fsproj