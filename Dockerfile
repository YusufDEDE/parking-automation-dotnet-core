FROM microsoft/dotnet:3.1-sdk AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY ../
RUN dotnet publish -c Release -o out

FORM microsoft/dotnet:3.1-aspnetcore-runtime
WORKDIR /app
COPY --from=buil-env /app/out

CMD ASPNETCORE_URLS=http://*:$PORT dotnet parking-automation-dotnet-core.dll