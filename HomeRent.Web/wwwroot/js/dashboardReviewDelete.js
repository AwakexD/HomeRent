document.addEventListener('DOMContentLoaded', () => {
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const antiForgeryToken = tokenInput ? tokenInput.value : '';

    const trashIcons = document.querySelectorAll('.icon-trash');

    trashIcons.forEach(icon => {
        icon.addEventListener('click', (event) => {
            const reviewItem = event.target.closest('li.mess-item');
            if (!reviewItem) return;

            const reviewIdInput = reviewItem.querySelector('input[name="ReviewId"]');
            if (!reviewIdInput) return;

            const reviewId = reviewIdInput.value;

            fetch('/api/Review/Delete', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': antiForgeryToken
                },
                body: JSON.stringify({ reviewId: reviewId })
            })
                .then(response => {
                    if (response.ok) {
                        reviewItem.remove();
                    } else {
                        return response.json().then(data => {
                            throw new Error(data.message);
                        });
                    }
                })
                .catch(error => {
                    console.error('Грешка при изтриване:', error);
                });
        });
    });
});