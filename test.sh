#! /usr/bin/env bash

set -eoux pipefail

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
    find ./test-protos -type f -name "*.proto" -print0 | xargs -0 sed -i 's/.*csharp_namespace.*/option csharp_namespace = "REPLACE_ME";/' 
}
trap cleanup EXIT

dotnet build ./FSharp.GrpcCodeGenerator/FSharp.GrpcCodeGenerator.fsproj

find ./test-protos -type f -name "*.proto" -print0 | xargs -0 sed -i 's/.*csharp_namespace.*/option csharp_namespace = "FSharp.GrpcCodeGenerator.TestProtos.FSharp";/' 

chmod +x $protoc_path && \
$protoc_path \
--plugin=protoc-gen-fsharp=./FSharp.GrpcCodeGenerator/bin/Debug/net6.0/$plugin_path \
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

chmod +x $protoc_path && \
$protoc_path \
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
