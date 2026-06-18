# Razor Weather Dashboard

A modern weather dashboard built with **ASP.NET Core 10** and **Razor Pages**. The app detects the user's location, fetches live forecast data from external APIs on the server, and renders a responsive dashboard with hourly and weekly views, temperature charts, and switchable color themes.

## Features

- **Live weather data** — current conditions, 24-hour hourly forecast, and 7-day outlook
- **Geolocation** — browser-based location detection with sensible fallbacks
- **Reverse geocoding** — resolves coordinates to a readable place name
- **Interactive dashboard** — today/week forecast tabs with paginated hourly list (7 items per page)
- **Temperature chart** — weekly min/average/max temperature visualization (SVG)
- **Theme switching** — four color themes (Dark, Light, Ocean, Sunset) via the settings control
- **Reusable UI** — Razor View Components for TopBar, forecast cards, charts, and icon buttons
- **Resilient API calls** — separate HTTP clients, retries, and extended timeouts for external services

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core 10 |
| UI | Razor Pages, View Components, Bootstrap 5 |
| Client | Vanilla JavaScript, CSS variables |
| APIs | [Open-Meteo](https://open-meteo.com/), [Nominatim](https://nominatim.org/) |
| Patterns | Dependency injection, `IHttpClientFactory`, server-side rendering |

## Architecture

The browser loads a single Razor Page (`/`). Weather data is fetched **server-side** during page render — external API calls do not originate from the browser.

```
Browser  →  GET /?latitude=…&longitude=…
Server   →  Open-Meteo (forecast) + Nominatim (location name)
Server   →  WeatherDashboardService builds view models
Server   →  Razor renders HTML with View Components
Browser  →  JS handles geolocation, themes, tabs, and pagination
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run locally

```bash
cd RazorInterviewDemo
dotnet run
```

Open [http://localhost:5078](http://localhost:5078). The app will request your location and load weather for your coordinates.

### Run with hot reload

```bash
cd RazorInterviewDemo
dotnet watch
```

### Build for production

```bash
dotnet publish RazorInterviewDemo/RazorInterviewDemo.csproj -c Release -o ./publish
```

## Project Structure

```
RazorInterviewDemo/
├── Pages/
│   ├── Weather/              # Main dashboard page
│   └── Shared/Components/    # View Component templates
├── ViewComponents/           # TopBar, ForecastCard, TemperatureChart, etc.
├── Models/Weather/           # View models and DTOs
├── Services/Weather/         # Dashboard service and Open-Meteo client
├── wwwroot/
│   ├── css/pages/weather.css # Theme variables and layout styles
│   └── js/pages/weather.js   # Geolocation, themes, tabs, pagination
└── Program.cs                # DI, HTTP clients, routing
```

## External APIs

| Service | Purpose |
|---------|---------|
| Open-Meteo Forecast API | Temperature, humidity, wind, weather codes |
| Nominatim (OpenStreetMap) | Reverse geocoding (coordinates → city/country) |

Both are free for non-commercial use. The server must have outbound HTTPS access in production.

## Deployment

This app ships with Docker and [Render](https://render.com) blueprint support.

### Option 1: Render (recommended)

1. Push this repo to GitHub.
2. Sign in to [Render](https://dashboard.render.com/).
3. Click **New +** → **Blueprint**.
4. Connect the `Razorpage_Demo_App` repository.
5. Render reads `render.yaml` and creates the web service.
6. Wait for the Docker build to finish, then open the generated URL.

The container listens on port `8080` and runs in `Production` mode.

### Option 2: Docker

```bash
docker build -t razor-weather .
docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production razor-weather
```

Open [http://localhost:8080](http://localhost:8080).

### Option 3: Manual publish

```bash
dotnet publish RazorInterviewDemo/RazorInterviewDemo.csproj -c Release -o ./publish
cd publish
ASPNETCORE_URLS=http://0.0.0.0:8080 dotnet RazorInterviewDemo.dll
```

Other compatible hosts: Azure App Service, Railway, Fly.io, or a Linux VPS with Nginx.

Requirements: **.NET 10** runtime (or use the provided Dockerfile) and outbound HTTPS to Open-Meteo and Nominatim.

## License

This project is intended as a demo / interview sample. Check third-party API terms before commercial use.
