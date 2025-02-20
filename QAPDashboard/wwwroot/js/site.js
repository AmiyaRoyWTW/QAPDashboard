function poll(fn) {
    let pollOnPage = 10 * 1000;
    let pollOffPage = 60 * 1000;
    let pollTime = document.hasFocus() ? pollOnPage : pollOffPage;

    let sleep = () => new Promise(resolve => {
        let sleepStart = new Date();
        let sleepCheck = () => {
            let timeDiff = new Date() - sleepStart;
            if (timeDiff >= pollTime) {
                resolve();
            } else {
                setTimeout(sleepCheck, 1000);
            }
        };
        setTimeout(sleepCheck, 0);
    });

    let poller = () => {
        new Promise((resolve, reject) => {
            fn(resolve, reject);
        }).then(
            () => sleep().then(poller),
            () => console.log('result polling stopped')
        );
    };

    setTimeout(poller, 0);

    window.addEventListener("focus", () => {
        pollTime = pollOnPage;
    }, false);

    window.addEventListener("blur", () => {
        pollTime = pollOffPage;
    }, false);
}

function escape(string) {
    let htmlEscapes = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#x27;',
        '/': '&#x2F;'
    };

    let htmlEscaper = /[&<>"'\/]/g;
    return ('' + string).replace(htmlEscaper, (match) => htmlEscapes[match]);
}