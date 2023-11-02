$('[id^="Image_"]').each(async function () {
    var image = this;
    var canvas = document.getElementById("Canvas_" + image.id.replace("Image_", ""));
    var ctx = canvas.getContext('2d');
    var img = new Image();

    var boxesFromDb = document.getElementById("boxesFromDb_" + image.id.replace("Image_", "")).value;
    var boxes = JSON.parse(boxesFromDb);

    if (canvas && image) {
        img.src = image.src;

        // Wait for the image to load before drawing
        await new Promise((resolve) => {
            img.onload = resolve;
        });

        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        boxes.forEach(drawBox);
    }

    // Simulate clik event on the canvas to trigger the drawing code
    var clickEvent = new MouseEvent('click');
    canvas.dispatchEvent(clickEvent);

    // Function to draw a box with a name
    function drawBox(box) {
        ctx.strokeStyle = 'red';
        ctx.lineWidth = 2;
        let width = box.endX - box.startX;
        let height = box.endY - box.startY;
        ctx.strokeRect(box.startX, box.startY, width, height);

        ctx.fillStyle = 'white';
        ctx.fillRect(box.startX, box.startY - 25, ctx.measureText(box.name).width + 40, 25);

        ctx.font = "16px Arial";
        ctx.fillStyle = "black";
        ctx.fillText(box.name, box.startX + 5, box.startY - 5);
    }
});
