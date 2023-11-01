﻿$(document).ready(function () {
    $('[id^="Image_"]').each(function (index) {
        var image = this;
        var canvas = document.getElementById("Canvas_" + image.id.replace("Image_", ""));
        var body = document.getElementById("PageBody");
        var ctx = canvas.getContext('2d');
        var startX, startY, endX, endY;
        var img = new Image();

        var boxFilled = false;
        var boxesFromDb = document.getElementById("boxesFromDb_" + image.id.replace("Image_", "")).value;

        if (canvas && image) {
            // Set the image source and draw it on the canvas when it loads
            img.src = image.src;
            img.onload = function () {
                ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
            };
        }

        body.addEventListener("mousemove", function (e) {
            endX = e.pageX - canvas.offsetLeft;
            endY = e.pageY - canvas.offsetTop;
            if (boxesFromDb !== "" && !boxFilled) {
                boxes = JSON.parse(boxesFromDb);
                boxFilled = true;
                clearCanvas();
                drawSelectionBox();
                boxes.forEach(drawBox);
            }
            boxes.push({ startX, startY, endX, endY });
            drawSelectionBox();
        });

        // Function to clear the canvas
        function clearCanvas() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
            boxes.forEach(drawBox);
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
        }
    });
});
