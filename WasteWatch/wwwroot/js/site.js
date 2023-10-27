// Get references to various HTML elements
var canvas = document.getElementById('skiaCanvas');
var image = document.getElementById('sourceImage');
var imageName = document.getElementById('imageName');
var ctx = canvas.getContext('2d');
var isDragging = false;
var startX, startY, endX, endY;
var img = new Image();
var boxNameInput = document.getElementById('boxName');
var saveBoxButton = document.getElementById('button-addon2');
var overview = document.getElementById('overview');
var downloadButton = document.getElementById('downloadButton');
var boxes = []; // array of boxes with the format {name, startX, startY, endX, endY}
var dataToDownload = []; // array of boxes with the format {name, startX, startY, endX, endY}



// Check if the canvas and image elements exist
if (canvas && image) {
    // Set the image source and draw it on the canvas when it loads
    img.src = image.src;
    img.onload = function () {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
}

// Event listener for when the mouse button is pressed on the canvas
canvas.addEventListener("mousedown", function (e) {
    isDragging = true;
    // Start drawing the selection box
    startX = e.clientX - canvas.getBoundingClientRect().left;
    startY = e.clientY - canvas.getBoundingClientRect().top;
});

// Event listener for when the mouse is moved over the canvas
canvas.addEventListener("mousemove", function (e) {
    if (isDragging) {
        endX = e.clientX - canvas.getBoundingClientRect().left;
        endY = e.clientY - canvas.getBoundingClientRect().top;
        clearCanvas();
        boxes.forEach(drawBox);
        drawSelectionBox();
    }
});

// Event listener for when the mouse button is released on the canvas
canvas.addEventListener("mouseup", function () {
    isDragging = false;
    boxNameInput.style.display = 'block';
    saveBoxButton.style.display = 'block';
    boxNameInput.focus();
});

// Event listener for the "Save Box" button
saveBoxButton.addEventListener("click", function () {
    let boxName = boxNameInput.value;
    if (boxName) {
        let box = {
            name: boxName,
            startX: startX,
            startY: startY,
            endX: endX,
            endY: endY
        };
        boxes.push(box);
        dataToDownload.push(box);
        // Log the coordinates
        console.log("Box Coordinates: startX=" + startX + ", startY=" + startY + ", endX=" + endX + ", endY=" + endY);
        console.log("Box Name: " + boxName);

        // Redraw all boxes on the canvas
        clearCanvas();
        boxes.forEach(drawBox);
        updateOverview();

        boxNameInput.style.display = 'none';
        saveBoxButton.style.display = 'none';
    }
});

// Event listener for the "Download" button
downloadButton.addEventListener("click", function () {
    const jsZip = new JSZip();

    // Create text files with box data inside a zip file
    dataToDownload.forEach((item, index) => {
        const textContent = `name: ${item.name}, startX: ${item.startX}, startY: ${item.startY}, endX: ${item.endX}, endY: ${item.endY}`;
        jsZip.file(`data_${index}.txt`, textContent);
    });

    // Generate and trigger the download of the zip file
    jsZip.generateAsync({ type: "blob" })
        .then(function (blob) {
            const zipFile = new Blob([blob], { type: 'application/zip' });

            // Create a temporary URL for the zip file
            const blobURL = URL.createObjectURL(zipFile);

            // Create a temporary link element to trigger the download
            const a = document.createElement('a');
            a.href = blobURL;
            a.download = 'data.zip';

            // Simulate a click on the link to trigger the download
            a.click();

            // Clean up by revoking the blob URL
            URL.revokeObjectURL(blobURL);
        });
});

// Function to clear the canvas
function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    boxes.forEach(drawBox);
    console.log("Cleared canvas")
}

// Function to draw the selection box
function drawSelectionBox() {
    ctx.strokeStyle = 'blue';
    ctx.lineWidth = 2;
    let width = endX - startX;
    let height = endY - startY;
    ctx.strokeRect(startX, startY, width, height);
}

// Function to draw a box with a name
function drawBox(box) {
    ctx.strokeStyle = 'red';
    ctx.lineWidth = 2;
    let width = box.endX - box.startX;
    let height = box.endY - box.startY;
    ctx.strokeRect(box.startX, box.startY, width, height);

    ctx.fillStyle = 'white';
    ctx.fillRect(box.startX, box.startY - 25, ctx.measureText(box.name).width + 10, 25);

    ctx.font = "16px Arial";
    ctx.fillStyle = "black";
    ctx.fillText(box.name, box.startX + 5, box.startY - 5);
}

// Function to update the overview of boxes
function updateOverview() {
    // Clear the overview
    overview.innerHTML = '';

    // Create an overview of all boxes
    boxes.forEach(function (box, index) {
        var boxContainer = document.createElement('div');
        boxContainer.classList.add('box-container');

        var boxDiv = document.createElement('div');
        boxDiv.classList.add('overview-box');

        var boxInfo = document.createElement('span');
        boxInfo.textContent = 'Box ' + (index + 1) + ': ' + box.name;

        var deleteButton = document.createElement('button');
        deleteButton.classList.add('btn-danger', 'btn');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', function (event) {
            event.stopPropagation(); // Stop event propagation
            // Handle click on the delete button to delete the box
            deleteBox(index);
        });

        boxDiv.appendChild(boxInfo);
        boxContainer.appendChild(boxDiv);
        boxContainer.appendChild(deleteButton);

        /*boxContainer.addEventListener('click', function () {
            // Handle click on the boxContainer for editing
            editBox(index);
        });*/

        // Append the boxContainer to the overview
        overview.appendChild(boxContainer);
    });
}

// Function to delete a box
function deleteBox(index) {
    // Remove the box from the 'boxes' array
    boxes.splice(index, 1);
    dataToDownload.splice(index, 1);
    // Redraw the canvas and update the overview
    clearCanvas();
    updateOverview();
}

// Function to edit a box's name
/*function editBox(index) {
    var editedBox = boxes[index];
    var newName = prompt("Edit the box name:");
    if (newName !== null) {
        editedBox.name = newName;
        clearCanvas();
        boxes.forEach(drawBox);
        updateOverview();
    }
}
*/
// Initial update of the overview
updateOverview();
