This site is built using [Jekyll](https://jekyllrb.com/).

To run locally, make sure you have `ruby 2.3.1p112+` installed.

Then run:

```
gem install bundler
bundle
bundle exec jekyll server --watch
```

The website will be hosted at `http://127.0.0.1:4000/`.

Syntax highlighting is the one
[built in Jekyll](http://jekyllrb.com/docs/templates/#code-snippet-highlighting)
and provided by [Rouge](http://rouge.jneen.net/).

To find the appropriate identifier to use for the language you
want to highlight, look up on the
[Rouge wiki](https://github.com/jneen/rouge/wiki/List-of-supported-languages-and-lexers).
