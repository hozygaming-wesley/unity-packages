import { progressHandler, onResize, displayDebugBoard } from "./util.js";
import templateProperty from "./template-property.js";
import unityManager from "./unity-manager.js";
const {
  DEBUG,
  OPTIMIZE_FOR_PIXEL_ART,
  LANDSCAPE_BIGGEST_HEIGHT,
  LANDSCAPE_BIGGEST_WIDTH,
  LANDSCAPE_SMALLEST_HEIGHT,
  LANDSCAPE_SMALLEST_WIDTH,
  PORTRAIT_BIGGEST_HEIGHT,
  PORTRAIT_BIGGEST_WIDTH,
  PORTRAIT_SMALLEST_HEIGHT,
  PORTRAIT_SMALLEST_WIDTH,
  PORTRAIT_MODE,
} = templateProperty;

window.onload = () => {
  // get the unity canvas
  const unityCanvas = document.getElementById("unity-canvas");
  // define the resize event handler
  const handleResize = () => onResize(
    unityCanvas.parentElement,
    unityCanvas,
    LANDSCAPE_BIGGEST_HEIGHT(),
    LANDSCAPE_BIGGEST_WIDTH(),
    LANDSCAPE_SMALLEST_HEIGHT(),
    LANDSCAPE_SMALLEST_WIDTH(),
    PORTRAIT_BIGGEST_HEIGHT(),
    PORTRAIT_BIGGEST_WIDTH(),
    PORTRAIT_SMALLEST_HEIGHT(),
    PORTRAIT_SMALLEST_WIDTH(),
    PORTRAIT_MODE(),
  );
  // define the progress event handler
  const handleProgress = (progress) => progressHandler(progress, unityCanvas);

  // add meta tag for mobile
  if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    const meta = document.createElement("meta");
    meta.name = "viewport";
    meta.content =
      "width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes";
    document.getElementsByTagName("head")[0].appendChild(meta);
  }

  // add resize event listener
  window.addEventListener("resize", handleResize);
  // activate the resize event
  handleResize();

  // create unity instance
  unityManager.CreateUnityInstance(unityCanvas, handleProgress, (instance) => {
    // on unity loaded
    unityCanvas.setAttribute("data-pixel-art", OPTIMIZE_FOR_PIXEL_ART());
    unityCanvas.style.cssText = "background: transparent !important; z-index: 100;";
    handleResize();
    if (DEBUG()) {
      // open debug board
      displayDebugBoard();
      // expose unityCanvas and unityInstance to the window object
      window.unityCanvas = unityCanvas;
      window.unityInstance = instance;
    }
  });
};