#! /usr/bin/env bash

os=$(echo $OS | awk '{print tolower($0)}' | cut -c -7)
if [ "$os" == "windows" ]; then
        protoc_path='./Protoc/windows_x64/protoc.exe'
	plugin_path='FSharp.GrpcCodeGenerator.exe'
else
        protoc_path='./Protoc/linux_x64/protoc'
	plugin_path='FSharp.GrpcCodeGenerator'
fi

function cleanup {
    chmod -x $protoc_path
}
trap cleanup EXIT

dotnet build ./FSharp.GrpcCodeGenerator/FSharp.GrpcCodeGenerator.fsproj

chmod +x $protoc_path && \
$protoc_path \
--plugin=protoc-gen-fsharp=./FSharp.GrpcCodeGenerator/bin/Debug/net6.0/$plugin_path \
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
