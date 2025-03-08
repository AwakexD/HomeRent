document.querySelectorAll(".cancel-booking").forEach(element => {
    element.addEventListener("click", event => {
        event.preventDefault();

        const antiForgeryToken = document.querySelector("[name='__RequestVerificationToken']").value;
        const bookingId = element.getAttribute("data-booking-id");

        fetch(`/api/Booking/CancelBooking?bookingId=${bookingId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": antiForgeryToken,
            }
        })
            .then(response => window.location.href = "/Dashboard/Index")
            .catch(error => console.error("Error:", error));
    });
});
