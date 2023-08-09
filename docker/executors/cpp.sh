#!/bin/bash

g++ -o binary -x c++ $1

inputs=$2
IFS=':'

read -a inputsArray <<< "$inputs"

for val in "${inputsArray[@]}";
do
    echo -n "TestCase:"
    echo $val | ./binary 
done
