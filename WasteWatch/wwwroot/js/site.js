var canvas = document.getElementById("skiaCanvas");
var ctx = canvas.getContext("2d");

var isDragging = false;
var startX, startY, endX, endY;

canvas.addEventListener("mousedown", function (e) {
    isDragging = true;
    startX = e.clientX - canvas.getBoundingClientRect().left;
    startY = e.clientY - canvas.getBoundingClientRect().top;
});

canvas.addEventListener("mousemove", function (e) {
    if (isDragging) {
        endX = e.clientX - canvas.getBoundingClientRect().left;
        endY = e.clientY - canvas.getBoundingClientRect().top;
        drawSelectionBox();
    }
});

canvas.addEventListener("mouseup", function () {
    isDragging = false;
    clearCanvas();
});

function drawSelectionBox() {
    clearCanvas();
    ctx.strokeStyle = "blue";
    ctx.lineWidth = 2;
    ctx.strokeRect(startX, startY, endX - startX, endY - startY);
}

function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}