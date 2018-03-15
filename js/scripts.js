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

/*==================*/
/* 'Thank You' Page */
/*==================*/

var ty_emotes = [
    0x2764, 0x2764, 0x2764, 0x2764,             // :heart:
    0x01F497, 0x01F497,                         // :heartpulse:
    0x01F44D, 0x01F44D,                         // :+1:
    0x01F62E,                                   // :open_mouth:
    0x01F632,                                   // :astonished:
    0x2615, 0x2615                              // :coffee:
];

function spawnHearts()
{
    var heart = document.createElement('span');
    $(heart).addClass('unselectable ty-heart').hide();
    $(heart).text(String.fromCodePoint(pickRandom(ty_emotes), 0xFE0F));
    $('body').append(heart);
    
    var left = 100 * Math.random();
    var xDelta = (Math.random() - 0.5) * 30;
    var final_left = left + xDelta;
    var final_bottom = 100 * getRandomArbitrary(0.6, 1);
    var animDuration = 10000 * getRandomArbitrary(0.5, 1);
    
    var animProperties = {
        left: final_left + 'vw',
        bottom: final_bottom + 'vh',
    };
    var animOptions = {
        queue: false,
        duration: animDuration,
        complete: function() {
            $(this).remove();
        }
    };
    
    $(heart).css({ left: left + 'vw' })
            .animate(animProperties, animOptions)
            .fadeIn({
                queue: false,
                duration: animDuration * getRandomArbitrary(0.2, 0.5)
            })
            .delay(animDuration * getRandomArbitrary(0.3, 0.6))
            .fadeOut(animDuration * getRandomArbitrary(0.1, 0.3));
    
    setTimeout(spawnHearts, 1000 * getRandomArbitrary(1, 2));
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