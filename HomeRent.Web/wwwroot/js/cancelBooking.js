﻿document.querySelector(".cancel-booking").addEventListener("click", function (event) {
    event.preventDefault();

    const antiForgeryToken = document.querySelector("[name='__RequestVerificationToken']").value;

    const bookingId = this.getAttribute("data-booking-id");

    fetch(`/api/Booking/CancelBooking?bookingId=${bookingId}`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": antiForgeryToken,
        }
    })
        .then(response => {
            window.location.reload();
        })
        .catch(error => console.error("Error:", error));
});