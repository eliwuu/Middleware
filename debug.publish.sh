#!/usr/bin/env bash

if [ -d debug/ ] 
then
    rm -rf debug/
fi

dotnet publish -c Debug --self-contained --runtime linux-x64 -o debug