const calendarElement = document.querySelector(".booking-calendar");
const propertyId = document.getElementById("propertyId").value;

// Reusable function for API calls
async function fetchAPI(url, options = {}) {
    try {
        const response = await fetch(url, options);

        // Check if the response is JSON or plain text
        const contentType = response.headers.get("Content-Type");
        if (contentType && contentType.includes("application/json")) {
            return await response.json();
        } else {
            return await response.text();
        }
    } catch (error) {
        console.error(`Error fetching data from ${url}:`, error);
        throw error;
    }
}

// Fetch booked dates
async function fetchBookedDates(propertyId) {
    const url = `/api/Booking/GetBookedDates?propertyId=${propertyId}`;
    return (await fetchAPI(url)).disable || [];
}

// Flatpickr initialization
async function initializeFlatpickr() {
    const bookedDates = await fetchBookedDates(propertyId);

    flatpickr(calendarElement, {
        minDate: "today",
        mode: "range",
        dateFormat: "d-m-Y",
        disable: bookedDates,
        onChange: async (selectedDates) => {
            const startDateInput = document.getElementById("start-date");
            const endDateInput = document.getElementById("end-date");
            const priceDisplay = document.querySelector("#total-price");

            if (selectedDates.length === 2) {
                const [startDate, endDate] = selectedDates;
                startDateInput.value = startDate.toISOString().split("T")[0];
                endDateInput.value = endDate.toISOString().split("T")[0];

                try {
                    const { pricePerNight } = await fetchAPI(`/api/Booking/GetPrice?propertyId=${propertyId}`);
                    const msPerDay = 24 * 60 * 60 * 1000;
                    const numDays = Math.ceil((endDate - startDate) / msPerDay);
                    const totalPrice = numDays * pricePerNight;
                    priceDisplay.textContent = `Обща цена · ${totalPrice}лв.`;
                } catch {
                    priceDisplay.textContent = "Грешка при изчисляване на цената.";
                }
            } else {
                startDateInput.value = "";
                endDateInput.value = "";
                priceDisplay.textContent = "Обща цена · 0лв.";
            }
        },
    });
}

// Booking form submission
async function handleFormSubmit(event) {
    event.preventDefault();

    const formData = new FormData(this);
    const data = Object.fromEntries(formData.entries());
    const antiForgeryToken = document.querySelector("[name='__RequestVerificationToken']").value;

    try {
        const redirectUrl = await fetchAPI("/api/Booking/CreateBooking", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": antiForgeryToken,
            },
            body: JSON.stringify(data),
        });

        window.location.href = redirectUrl;
    } catch (error) {
        console.error("Failed to submit booking:", error);
    }
}

// Initialize components
document.getElementById("booking-form").addEventListener("submit", handleFormSubmit);
initializeFlatpickr();
