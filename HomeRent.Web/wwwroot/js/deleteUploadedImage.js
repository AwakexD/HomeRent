document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.remove-file');

    deleteButtons.forEach(button => {
        button.addEventListener('click', async function () {
            const container = button.closest('.item-upload');
            if (!container) return;

            const publicId = container.getAttribute('data-public-id');
            const propertyId = document.getElementById("propertyId").value;
            if (!publicId || !propertyId) return;

            const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

            try {
                const response = await fetch('/Property/DeletePhoto', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': antiForgeryToken
                    },
                    body: JSON.stringify({ propertyId:propertyId, publicId: publicId })
                });

                const result = await response.json();

                if (result.success) {
                    container.remove();
                } else {
                    console.error(result.message || 'Failed to delete image.');
                }
            } catch (error) {
                console.error('Error deleting image:', error);
            }
        });
    });
});