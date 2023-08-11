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

docker run^
    -m 128m^
    --cpus=".5"^
    --rm ^
    --log-driver none^
    -v %abs%:/%file%^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\input_processing.sh:/input_processing.sh"^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\c.sh:/c_executor"^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\cpp.sh:/cpp_executor"^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\python.sh:/python_executor"^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\js.sh:/js_executor"^
    -v "C:\Users\Jurica Leljak\RiderProjects\Codely\docker\executors\java.sh:/java_executor"^
    rce runuser^
        -l runner -c "%executor% /%file% %params% 2>&1"

del %abs%