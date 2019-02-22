const PARAM_TOASTIFY_PORT = "toastifyPort";
const PARAM_REDIRECT_URL = "redirectUrl";

function postData(url = "", data = "") {
    return fetch(url, {
            method: "POST",
            mode: "no-cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            redirect: "follow",
            referrer: "no-referrer",
            body: data
        })
        .then(response => response.json());
}

// ================

const urlParams = new URLSearchParams(window.location.search);
if (urlParams.has(PARAM_TOASTIFY_PORT) && urlParams.has(PARAM_REDIRECT_URL)) {
    // Save the port number Toastify is listening to in the browser's app storage
    window.localStorage.setItem(PARAM_TOASTIFY_PORT, urlParams.get(PARAM_TOASTIFY_PORT));

    const redirectUrl = atob(decodeURIComponent(urlParams.get(PARAM_REDIRECT_URL)));
    window.location.replace(redirectUrl);
} else if ((urlParams.has("code") || urlParams.has("error")) && urlParams.has("state")) {
    // Spotify redirect
    var data = `state=${urlParams.has("state")}`;
    if (urlParams.has("code"))
        data += `&code=${urlParams.has("code")}`;
    if (urlParams.has("error"))
        data += `&error=${urlParams.has("error")}`;

    postData(`http://localhost:${window.localStorage.getItem(PARAM_TOASTIFY_PORT)}`, data)
        .then(response => console.log(JSON.stringify(response)))
        .catch(error => console.error(error));
} else {
    // ???
    console.error("???");
}