#!/bin/bash
source "$(dirname "$0")/input_processing.sh"
g++ -o binary -x c++ $1

inputs=${@:2}

processInput "$inputs"

for val in "${INPUT_ARRAY[@]}";
do
    echo -n $DELIMITER
    echo -e $val | ./binary 
done