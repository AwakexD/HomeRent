const calendarElement = document.querySelector(".booking-calendar");

// Flatpickr calendar config
flatpickr(calendarElement, {
    minDate: "today",
    mode: "range",
    dateFormat: "d-m-Y",
    onChange: async function (selectedDates, dateStr, instance) {

        const startDateInput = document.getElementById("start-date");
        const endDateInput = document.getElementById("end-date");
        if (selectedDates.length === 2) {
            startDateInput.value = selectedDates[0].toISOString().split("T")[0];
            endDateInput.value = selectedDates[1].toISOString().split("T")[0];

            const propertyId = document.getElementById("propertyId").value;
            try {
                const response = await fetch(`/api/Booking/GetPrice?propertyId=${propertyId}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                });
                if (response.ok) {
                    const data = await response.json();
                    const pricePerNight = data.pricePerNight;

                    const msPerDay = 24 * 60 * 60 * 1000;
                    const numDays = Math.ceil((selectedDates[1] - selectedDates[0]) / msPerDay);
                    const totalPrice = numDays * pricePerNight;

                    const priceDisplay = document.querySelector("#total-price");
                    priceDisplay.textContent = `Обща цена · ${totalPrice}лв.`;
                }
            } catch (error) {
                console.error("Error fetching price:", error);
            }
        } else {
            startDateInput.value = "";
            endDateInput.value = "";
            const priceDisplay = document.querySelector("#total-price");
            priceDisplay.textContent = "Обща цена · 0лв.";
        }
    }
});

// Create booking form handler

const bookingFrom = document.getElementById("booking-form")
bookingFrom.addEventListener("submit", async function (e) {
    e.preventDefault();

    const formData = new FormData(this);
    const data = Object.fromEntries(formData.entries());

    const antiForgeryToken = document.getElementsByName('__RequestVerificationToken')[1].value;

    try {
        const response = await fetch("/api/Booking/Create", {
            method: "POST",
            headers: {
                'Content-Type': "application/json",
                'RequestVerificationToken': antiForgeryToken,
            },
            body: JSON.stringify(data),
        });
    } catch (error) {
        console.error("Error submitting form:", error);
    }
})

// ToDO : Fetch booked dates from the BookingsController (API Controller) and make them as disabled