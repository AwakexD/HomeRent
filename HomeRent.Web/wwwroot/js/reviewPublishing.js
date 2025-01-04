document.addEventListener('DOMContentLoaded', () => {
    function ratingStarsHandler() {
        document.getElementById('divRating').addEventListener('click', function (event) {
            if (event.target.tagName.toLowerCase() !== 'span') return;

            Array.prototype.forEach.call(document.querySelectorAll('.rating > .star'), function (el) {
                el.classList.remove('rated');
            });

            event.target.classList.add('rated');
            let previousSiblings = Array.from(event.target.parentNode.children);
            let index = previousSiblings.indexOf(event.target);
            previousSiblings.slice(index).forEach(function (el) {
                el.classList.add('rated');
            });

            document.getElementById('ratingValue').value = 5 - index;
        });
    }

    ratingStarsHandler();
    document.getElementById('review-form').addEventListener('submit', async function (e) {
        e.preventDefault();

        const antiForgeryToken = document.getElementsByName('__RequestVerificationToken')[1].value;

        const formData = new FormData(this);
        const data = Object.fromEntries(formData.entries());

        const response = await fetch('/api/Review/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': antiForgeryToken,
            },
            body: JSON.stringify(data),
        });
    });
})