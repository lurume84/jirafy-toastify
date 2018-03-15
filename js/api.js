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
function getRandomQuote(seed, doneCallback)
{
    if (seed === undefined || seed == null)
        seed = Math.floor(Math.random() * 100000);
    $.ajax({
        method: 'GET',
        url: 'https://api.forismatic.com/api/1.0/?lang=en&method=getQuote&key=' + seed + '&format=jsonp&jsonp=getRandomQuote_Callback',
        dataType: 'jsonp'
    }).always(doneCallback);
}