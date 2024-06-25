@echo off
REM Đặt các biến môi trường
setlocal enabledelayedexpansion
set SOLUTION_DIR=%~dp0
set PROJECT_DIR=%SOLUTION_DIR%Presentation
set PUBLISH_PATH=C:\inetpub\wwwroot\be-happy-milk

REM Điều hướng đến thư mục project
cd /d "%PROJECT_DIR%"

REM Đồng bộ code từ nhánh main trên Git
echo Fetching latest code from main branch...
git fetch origin
git reset --hard origin/main

REM Kiểm tra xem lệnh đồng bộ có thành công không
if %errorlevel% neq 0 (
    echo Failed to sync code from Git!
    exit /b %errorlevel%
)

REM Tắt IIS Server
echo Stopping IIS Server...
iisreset /stop

REM Kiểm tra xem lệnh dừng IIS có thành công không
if %errorlevel% neq 0 (
    echo Failed to stop IIS Server!
    exit /b %errorlevel%
)

REM Kiểm tra xem thư mục project có tồn tại không
if not exist "%PROJECT_DIR%" (
    echo Project directory does not exist: %PROJECT_DIR%
    exit /b 1
)

REM Publish ứng dụng .NET
echo Publishing .NET application...
dotnet publish "%PROJECT_DIR%" -c Release -o "%PUBLISH_PATH%"

REM Kiểm tra xem lệnh publish có thành công không
if %errorlevel% neq 0 (
    echo Publishing failed!
    REM Mở lại IIS Server nếu publish thất bại
    echo Starting IIS Server...
    iisreset /start
    exit /b %errorlevel%
)

REM Mở lại IIS Server sau khi publish thành công
echo Starting IIS Server...
iisreset /start

REM Kiểm tra xem lệnh mở lại IIS có thành công không
if %errorlevel% neq 0 (
    echo Failed to start IIS Server!
    exit /b %errorlevel%
)

echo Publishing completed successfully!
pause
