#!/bin/bash
SOURCE="origin/$SYSTEM_PULLREQUEST_SOURCEBRANCH"
TARGET="origin/$SYSTEM_PULLREQUEST_TARGETBRANCH"
PROJECTPATH="$ROOTPATH/FileManager"

echo $SOURCE
echo $TARGET
echo $PROJECTPATH
if [[ $(git diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH) ]]; then
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]true"
  echo "##vso[task.setvariable variable=FileManagerPath;isOutput=true]$PROJECTPATH"
fi
