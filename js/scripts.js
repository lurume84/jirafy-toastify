if (!String.format)
{
    String.format = function(format)
    {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function(match, number) { 
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

function emojify()
{
    $('img.emoji').each(function(index) {
        var title = $(this)[0].title;
        var src = $(this)[0].src;
        var code = src.replace(/.*\/emoji\/unicode\/([0-9A-F]+)\.png/i, '0x$1');
        $(this).replaceWith($('<g-emoji class="emoji" title="' + title + '" fallback-src="' + src + '">' + String.fromCodePoint(code) + '</g-emoji>'));
    });
}

function correctString(str)
{
    return str.trim()
              .replace(/(.*\.)([A-Z].*)/g, '$1 $2')
              .replace(/(.*,)(\S+.*)/ig, '$1 $2');
}

function generateMailTo()
{
    $('email').each(function(index) {
        var local = $(this)[0].dataset['local'] || $(this)[0].getAttribute('data-local');
        var domain = $(this)[0].dataset['domain'] || $(this)[0].getAttribute('data-domain');
        var text = $(this)[0].dataset['text'] || $(this)[0].getAttribute('data-text');
        var subject = $(this)[0].dataset['subject'] || $(this)[0].getAttribute('data-subject');

        var address = local + '@' + domain;
        var aHref = 'mailto:' + address + (subject !== undefined && subject ? '?subject=' + subject : '');
        var aText = text !== undefined && text ? text : address;

        $(this).replaceWith($('<a href="' + aHref + '">' + aText + '</a>'));
    });
}

/*-------*/
/* MATHS */
/*-------*/

function getRandomArbitrary(min, max)
{
    return Math.random() * (max - min) + min;
}

function pickRandom(array)
{
    var i = Math.floor(Math.random() * array.length);
    return array[i];
}