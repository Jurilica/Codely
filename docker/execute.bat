@echo off

set abs=%1
set params=%~2

for %%a in (%abs%) do (
    set file=%%~nxa
    set extension=%%~xa
)

if "%extension%" == ".c" set executor="/c_executor"
if "%extension%" == ".cpp" set executor="/cpp_executor"
if "%extension%" == ".py" set executor="/python_executor"
if "%extension%" == ".js" set executor="/js_executor"
if "%extension%" == ".java" set executor="/java_executor"

set scriptDirectory=%~dp0

docker run^
    -m 128m^
    --cpus=".5"^
    --rm ^
    --log-driver none^
    -v %abs%:/%file%^
    -v "%scriptDirectory%executors\input_processing.sh:/input_processing.sh"^
    -v "%scriptDirectory%executors\c.sh:/c_executor"^
    -v "%scriptDirectory%executors\cpp.sh:/cpp_executor"^
    -v "%scriptDirectory%executors\python.sh:/python_executor"^
    -v "%scriptDirectory%executors\js.sh:/js_executor"^
    -v "%scriptDirectory%executors\java.sh:/java_executor"^
    rce runuser^
        -l runner -c "%executor% /%file% %params% 2>&1"

del %abs%