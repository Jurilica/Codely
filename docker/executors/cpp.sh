#!/bin/bash
source "$(dirname "$0")/input_processing.sh"
g++ -o binary -x c++ $1

inputs=$2

inputsArray=($(processInput "$inputs"))

delimiter=$(getDelimiter)

for val in "${inputsArray[@]}";
do
    echo -n $delimiter
    echo -e $val | ./binary 
done