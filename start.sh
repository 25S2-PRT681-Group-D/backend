#!/bin/bash

echo "Starting AgroScan API..."

# Navigate to the API project directory
cd src/AgroScan.API

# Build the project
echo "Building the project..."
dotnet build

# Check if build was successful
if [ $? -eq 0 ]; then
    echo "Build successful! Starting the API..."
    echo "API will be available at:"
    echo "  - HTTP: http://localhost:5002"
    echo "  - Swagger UI: http://localhost:5002"
    echo ""
    echo "Press Ctrl+C to stop the server"
    echo ""
    
    # Run the API
    dotnet run
else
    echo "Build failed! Please check the errors above."
    exit 1
fi
