import unityManager from "./unity-manager.js";

const unityWebView =
{
    /* -------------------------------- Property -------------------------------- */
    loaded: [],
    quitButton: null,
    iframe: null,
    emptyPage: window.location.origin + window.location.pathname.replace(/\/[^\/]*$/, '/') + 'empty.html',
    init: function (UnityObjectName) {
        console.log('----- init -----');

        // initialize quit button
        this.quitButton = document.getElementById("quitButton");
        this.quitButton.addEventListener("click", function () {
            unityWebView.setVisibility(UnityObjectName, false);
        });
        this.iframe = document.getElementById('webview_' + UnityObjectName);

        let $containers = $('.webviewContainer');

        if ($containers.length === 0) {
            $('<div style="position: absolute; left: 0px; width: 100%; height: 100%; top: 0px; pointer-events: none;"><div class="webviewContainer" style="overflow: hidden; position: relative; width: 100%; height: 100%; z-index: 1;"></div></div>')
                .appendTo($('#unity-container'));
        }

        const $last = $('.webviewContainer:last');
        // const clonedTop = parseInt($last.css('top')) - 100;
        // const $clone = $last.clone().insertAfter($last).css('top', clonedTop + '%');
        const $iframe =
            $(`<iframe src="${this.emptyPage}" title="WebView" style="position:relative; width:100%; height:100%; border-style:none; display:none; pointer-events:auto;"></iframe>`)
                .attr('id', 'webview_' + UnityObjectName)
                .appendTo($last)
                .on('load', function () {
                    console.log('----- onload -----');


                    $(this).attr('loaded', 'true');
                    const contents = $(this).contents();
                    const w = $(this)[0].contentWindow;
                    contents.find('a').click(function (e) {
                        const href = $.trim($(this).attr('href'));
                        if (href.substr(0, 6) === 'unity:') {
                            unityManager.SendMessage(UnityObjectName, "CallFromJS", href.substring(6, href.length));
                            e.preventDefault();
                        }
                    });
                    contents.find('form').submit(function () {
                        $this = $(this);
                        const action = $.trim($this.attr('action'));
                        if (action.substr(0, 6) === 'unity:') {
                            const message = action.substring(6, action.length);
                            if ($this.attr('method').toLowerCase() == 'get') {
                                message += '?' + $this.serialize();
                            }
                            unityManager.SendMessage(UnityObjectName, "CallFromJS", message);
                            return false;
                        }
                        return true;
                    });

                    // unityWebView.setQuitButton(UnityObjectName);
                    unityManager.SendMessage(UnityObjectName, "CallOnLoaded", location.href);
                });

    },

    sendMessage: function (UnityObjectName, message) {
        unityManager.SendMessage(UnityObjectName, "CallFromJS", message);
    },

    setMargins: function (UnityObjectName, left, top, right, bottom) {
        const container = $('#unity-container');
        const r = (container.hasClass('unity-desktop')) ? window.devicePixelRatio : 1;
        const w0 = container.width() * r;
        const h0 = container.height() * r;
        const canvas = $('#unity-canvas');
        const w1 = canvas.attr('width');
        const h1 = canvas.attr('height');

        const lp = left / w0 * 100;
        const tp = top / h0 * 100;
        const wp = (w1 - left - right) / w0 * 100;
        const hp = (h1 - top - bottom) / h0 * 100;

        this.JQiframe(UnityObjectName)
            .css('left', lp + '%')
            .css('top', tp + '%')
            .css('width', wp + '%')
            .css('height', hp + '%');
    },

    setVisibility: async function (UnityObjectName, visible) {
        const setQuitButton = () => {
            this.quitButton.style.display = "block";
            this.JQiframe(UnityObjectName).off('load', setQuitButton);
        };
        if (visible) {
            this.JQiframe(UnityObjectName).show();
            if (this.JQiframe(UnityObjectName).attr('loaded') === 'true') {
                this.quitButton.style.display = "block";
            } else {
                this.JQiframe(UnityObjectName).on('load', setQuitButton);
            }
        }
        else {
            this.quitButton.style.display = "none";
            this.JQiframe(UnityObjectName).hide();
            this.JQiframe(UnityObjectName).attr('loaded', 'false')[0].contentWindow.location.replace(this.emptyPage);
        }
    },

    loadURL: function (UnityObjectName, url) {
        this.JQiframe(UnityObjectName).attr('loaded', 'false')[0].contentWindow.location.replace(url);
    },

    evaluateJS: function (UnityObjectName, js) {
        $iframe = this.JQiframe(UnityObjectName);
        if ($iframe.attr('loaded') === 'true') {
            $iframe[0].contentWindow.eval(js);
        } else {
            $iframe.on('load', function () {
                $(this)[0].contentWindow.eval(js);
            });
        }
    },

    destroy: function (UnityObjectName) {
        this.JQiframe(UnityObjectName).parent().parent().remove();
    },

    JQiframe: function (UnityObjectName) {
        return $('#webview_' + UnityObjectName);
    },

    /* --------------------------------- Actions -------------------------------- */
};

window.unityWebView = unityWebView;
