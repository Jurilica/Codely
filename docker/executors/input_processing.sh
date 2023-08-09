#!/bin/bash

function getDelimiter()
{
    echo "TestCase:"
}

function processInput()
{
    local delimiter=$(getDelimiter)

    local inputs=$1
    local inpustWithLineEndigs="${inputs//!!!/\\n}"

    local inputsString=$inpustWithLineEndigs$delimiter

    inputsArray=()
    while [[ $inputsString ]];
    do
        inputsArray+=("${inputsString%%"$delimiter"*}")
        inputsString=${inputsString#*"$delimiter"}
    done

    echo "${inputsArray[@]}"
}
