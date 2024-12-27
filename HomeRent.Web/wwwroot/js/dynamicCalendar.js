const calendarElement = document.querySelector(".booking-calendar");

// Flatpickr calendar config
flatpickr(calendarElement, {
    mode: "range",
    dateFormat: "d-m-Y",
    onChange: function (selectedDates, dateStr, instance) {
        console.log("Selected range:", dateStr);
    }
});

// ToDO : Fetch booked dates from the BookingsController (API Controller) and make them as disabled
// ToDO : Create an input fields (hidden) storing the booking date value - onchange
// ToDO : Automatically calculate the price (on date range change) and display it above the submit button