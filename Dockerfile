FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY RazorInterviewDemo/RazorInterviewDemo.csproj RazorInterviewDemo/
RUN dotnet restore RazorInterviewDemo/RazorInterviewDemo.csproj

COPY RazorInterviewDemo/ RazorInterviewDemo/
RUN dotnet publish RazorInterviewDemo/RazorInterviewDemo.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RazorInterviewDemo.dll"]
