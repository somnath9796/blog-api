# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Publish the application to the /out directory
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# 👇 Fix: Ensure the Images directory exists in container
RUN mkdir /app/Images

# Copy the published output from build stage
COPY --from=build /app/out .

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "CodePulse.API.dll"]
