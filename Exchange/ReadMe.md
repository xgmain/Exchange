# How to run the application

## run it on normal mode
- go to Exchange folder
- create or copy your test data to input.txt
- dotnet run --project Exchange input.txt output.txt

## run it on release mode
- go to Exchange folder
- dotnet publish -c Release -o ./publish
- go to .\publish\
- dotnet Exchange.dll ../input.txt ../output.txt or .\Exchange.exe ../input.txt ../output.txt

## where is log file
- should find in .\Exchange\Exchange\bin\Debug\net8.0\logs\

## how to run unit test
- go to Exchange folder then go to Exchange.Tests folder
- run dotnet test .\ExchangeTests.csproj