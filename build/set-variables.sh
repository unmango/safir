#!/bin/bash
SOURCE="origin/$SYSTEM_PULLREQUEST_SOURCEBRANCH"
TARGET="origin/$SYSTEM_PULLREQUEST_TARGETBRANCH"
PROJECTPATH="$ROOTPATH/FileManager"

echo $SOURCE
echo $TARGET
echo $PROJECTPATH
GITPATH=$(which git)
echo $GITPATH
$GITPATH diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH
$($GITPATH diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH)
echo $($GITPATH diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH)
TMP=$($GITPATH diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH)
echo $TMP
if [[ $($GITPATH diff-tree --dirstat $SOURCE..$TARGET -- $PROJECTPATH) ]]; then
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]true"
  echo "##vso[task.setvariable variable=FileManagerPath;isOutput=true]$PROJECTPATH"
  echo "True"
else
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]false"
  echo "False"
fi
