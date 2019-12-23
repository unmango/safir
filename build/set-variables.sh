#!/bin/bash
SOURCE=$SYSTEM_PULLREQUEST_SOURCEBRANCH
TARGET=$SYSTEM_PULLREQUEST_TARGETBRANCH
PROJECTPATH="$ROOTPATH/UI"

echo $SOURCE
echo $TARGET
echo $PROJECTPATH
if [[ $(git diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH) ]]; then
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]true"
  echo "##vso[task.setvariable variable=FileManagerPath;isOutput=true]$PROJECTPATH"
fi
