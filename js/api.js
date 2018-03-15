/* GitHub */
function listTopics(repository, callback)
{
    $.ajax({
        url: 'https://api.github.com/repos/' + repository + '/topics',
        method: 'GET',
        headers: { 'Accept': 'application/vnd.github.mercy-preview+json' }
    }).done(callback);
}

function getLatestRelease(repository, callback)
{
    $.get('https://api.github.com/repos/' + repository + '/releases/latest', null, callback);
}

/* https://forismatic.com/en/api/ */
function getRandomQuote(key, callback)
{
    if (key === undefined || key == null)
        key = Math.floor(Math.random() * 100000);
    $.getJSON('https://api.forismatic.com/api/1.0/?lang=en&format=json&method=getQuote&key=' + key, callback);
}