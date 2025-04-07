// unity-template-property.js
export const templateProperty = {
  LOADER_FILENAME: "{{{ LOADER_FILENAME }}}",
  DATA_FILENAME: "{{{ DATA_FILENAME }}}",
  FRAMEWORK_FILENAME: "{{{ FRAMEWORK_FILENAME }}}",
  CODE_FILENAME: "{{{ CODE_FILENAME }}}",
  COMPANY_NAME: "{{{ COMPANY_NAME }}}",
  PRODUCT_NAME: "{{{ PRODUCT_NAME }}}",
  PRODUCT_VERSION: "{{{ PRODUCT_VERSION }}}",
  WIDTH: JSON.parse("{{{ WIDTH }}}"),
  HEIGHT: JSON.parse("{{{ HEIGHT }}}"),
  DEBUG: () => {
    try {
      return JSON.parse("{{{ DEBUG }}}");
    } catch (error) {
      return false;
    }
  },
  OPTIMIZE_FOR_PIXEL_ART: () => {
    try {
      return JSON.parse("{{{ OPTIMIZE_FOR_PIXEL_ART }}}");
    } catch (error) {
      return false;
    }
  },
  PORTRAIT_BIGGEST_WIDTH: () => {
    try {
      return JSON.parse("{{{ PORTRAIT_BIGGEST_WIDTH }}}");
    } catch (error) {
      return 0;
    }
  },
  PORTRAIT_SMALLEST_WIDTH: () => {
    try {
      return JSON.parse("{{{ PORTRAIT_SMALLEST_WIDTH }}}");
    } catch (error) {
      return 0;
    }
  },
  PORTRAIT_BIGGEST_HEIGHT: () => {
    try {
      return JSON.parse("{{{ PORTRAIT_BIGGEST_HEIGHT }}}");
    } catch (error) {
      return 0;
    }
  },
  PORTRAIT_SMALLEST_HEIGHT: () => {
    try {
      return JSON.parse("{{{ PORTRAIT_SMALLEST_HEIGHT }}}");
    } catch (error) {
      return 0;
    }
  },
  LANDSCAPE_BIGGEST_WIDTH: () => {
    try {
      return JSON.parse("{{{ LANDSCAPE_BIGGEST_WIDTH }}}");
    } catch (error) {
      return 0;
    }
  },
  LANDSCAPE_SMALLEST_WIDTH: () => {
    try {
      return JSON.parse("{{{ LANDSCAPE_SMALLEST_WIDTH }}}");
    } catch (error) {
      return 0;
    }
  },
  LANDSCAPE_BIGGEST_HEIGHT: () => {
    try {
      return JSON.parse("{{{ LANDSCAPE_BIGGEST_HEIGHT }}}");
    } catch (error) {
      return 0;
    }
  },
  LANDSCAPE_SMALLEST_HEIGHT: () => {
    try {
      return JSON.parse("{{{ LANDSCAPE_SMALLEST_HEIGHT }}}");
    } catch (error) {
      return 0;
    }
  },
  MEMORY_FILENAME: () => {
    try {
      return JSON.parse("{{{ MEMORY_FILENAME }}}");
    } catch (error) {
      return null;
    }
  },
  SYMBOLS_FILENAME: () => {
    try {
      return JSON.parse("{{{ SYMBOLS_FILENAME }}}");
    } catch (error) {
      return null;
    }
  },
  PORTRAIT_MODE: () => {
    try {
      return JSON.parse("{{{ PORTRAIT_MODE }}}");
    } catch (error) {
      return false;
    }
  },

}
export default templateProperty;
