const calendarElement = document.querySelector(".booking-calendar");
const propertyId = document.getElementById("propertyId").value;

// Reusable function for API calls
async function fetchAPI(url, options = {}) {
    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        return await response.json();
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
        await fetchAPI("/api/Booking/Create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": antiForgeryToken,
            },
            body: JSON.stringify(data),
        });
    } catch {
        throw new Error("Failed to submit booking. Please try again.");
    }
}

// Initialize components
document.getElementById("booking-form").addEventListener("submit", handleFormSubmit);
initializeFlatpickr();
