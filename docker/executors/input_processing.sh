#!/bin/bash

DELIMITER="TestCase:"
INPUT_ARRAY=()

function processInput()
{
    local inputs=$1
    local inputsWithLineEndings="${inputs//%n/\\n}"

    local inputsString=$inputsWithLineEndings$DELIMITER

    INPUT_ARRAY=()
    while [[ $inputsString ]];
    do
        INPUT_ARRAY+=("${inputsString%%"$DELIMITER"*}")
        inputsString=${inputsString#*"$DELIMITER"}
    done
}
