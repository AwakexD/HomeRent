document.getElementById('review-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const antiForgeryToken = document.getElementsByName('__RequestVerificationToken')[1].value;

    const formData = new FormData(this);
    const data = Object.fromEntries(formData.entries());

    const response = await fetch('/api/Review/Create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-VERIFICATION-TOKEN': antiForgeryToken,
        },
        body: JSON.stringify(data),
    });
});
