#!/bin/bash
source "$(dirname "$0")/input_processing.sh"
timeout -s KILL 10 gcc -o binary $1

inputs=${@:2}

processInput "$inputs"

for val in "${INPUT_ARRAY[@]}";
do
    echo -n $DELIMITER
    timeout -s KILL 3 echo -e $val | ./binary
done