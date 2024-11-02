# Этап сборки
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

# Копируем проект и выполняем dotnet restore
COPY --link "./FHW.csproj" .
RUN dotnet restore -a $TARGETARCH

# Копируем исходный код и выполняем публикацию
COPY --link . .
RUN dotnet publish "./FHW.csproj" --no-restore -a $TARGETARCH -o /app

# Этап финального образа
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
LABEL org.opencontainers.image.source="https://github.com/vse3211/factorio-helper-online"
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs tzdata
ENV TZ=Asia/Almaty
RUN apk add --no-cache curl

# Открытые порты
EXPOSE 8080
EXPOSE 8081

# Рабочая директория и копирование артефактов сборки
WORKDIR /app
COPY --link --from=build /app .

# Указываем пользователя и точку входа
USER $APP_UID
ENTRYPOINT ["dotnet", "FHW.dll"]
