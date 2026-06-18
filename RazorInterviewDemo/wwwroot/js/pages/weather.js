(() => {
    const DEFAULT_LATITUDE = 40.7128;
    const DEFAULT_LONGITUDE = -74.0060;
    const THEME_STORAGE_KEY = "weather-theme";
    const THEME_CYCLE = ["dark", "light", "ocean", "sunset"];
    const FORECAST_PARAMS =
        "current=temperature_2m,apparent_temperature,relative_humidity_2m,wind_speed_10m,weather_code" +
        "&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code" +
        "&daily=weather_code,temperature_2m_max,temperature_2m_min,relative_humidity_2m_mean,wind_speed_10m_max" +
        "&forecast_days=7&timezone=auto&wind_speed_unit=ms";

    function getCoordinatesFromQuery() {
        const params = new URLSearchParams(window.location.search);
        const latitude = params.get("latitude");
        const longitude = params.get("longitude");

        if (!latitude || !longitude) {
            return null;
        }

        return {
            latitude: Number.parseFloat(latitude),
            longitude: Number.parseFloat(longitude)
        };
    }

    function formatCoordinates(latitude, longitude) {
        return `${latitude.toFixed(2)}, ${longitude.toFixed(2)}`;
    }

    function buildForecastUrl(latitude, longitude) {
        const params = new URLSearchParams({
            latitude: latitude.toFixed(4),
            longitude: longitude.toFixed(4)
        });

        return `https://api.open-meteo.com/v1/forecast?${params.toString()}&${FORECAST_PARAMS}`;
    }

    async function fetchForecast(latitude, longitude) {
        const response = await fetch(buildForecastUrl(latitude, longitude));
        if (!response.ok) {
            throw new Error(`Forecast request failed with status ${response.status}`);
        }

        return response.json();
    }

    async function fetchLocationName(latitude, longitude) {
        const params = new URLSearchParams({
            lat: latitude.toFixed(4),
            lon: longitude.toFixed(4),
            format: "json",
            addressdetails: "1",
            "accept-language": "en"
        });

        try {
            const response = await fetch(
                `https://nominatim.openstreetmap.org/reverse?${params.toString()}`,
                {
                    headers: {
                        Accept: "application/json"
                    }
                }
            );

            if (!response.ok) {
                return formatCoordinates(latitude, longitude);
            }

            const geocode = await response.json();
            const address = geocode.address;
            if (!address) {
                return formatCoordinates(latitude, longitude);
            }

            const city = address.city || address.town || address.village || address.state || "";
            const country = address.country || "";

            if (city && country) {
                return `${city}, ${country}`;
            }

            if (geocode.display_name) {
                return geocode.display_name;
            }
        } catch {
            return formatCoordinates(latitude, longitude);
        }

        return formatCoordinates(latitude, longitude);
    }

    function showWeatherError(root) {
        root.innerHTML = `
            <div class="weather-loading rounded-4 p-5 text-center">
                <p class="mb-3 weather-text-error">Unable to load live weather data. Please try again.</p>
                <button type="button"
                        class="btn rounded-pill px-4 weather-accent-btn"
                        data-action="refresh-weather">
                    Retry
                </button>
            </div>`;

        initActionButtons();
    }

    async function loadWeatherFromClient() {
        const root = document.getElementById("weather-root");
        const loading = document.getElementById("weather-loading-deferred");
        const coords = getCoordinatesFromQuery();

        if (!root || !loading || !coords) {
            return;
        }

        try {
            const [forecast, locationName] = await Promise.all([
                fetchForecast(coords.latitude, coords.longitude),
                fetchLocationName(coords.latitude, coords.longitude)
            ]);

            const response = await fetch("/weather/render", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Accept: "text/html"
                },
                body: JSON.stringify({
                    forecast,
                    locationName
                })
            });

            if (!response.ok) {
                throw new Error(`Render request failed with status ${response.status}`);
            }

            const html = await response.text();
            root.outerHTML = html;
            initForecastTabs();
            initTodayPagination();
            initActionButtons();
        } catch {
            showWeatherError(root);
        }
    }

    function redirectWithCoordinates(latitude, longitude) {
        const url = new URL(window.location.href);
        url.searchParams.set("latitude", latitude.toFixed(4));
        url.searchParams.set("longitude", longitude.toFixed(4));
        window.location.replace(url.toString());
    }

    function detectLocation() {
        if (getCoordinatesFromQuery()) {
            return;
        }

        if (!navigator.geolocation) {
            redirectWithCoordinates(DEFAULT_LATITUDE, DEFAULT_LONGITUDE);
            return;
        }

        navigator.geolocation.getCurrentPosition(
            (position) => {
                redirectWithCoordinates(position.coords.latitude, position.coords.longitude);
            },
            () => {
                redirectWithCoordinates(DEFAULT_LATITUDE, DEFAULT_LONGITUDE);
            },
            {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 300000
            }
        );
    }

    function setActiveForecastTab(tabName) {
        const tabs = document.querySelectorAll("[data-forecast-tab]");
        const panels = document.querySelectorAll("[data-forecast-panel]");

        tabs.forEach((tab) => {
            const isActive = tab.dataset.forecastTab === tabName;
            tab.classList.toggle("is-active", isActive);
            tab.setAttribute("aria-selected", isActive ? "true" : "false");
        });

        panels.forEach((panel) => {
            const isActive = panel.dataset.forecastPanel === tabName;
            panel.classList.toggle("d-none", !isActive);
            panel.hidden = !isActive;
        });
    }

    function updateThemeButtons(theme) {
        const settingsButton = document.querySelector('[data-action="theme-cycle"]');
        if (settingsButton) {
            settingsButton.classList.toggle("is-active", THEME_CYCLE.includes(theme));
        }
    }

    function cycleTheme() {
        const currentTheme = document.body.dataset.theme || "dark";
        const currentIndex = THEME_CYCLE.indexOf(currentTheme);
        const nextIndex = currentIndex === -1 ? 0 : (currentIndex + 1) % THEME_CYCLE.length;
        applyTheme(THEME_CYCLE[nextIndex]);
    }

    function applyTheme(theme) {
        document.body.dataset.theme = theme;
        localStorage.setItem(THEME_STORAGE_KEY, theme);
        updateThemeButtons(theme);
    }

    function initTheme() {
        const savedTheme = localStorage.getItem(THEME_STORAGE_KEY) || "dark";
        applyTheme(savedTheme);
    }

    function initTodayPagination() {
        const pagination = document.querySelector("[data-today-pagination]");
        if (!pagination) {
            return;
        }

        const pageSize = 7;
        const items = Array.from(document.querySelectorAll("[data-today-hour-item]"));
        const totalPages = Number.parseInt(pagination.dataset.totalPages, 10) || 1;
        let currentPage = 1;

        const prevButton = pagination.querySelector('[data-today-page="prev"]');
        const nextButton = pagination.querySelector('[data-today-page="next"]');
        const pageInfo = pagination.querySelector("[data-today-page-info]");

        function showPage(page) {
            currentPage = page;
            const start = (page - 1) * pageSize;
            const end = start + pageSize;

            items.forEach((item, index) => {
                item.classList.toggle("d-none", index < start || index >= end);
            });

            pageInfo.textContent = `Page ${currentPage} of ${totalPages}`;
            prevButton.disabled = currentPage === 1;
            nextButton.disabled = currentPage === totalPages;
        }

        prevButton.addEventListener("click", () => {
            if (currentPage > 1) {
                showPage(currentPage - 1);
            }
        });

        nextButton.addEventListener("click", () => {
            if (currentPage < totalPages) {
                showPage(currentPage + 1);
            }
        });
    }

    function initForecastTabs() {
        const card = document.querySelector("[data-forecast-card]");
        if (!card) {
            return;
        }

        card.querySelectorAll("[data-forecast-tab]").forEach((tab) => {
            tab.addEventListener("click", () => {
                setActiveForecastTab(tab.dataset.forecastTab);
            });
        });
    }

    function initActionButtons() {
        document.querySelectorAll("[data-action]").forEach((button) => {
            button.addEventListener("click", () => {
                switch (button.dataset.action) {
                    case "theme-cycle":
                        cycleTheme();
                        break;
                    case "nav-weather":
                        setActiveForecastTab("today");
                        break;
                    case "refresh-weather":
                        window.location.reload();
                        break;
                    default:
                        break;
                }
            });
        });
    }

    detectLocation();
    loadWeatherFromClient();
    initTheme();
    initForecastTabs();
    initTodayPagination();
    initActionButtons();
})();
