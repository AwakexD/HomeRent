document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.querySelector(".ip-file");
    const imgUploadBox = document.querySelector(".box-img-upload");

    fileInput.addEventListener("change", function () {
        const files = fileInput.files;
        for (let i = 0; i < files.length; i++) {
            const reader = new FileReader();
            const file = files[i];

            reader.onload = function (event) {
                const imagePreview = document.createElement("div");
                imagePreview.className = "item-upload file-delete";
                imagePreview.innerHTML = `<img src="${event.target.result}" alt="img">`;
                imgUploadBox.appendChild(imagePreview);
            };

            reader.readAsDataURL(file);
        }
    });
});
