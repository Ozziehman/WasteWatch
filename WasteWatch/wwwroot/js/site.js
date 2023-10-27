var canvas = document.getElementById('skiaCanvas');
var image = document.getElementById('sourceImage');
var ctx = canvas.getContext('2d');
var isDragging = false;
var startX, startY, endX, endY;
var img = new Image();
var boxNameInput = document.getElementById('boxName');
var saveBoxButton = document.getElementById('saveBoxButton');
var overview = document.getElementById('overview');
var boxes = [];

if (canvas && image) {
    img.src = image.src;
    img.onload = function () {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
}

canvas.addEventListener("mousedown", function (e) {
    isDragging = true;
    startX = e.clientX - canvas.getBoundingClientRect().left;
    startY = e.clientY - canvas.getBoundingClientRect().top;
});

canvas.addEventListener("mousemove", function (e) {
    if (isDragging) {
        endX = e.clientX - canvas.getBoundingClientRect().left;
        endY = e.clientY - canvas.getBoundingClientRect().top;
        clearCanvas();
        boxes.forEach(drawBox);
        drawSelectionBox();
    }
});

canvas.addEventListener("mouseup", function () {
    isDragging = false;
    boxNameInput.style.display = 'block';
    saveBoxButton.style.display = 'block';
    boxNameInput.focus();
});

saveBoxButton.addEventListener("click", function () {
    let boxName = boxNameInput.value;
    if (boxName) {
        boxes.push({ startX, startY, endX, endY, name: boxName });

        // Log the coordinates
        console.log("Box Coordinates: startX=" + startX + ", startY=" + startY + ", endX=" + endX + ", endY=" + endY);
        console.log("Box Name: " + boxName);

        clearCanvas();
        boxes.forEach(drawBox);
        updateOverview();
        boxNameInput.style.display = 'none';
        saveBoxButton.style.display = 'none';
    }
});

function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    boxes.forEach(drawBox);
}

function drawSelectionBox() {
    ctx.strokeStyle = 'blue';
    ctx.lineWidth = 2;
    let width = endX - startX;
    let height = endY - startY;
    ctx.strokeRect(startX, startY, width, height);
}

function drawBox(box) {
    ctx.strokeStyle = 'red';
    ctx.lineWidth = 2;
    let width = box.endX - box.startX;
    let height = box.endY - box.startY;
    ctx.strokeRect(box.startX, box.startY, width, height);

    // Draw a filled rectangle as a button-like label
    ctx.fillStyle = 'white'; // Background color for the label
    ctx.fillRect(box.startX, box.startY - 25, ctx.measureText(box.name).width + 10, 25);

    ctx.font = "16px Arial";
    ctx.fillStyle = "black";
    ctx.fillText(box.name, box.startX + 5, box.startY - 5);
}

function updateOverview() {
    // Clear the overview
    overview.innerHTML = '';

    // Create an overview of all boxes
    boxes.forEach(function (box, index) {
        var boxDiv = document.createElement('div');
        boxDiv.classList.add('overview-box'); // Add both 'btn' and 'overview-box' classes
        boxDiv.textContent = 'Box ' + (index + 1) + ': ' + box.name;

        // Create a delete button
        var deleteButton = document.createElement('button');
        deleteButton.classList.add('btn-danger', 'btn');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', function () {
            // Handle click on the delete button to delete the box
            deleteBox(index);
        });

        // Append the delete button to the overview box
        boxDiv.appendChild(deleteButton);

        // Add a click event to the boxDiv for editing
        boxDiv.addEventListener('click', function () {
            // Handle click on the overview box to edit it
            editBox(index);
        });

        // Append the overview box to the overview container
        overview.appendChild(boxDiv);
    });
}


function deleteBox(index) {
    // Remove the box from the 'boxes' array
    boxes.splice(index, 1);

    // Redraw the canvas and update the overview
    clearCanvas();
    updateOverview();
}

function editBox(index) {
    // You can implement the box editing logic here, for example:
    var editedBox = boxes[index];
    var newName = prompt("Edit the box name:");
    if (newName !== null) {
        editedBox.name = newName;
        clearCanvas();
        boxes.forEach(drawBox);
        updateOverview();
    }
}

// Initial update of the overview
updateOverview();