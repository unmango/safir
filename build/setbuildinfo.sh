#!/bin/bash

# https://github.com/dotnet/core-sdk/blob/master/eng/setbuildinfo.sh

shopt -s nocasematch

if [[ "$AdditionalBuildParameters" == '$(_AdditionalBuildParameters)' ]]
then
    echo "##vso[task.setvariable variable=AdditionalBuildParameters]"
fi
