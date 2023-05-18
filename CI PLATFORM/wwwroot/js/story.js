const dropZone = document.getElementById("drop-zone");
const fileInput = document.getElementById("file-input");
const imagePreview = document.getElementById("image-preview");
const uploadedFiles = new Set();

dropZone.addEventListener("click", () => {
    fileInput.click();
});

dropZone.addEventListener("dragover", (event) => {
    event.preventDefault();
    dropZone.classList.add("dragover");
});

dropZone.addEventListener("dragleave", () => {
    dropZone.classList.remove("dragover");
});

dropZone.addEventListener("drop", (event) => {
    event.preventDefault();
    dropZone.classList.remove("dragover");
    const files = event.dataTransfer.files;
    handleFiles(files);
});

fileInput.addEventListener("change", () => {
    const files = fileInput.files;
    handleFiles(files);
});

function handleFiles(files) {
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (!file.type.startsWith("image/") && !file.type.startsWith("video/")) continue;
        if (uploadedFiles.has(file.name)) {
            alert(`File "${file.name}" has already been uploaded.`);
            continue;
        }
        uploadedFiles.add(file.name);
        const image = document.createElement("img");
        image.classList.add("image-preview");
        const imageContainer = document.createElement("div");
        imageContainer.classList.add("image-container");
        const removeImage = document.createElement("div");
        removeImage.innerHTML = "&#10006;";
        removeImage.classList.add("remove-image");
        removeImage.addEventListener("click", () => {
            uploadedFiles.delete(file.name);
            imageContainer.remove();
        });
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            image.src = reader.result;
            imageContainer.appendChild(image);
            imageContainer.appendChild(removeImage);
            imagePreview.appendChild(imageContainer);
        };
    }
}

/*Story Listing Page*/
/*const dropZone = document.getElementById("drop-zone");
const fileInput = document.getElementById("file-input");
const imagePreview = document.getElementById("image-preview");
const uploadedFiles = new Set();

dropZone.addEventListener("click", () => {
    fileInput.click();
});

dropZone.addEventListener("dragover", (event) => {
    event.preventDefault();
    dropZone.classList.add("dragover");
});

dropZone.addEventListener("dragleave", () => {
    dropZone.classList.remove("dragover");
});


dropZone.addEventListener("drop", (event) => {
    event.preventDefault();
    dropZone.classList.remove("dragover");
    const files = event.dataTransfer.files;
    handleFiles(files);
});

fileInput.addEventListener("change", () => {
    const files = fileInput.files;
    handleFiles(files);
});
const totalfiles = [];
function handleFiles(files) {

    for (var i = 0; i < files.length; i++) {
        totalfiles.push(files[i]);
    }
    imagePreview.innerHTML = ""
    for (let i = 0; i < totalfiles.length; i++) {
        const file = totalfiles[i];
        if (!file.type.startsWith("image/") && !file.type.startsWith("video/")) continue;

        uploadedFiles.add(file.name);
        const image = document.createElement("img");
        image.classList.add("image-preview");
        const imageContainer = document.createElement("div");
        imageContainer.classList.add("image-container");
        const removeImage = document.createElement("div");
        removeImage.innerHTML = "&#10006;";
        removeImage.classList.add("remove-image");
        removeImage.addEventListener("click", () => {
            totalfiles.splice(i, 1);
            uploadedFiles.delete(file.name);
            imageContainer.remove();
        });
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            image.src = reader.result;
            imageContainer.appendChild(image);
            imageContainer.appendChild(removeImage);
            imagePreview.appendChild(imageContainer);
        };
    }
}

const totalfilesRemove = [];
function handleFiless(files) {
    for (var i = 0; i < files.length; i++) {
        totalfilesRemove.push(files[i]);
    }
    console.log(totalfilesRemove);
    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const image = document.createElement("img");
        image.classList.add("image-preview");
        const imageContainer = document.createElement("div");
        imageContainer.classList.add("image-container");
        const removeImage = document.createElement("div");
        removeImage.innerHTML = "&#10006;";
        removeImage.classList.add("remove-image");
        removeImage.addEventListener("click", () => {
            totalfilesRemove.splice(i, 1);
            uploadedFiles.delete(file);
            imageContainer.remove();
        });
        image.src = file;
        imageContainer.appendChild(image);
        imageContainer.appendChild(removeImage);
        imagePreview.appendChild(imageContainer);
    }
}

function GetDraftMission(missionId) {
    document.getElementById('StoryTitle').value = '';
    tinymce.activeEditor.setContent('');
    document.getElementById('PublishedAt').value = '';
    document.getElementById('VideoPath').value = '';
    document.getElementById('image-preview').value = '';
    if (missionId != '-1') {
        $.ajax({
            url: '/Story/getDraftMissions',
            type: 'json',
            data: {
                MissionId: missionId
            },
            success: function (result) {
                console.log(result);
                imagePreview.innerHTML = '';
                if (result.title != null)
                    document.getElementById('StoryTitle').value = result.title;

                if (result.description != null)
                    tinymce.activeEditor.setContent(result.description);

                if (result.publishedAt != null)
                    document.getElementById('PublishedAt').value = result.publishedAt.slice(0, 10);

                if (result.path != null)
                    document.getElementById('VideoPath').value = result.path;

                if (result.imagePath != null)
                    handleFiless(result.imagePath);
                if (result.storyId != null)
                    document.getElementById('STORYID').value = result.storyId
            }
        });
    }

}


function SubmitStoryDetails() {
    var Discription = tinymce.activeEditor.getContent();
    var form = document.getElementById('StoryForm');
    var formData = new FormData(form);
    formData.append('Description', Discription)
    for (var i = 0; i < totalfiles.length; i++) {
        formData.append('Storyimages', totalfiles[i]);
    }
    for (var i = 0; i < totalfilesRemove.length; i++) {
        formData.append('AllImagesPath', totalfilesRemove[i]);
    }
    $.ajax({
        url: 'StoryListingPage',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            debugger;
        },
        error: function (resonse) {
            debugger;
        }

    })
}*/