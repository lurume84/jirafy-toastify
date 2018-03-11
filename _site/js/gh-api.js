function listTopics(repository, callback)
{
    $.ajax({
        url: 'https://api.github.com/repos/' + repository + '/topics',
        type: 'GET',
        headers: { 'Accept': 'application/vnd.github.mercy-preview+json' }
    }).done(callback);
}

function getLatestRelease(repository, callback)
{
    $.get('https://api.github.com/repos/' + repository + '/releases/latest', null, callback);
}