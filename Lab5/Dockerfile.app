FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# ���� ���� ������������ ��� ������ ������� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Lab5.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

# ���� ���� ������������ ��� ���������� ������� ������, ������� ����� ���������� �� ��������� ����
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# ���� ���� ������������ � ������� ����� ��� ��� ������� �� VS � ������� ������ (�� ���������, ����� ������������ ������� �� ������������)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lab5.dll"]