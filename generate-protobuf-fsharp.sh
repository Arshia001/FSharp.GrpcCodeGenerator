#! /usr/bin/env bash

dotnet build ./FSharp.GrpcCodeGenerator/FSharp.GrpcCodeGenerator.fsproj

chmod +x ./Protoc/linux_x64/protoc && \
./Protoc/linux_x64/protoc \
--plugin=protoc-gen-fsharp=./FSharp.GrpcCodeGenerator/bin/Debug/net5.0/FSharp.GrpcCodeGenerator \
--fsharp_out=./Protobuf.FSharp/BuiltinTypes \
-I ./Proto \
./Proto/google/protobuf/any.proto \
./Proto/google/protobuf/api.proto \
./Proto/google/protobuf/descriptor.proto \
./Proto/google/protobuf/duration.proto \
./Proto/google/protobuf/empty.proto \
./Proto/google/protobuf/field_mask.proto \
./Proto/google/protobuf/source_context.proto \
./Proto/google/protobuf/struct.proto \
./Proto/google/protobuf/timestamp.proto \
./Proto/google/protobuf/type.proto \
./Proto/google/protobuf/wrappers.proto