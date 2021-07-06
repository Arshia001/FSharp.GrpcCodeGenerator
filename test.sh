#! /usr/bin/env bash

set -eoux pipefail

dotnet build ./FSharp.GrpcCodeGenerator/FSharp.GrpcCodeGenerator.fsproj

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
./test-protos/unittest_proto3.proto

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
./test-protos/unittest_proto3.proto

dotnet build ./FSharp.GrpcCodeGenerator.TestProtos.FSharp/FSharp.GrpcCodeGenerator.TestProtos.FSharp.fsproj
dotnet build ./FSharp.GrpcCodeGenerator.TestProtos.CSharp/FSharp.GrpcCodeGenerator.TestProtos.CSharp.csproj
