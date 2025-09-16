@echo off
echo Starting AgroScan API...

REM Navigate to the API project directory
cd src\AgroScan.API

REM Build the project
echo Building the project...
dotnet build

REM Check if build was successful
if %errorlevel% equ 0 (
    echo Build successful! Starting the API...
    echo API will be available at:
    echo   - HTTP: http://localhost:5002
    echo   - Swagger UI: http://localhost:5002
    echo.
    echo Press Ctrl+C to stop the server
    echo.
    
    REM Run the API
    dotnet run
) else (
    echo Build failed! Please check the errors above.
    pause
    exit /b 1
)
