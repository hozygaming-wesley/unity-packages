export const progressHandler = (progress, canvas) => {
    const percent = `${progress * 100}%`;
    canvas.style.background = `linear-gradient(to right, white, white ${percent}, transparent ${percent}, transparent) no-repeat center`;
    canvas.style.backgroundSize = "100% 1rem";
};

/**
 * Determines if the current window orientation is portrait.
 *
 * @returns {boolean} True if the window is in portrait mode, false otherwise.
 */
export const isPortrait = () => {
    return window.innerHeight > window.innerWidth;
};
/**
 * Adjusts the size of the Unity canvas based on the window dimensions and specified canvas size constraints.
 *
 * @param {number} canvasBiggestHeight - The maximum height of the canvas.
 * @param {number} canvasBiggestWidth - The maximum width of the canvas.
 * @param {number} canvasSmallestHeight - The minimum height of the canvas.
 * @param {number} canvasSmallestWidth - The minimum width of the canvas.
 * @param {HTMLElement} container - The container element for the canvas.
 * @param {HTMLCanvasElement} canvas - The canvas element to be resized.
 */
export const onResize = (
    container,
    canvas,
    landscapeBiggestHeight,
    landscapeBiggestWidth,
    landscapeSmallestHeight,
    landscapeSmallestWidth,
    portraitBiggestHeight,
    portraitBiggestWidth,
    portraitSmallestHeight,
    portraitSmallestWidth,
    isPortraitMode
) => {

    // cuclate the ratio of the canvas
    // height / width = ratio
    // width * ratio = height
    // height / ratio = width

    // Landscape
    // 1708*960 : 0.5620
    // 1708*720 : 0.4215
    // 1280*720 : 0.5625
    // 1280*960 : 0.75

    // Portrait
    // 720*1708 : 2.3722
    // 960*1708 : 1.7791
    // 720*1280 : 1.7777
    // 960*1280 : 1.3333

    let heightResult = window.innerHeight;
    let widthResult = window.innerWidth;

    // for landscape mode
    const landscapeSmallestRatio = landscapeSmallestHeight / landscapeSmallestWidth;
    const landscapeBiggestRatio = landscapeBiggestHeight / landscapeBiggestWidth;
    let landscapeSmallestMinWidth = Math.floor(window.innerHeight / landscapeSmallestRatio);
    let landscapeBiggestMinWidth = Math.floor(window.innerHeight / landscapeBiggestRatio);

    // for portrait mode
    const portraitSmallestRatio = portraitSmallestHeight / portraitBiggestWidth;
    const portraitBiggestRatio = portraitBiggestHeight / portraitSmallestWidth;
    let portraitSmallestMinWidth = Math.floor(window.innerHeight / portraitSmallestRatio);
    let portraitBiggestMinWidth = Math.floor(window.innerHeight / portraitBiggestRatio);

    if (isPortraitMode) {
        // unityManager.SendMessage("WebGLMessager", "SetRatioMode", "Portrait");
        if (window.innerWidth < portraitBiggestMinWidth) {
            widthResult = window.innerWidth;
            heightResult = Math.floor(window.innerWidth * portraitBiggestRatio);
        } else if (window.innerWidth > portraitSmallestMinWidth) {
            widthResult = Math.floor(window.innerHeight / portraitSmallestRatio);
        } else {
            widthResult = window.innerWidth;
        }

    }
    else {
        // unityManager.SendMessage("WebGLMessager", "SetRatioMode", "Landscape");
        if (window.innerWidth < landscapeSmallestMinWidth) {
            widthResult = window.innerWidth;
            heightResult = Math.floor(window.innerWidth * landscapeSmallestRatio);
        }
        else if (window.innerWidth > landscapeBiggestMinWidth) {
            widthResult = Math.floor(window.innerHeight / landscapeBiggestRatio);
        }
        else {
            widthResult = window.innerWidth;
        }
    }

    container.style.width = canvas.style.width = `${widthResult}px`;
    container.style.height = canvas.style.height = `${heightResult}px`;
    container.style.top = `${Math.floor((window.innerHeight - heightResult) / 2)}px`;
    container.style.left = `${Math.floor((window.innerWidth - widthResult) / 2)}px`;
};


export const displayDebugBoard = () => {
    const fpsCounter = document.createElement('div');
    Object.assign(fpsCounter.style, {
        position: 'fixed',
        top: '10px',
        right: '10px',
        backgroundColor: 'rgba(0, 0, 0, 0.7)',
        color: 'white',
        padding: '5px',
        fontFamily: 'Arial, sans-serif',
        fontSize: '14px',
        zIndex: '1000'
    });
    fpsCounter.textContent = 'FPS: 0';
    document.body.appendChild(fpsCounter);

    let lastFrameTime = performance.now();
    let frameCount = 0;

    const updateFPS = () => {
        const now = performance.now();
        frameCount++;
        const delta = now - lastFrameTime;

        if (delta >= 1000) {
            fpsCounter.textContent = `FPS: ${frameCount}`;
            frameCount = 0;
            lastFrameTime = now;
        }

        requestAnimationFrame(updateFPS);
    };

    updateFPS();
};


