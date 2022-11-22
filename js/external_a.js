function ready(fn) {
    if (document.readyState != 'loading') {
      fn();
    } else if (document.addEventListener) {
      document.addEventListener('DOMContentLoaded', fn);
    } else {
      document.attachEvent('onreadystatechange', function() {
        if (document.readyState != 'loading')
          fn();
      });
    }
}

ready(function() {
    var website = window.location.hostnam;
    var internalLinkRegex = new RegExp('^((((http:\\/\\/|https:\\/\\/)(www\\.)?)?'
        + website
        + ')|(localhost:\\d{4})|(\\/.*))(\\/.*)?$', '');

    var articles = document.getElementsByClassName('article-body');
    for (var articleIndex = 0; articleIndex < articles.length; articleIndex++)
    {
      var anchorEls = articles[articleIndex].querySelectorAll('a');

      for (var i = 0; i < anchorEls.length; i++) {
          var anchorEl = anchorEls[i];
          var href = anchorEl.getAttribute('href');
          if (!internalLinkRegex.test(href)) {
              anchorEl.setAttribute('target', '_blank');
              anchorEl.setAttribute('rel', 'noopener noreferrer');

              if (!anchorEl.className.includes('no-icon'))
              {
                  const space = document.createElement('span');
                  space.innerHTML = ' ';
                  anchorEl.appendChild(space);
                  const icon = document.createElement('i');
                  icon.className = 'fas fa-external-link-alt';
                  anchorEl.appendChild(icon);
              }
          }
      }
    }
    
});

// card-content