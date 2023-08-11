#!/bin/bash
source "$(dirname "$0")/input_processing.sh"

fileName=$1
inputs=${@:2}

cp $fileName temp.java

name=$(cat temp.java | grep -Eo 'public\s+class\s+([A-Za-z0-9]+)' | sed -n 's/  */ /gp' | cut -d' ' -f3)
mv temp.java "$name.java"
timeout -s KILL 10 javac $name.java > /dev/null 2>&1

processInput "$inputs"

for val in "${INPUT_ARRAY[@]}";
do
    echo -n $DELIMITER
    timeout -s KILL 3 echo -e $val | java $name
done