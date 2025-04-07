import templateProperty from "./template-property.js";

export class UnityManager {
  constructor() {
    if (UnityManager.instance) {
      return UnityManager.instance;
    }

    this.unityInstance = null;
    this.config = this.createDefaultConfig();

    UnityManager.instance = this;
  }

  createDefaultConfig() {
    const config = {
      dataUrl: `Build/${templateProperty.DATA_FILENAME}`,
      frameworkUrl: `Build/${templateProperty.FRAMEWORK_FILENAME}`,
      codeUrl: `Build/${templateProperty.CODE_FILENAME}`,
      streamingAssetsUrl: "StreamingAssets",
      companyName: `${templateProperty.COMPANY_NAME}`,
      productName: `${templateProperty.PRODUCT_NAME}`,
      productVersion: `${templateProperty.PRODUCT_VERSION}`,
      // matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
      // devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
      // autoSyncPersistentDataPath: true,
    };

    const memoryFilename = templateProperty.MEMORY_FILENAME();
    if (memoryFilename) {
      config.memoryUrl = `Build/${memoryFilename}`;
    }

    const symbolsFilename = templateProperty.SYMBOLS_FILENAME();
    if (symbolsFilename) {
      config.symbolsUrl = `Build/${symbolsFilename}`;
    }

    return config;
  }

  SetConfig(config) {
    this.config = config;
  }

  SendMessage(unityObjectName, functionName, value) {
    if (!this.unityInstance) {
      return;
    }
    this.unityInstance.SendMessage(unityObjectName, functionName, value);
    if (templateProperty.DEBUG()) {
      console.log(`UnitySendMessage: ${unityObjectName}.${functionName}(${value})`);
    }
  }

  SetFullscreen(visible) {
    if (!this.unityInstance) {
      console.error('Unity instance not created');
      return;
    }
    this.unityInstance.SetFullscreen(visible ? 1 : 0);
  };

  Quit(onQuitCallBacks) {
    if (!this.unityInstance) {
      console.error('Unity instance not created');
      return;
    }
    this.unityInstance.Quit()
      .then(() => {
        if (onQuitCallBacks && typeof onQuitCallBacks === 'function') {
          onQuitCallBacks();
        }
      })
  };

  async CreateUnityInstance(unityCanvas, handleProgress, callbacks) {
    try {
      if (this.unityInstance) {
        console.error('Unity instance already created');
        return;
      }
      const instance = await createUnityInstance(unityCanvas, this.config, handleProgress);
      this.unityInstance = instance;
      if (templateProperty.DEBUG()) {
        console.log('Unity instance created:', instance);
      };
      if (callbacks && typeof callbacks === 'function') {
        callbacks(instance);
      }
    } catch (error) {
      console.error('Failed to create Unity instance:', error);
    }
  }

  GetUnityInstance() {
    if (!this.unityInstance) {
      console.error('Unity instance not created');
      return;

    }
    return this.unityInstance;
  }

  GetUnityConfig() {
    if (!this.config) {
      console.error('Unity config not set');
      return;

    }
    return this.config;
  }
}

// Ensure only one instance is created
export default new UnityManager();;
