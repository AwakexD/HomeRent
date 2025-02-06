document.querySelector(".cancel-booking").addEventListener("click", function (event) {
    event.preventDefault();

    console.log("Click")

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
           window.location.href = "/Dashboard/Index";
        })
        .catch(error => console.error("Error:", error));
});