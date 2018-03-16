/*==================*/
/* 'Thank You' Page */
/*==================*/

$(document).ready(function()
{
    jQuery.fx.interval = 22;

    var heartsContainer = document.createElement('div');
    $(heartsContainer).addClass('ty-hearts_container');
    $('body').append(heartsContainer);
    spawnHearts(heartsContainer);

    getRandomQuote(null, function() {
        $('#ty_container #ty').hide(0).removeClass('not-visible').fadeIn(700);
    });
});

function getRandomQuote_Callback(quote)
{
    var jqQuoteContainer = $('#quote_container');
    var jqQuote = $('#quote');
    jqQuote.empty();

    var el = document.createElement('span');
    el.innerHTML = '\u201C' + correctString(quote.quoteText) + '\u201D';
    jqQuote.append(el);

    el = document.createElement('span');
    el.innerHTML = '~ ';
    var author = quote.quoteAuthor.trim();
    if (author !== undefined && author) {
        var a = document.createElement('a');
        a.innerHTML = author;
        a.href = encodeURI('https://en.wikipedia.org/wiki/' + author);
        a.target = '_blank';
        $(el).append(a);
    } else {
        el.innerHTML += '[Unknown]';
    }
    jqQuote.append(el);

    if (quote.quoteLink !== undefined && quote.quoteLink) {
        $('#quote_container #quote_link')[0].href = quote.quoteLink;
    }

    setTimeout(function() {
        jqQuoteContainer.hide(0).removeClass('not-visible').fadeIn(800);
    }, 800);
}

var ty_emotes = [
    0x2764, 0x2764, 0x2764, 0x2764,             // :heart:
    0x01F497, 0x01F497,                         // :heartpulse:
    0x01F44D, 0x01F44D,                         // :+1:
    0x01F62E,                                   // :open_mouth:
    0x01F632,                                   // :astonished:
    0x2615, 0x2615                              // :coffee:
];

function spawnHearts(container)
{
    var heart = document.createElement('span');
    $(heart).addClass('unselectable ty-heart').hide();
    $(heart).text(String.fromCodePoint(pickRandom(ty_emotes), 0xFE0F));
    $(container ? container : 'body').append(heart);

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

    setTimeout(spawnHearts, 1000 * getRandomArbitrary(1, 2), container);
}