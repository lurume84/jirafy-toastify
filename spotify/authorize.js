const PARAM_TOASTIFY_PORT = "toastifyPort";
const PARAM_REDIRECT_URL = "redirectUrl";

const urlParams = new URLSearchParams(window.location.search);
if (urlParams.has(PARAM_TOASTIFY_PORT) && urlParams.has(PARAM_REDIRECT_URL)) {
    // Save the port number Toastify is listening to in the browser's app storage
    window.localStorage.setItem(PARAM_TOASTIFY_PORT, urlParams.get(PARAM_TOASTIFY_PORT));

    const redirectUrl = atob(decodeURIComponent(urlParams.get(PARAM_REDIRECT_URL)));
    window.location.replace(redirectUrl);
} else if ((urlParams.has("code") || urlParams.has("error")) && urlParams.has("state")) {
    // Spotify redirect
    $.ajax({
        url: `http://localhost:${window.localStorage.getItem(PARAM_TOASTIFY_PORT)}`,
        method: "POST",
        xhrFields: { "withCredentials": true },
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        data: {
            code: urlParams.get("code"),
            state: urlParams.get("state"),
            error: urlParams.get("error")
        },
        success: (res, textStatus, xhr) => {
            if (xhr.status === 200)
                window.location.replace("https://aleab.github.io/toastify/spotify/done");
            else
                console.error(xhr.status);
        }
    });
} else {
    // ???
    console.error("???");
}