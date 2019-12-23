#!/bin/bash
echo $Source
echo $Target
echo $ProjectPath
GITPATH=$(which git)
echo $GITPATH
$GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath
$($GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath)
echo $($GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath)
echo "$GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath"
TMP=$($GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath)
echo $TMP
if [[ $($GITPATH diff-tree --dirstat $Source..$Target -- $ProjectPath) ]]; then
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]true"
  echo "True"
else
  echo "##vso[task.setvariable variable=BuildFileManager;isOutput=true]false"
  echo "False"
fi
