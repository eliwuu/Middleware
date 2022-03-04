#!/usr/bin/env bash

if [ -d release/ ] 
then
    rm -rf release/
fi

dotnet publish -c Release --self-contained --runtime linux-x64 -o release