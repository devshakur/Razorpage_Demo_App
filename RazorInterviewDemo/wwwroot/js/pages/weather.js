(() => {
    const DEFAULT_LATITUDE = 40.7128;
    const DEFAULT_LONGITUDE = -74.0060;

    function getCoordinatesFromQuery() {
        const params = new URLSearchParams(window.location.search);
        const latitude = params.get("latitude");
        const longitude = params.get("longitude");

        if (!latitude || !longitude) {
            return null;
        }

        return { latitude, longitude };
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
            tab.style.backgroundColor = isActive ? "#ffffff" : "transparent";
            tab.style.color = isActive ? "#111111" : "#888888";
        });

        panels.forEach((panel) => {
            const isActive = panel.dataset.forecastPanel === tabName;
            panel.classList.toggle("d-none", !isActive);
            panel.hidden = !isActive;
        });
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
                    case "refresh-weather":
                        window.location.reload();
                        break;
                    case "nav-weather":
                        setActiveForecastTab("today");
                        break;
                    case "nav-notes":
                    case "nav-travel":
                        button.classList.add("is-pressed");
                        window.setTimeout(() => button.classList.remove("is-pressed"), 180);
                        break;
                    default:
                        break;
                }
            });
        });
    }

    detectLocation();
    initForecastTabs();
    initTodayPagination();
    initActionButtons();
})();
