# Path to the solution file
$solutionPath = "C:\Users\Breez\source\repos\FactoryShapes\FactoryShapes.sln"

# Path to MSBuild (change if necessary depending on your Visual Studio version)
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"

# Output directory
$outputPath = "C:\Users\Breez\source\repos\FactoryShapes\Shapes\bin\Debug\net8.0-windows"

# Build Configuration (Debug/Release)
$configuration = "Debug"

# Clean the solution (Optional step)
Write-Host "Cleaning the solution..."
& $msbuildPath $solutionPath /t:Clean

# Build the solution
Write-Host "Building solution $solutionPath with configuration $configuration..."
$buildProcess = Start-Process -FilePath $msbuildPath -ArgumentList "$solutionPath /p:Configuration=$configuration /p:OutDir=$outputPath" -PassThru -Wait

# Check if the build succeeded
if ($buildProcess.ExitCode -eq 0) 
{
    Write-Host "Build completed successfully."
} 
else 
{
    Write-Host "Build failed with exit code $($buildProcess.ExitCode)."
}
